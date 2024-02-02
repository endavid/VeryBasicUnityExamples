using System.Linq;
using UnityEngine;

public class PolylinePaintWall : MonoBehaviour
{
    public Vector3[] positions;
    public Color color = Color.white;
    public float lineWidth = 0.2f;
    public float bounceForce = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        var material = new Material(Shader.Find("Sprites/Default"))
        {
            color = color
        };
        var renderer = gameObject.AddComponent<LineRenderer>();
        renderer.material = material;
        renderer.widthMultiplier = lineWidth;
        renderer.positionCount = positions.Length;
        renderer.SetPositions(positions);
        var collider = gameObject.AddComponent<EdgeCollider2D>();
        var points = positions.ToList().ConvertAll(p => new Vector2(p.x, p.y));
        collider.SetPoints(points);
        PhysicsMaterial2D physicsMaterial = new()
        {
            friction = 0.1f,
            bounciness = 0.8f
        };
        var body = gameObject.AddComponent<Rigidbody2D>();
        //body.bodyType = RigidbodyType2D.Static;
        // Instead of making it static, make it kinematic
        // so I can move it, but it won't be affectect by others (=!dynamic)
        body.bodyType = RigidbodyType2D.Kinematic;
        body.gravityScale = 0;
        collider.sharedMaterial = physicsMaterial;
        // we need a separate collider for the triggers
        var triggerCollider = gameObject.AddComponent<EdgeCollider2D>();
        triggerCollider.SetPoints(points);
        triggerCollider.isTrigger = true;
        UpdateLines();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLines();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball"))
        {
            Debug.Log($"{other.name} hit the edge!");
            var renderer = other.gameObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = color;
            }
            var body = other.gameObject.GetComponent<Rigidbody2D>();
            if (body != null)
            {
                body.AddForce(RandomUpwardForce(), ForceMode2D.Impulse);
            }
        }
    }

    private Vector2 RandomUpwardForce()
    {
        var randomDirection = Random.insideUnitCircle;
        // always upwards, and not very lateral
        randomDirection.y = 0.5f * System.Math.Abs(randomDirection.y) + 0.5f;
        randomDirection.x *= 0.5f;
        return randomDirection.normalized * bounceForce;
    }

    private void UpdateLines()
    {
        var renderer = gameObject.GetComponent<LineRenderer>();
        if (renderer != null)
        {
            for (int i = 0; i < positions.Length; i++)
            {
                var p = transform.TransformPoint(positions[i]);
                renderer.SetPosition(i, p);
            }
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.magenta;
        var collider = gameObject.GetComponent<EdgeCollider2D>();
        if (collider != null)
        {
            var points = collider.points.ToList();
            points.ForEach(p => Gizmos.DrawSphere(transform.TransformVector(p), 0.1f));
        }
    }
}
