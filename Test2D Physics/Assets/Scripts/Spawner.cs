using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public int count = 10;
    public Sprite sprite;
    public float initialForce = 5f;
    public float scaleMin = 1f;
    public float scaleMax = 1f;

    private List<GameObject> balls = new();

    // Start is called before the first frame update
    void Start()
    {
        PhysicsMaterial2D material = new()
        {
            friction = 0.2f,
            bounciness = 0.4f
        };
        var name = gameObject.name;
        for (int i = 0; i < count; i++)
        {
            GameObject obj = new($"{name}_obj_{i}");
            obj.tag = "Ball";
            var s = Random.Range(scaleMin, scaleMax);
            obj.transform.localScale = new Vector3(s,s,1f);
            var renderer = obj.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            var body = obj.AddComponent<Rigidbody2D>();
            body.mass = 1f;
            body.gravityScale = 1f;
            body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            body.interpolation = RigidbodyInterpolation2D.Interpolate;
            body.AddForce(RandomUpwardForce(), ForceMode2D.Impulse);
            var collider = obj.AddComponent<CircleCollider2D>();
            collider.radius = 0.5f;
            collider.sharedMaterial = material;
            balls.Add(obj);
        }
        Debug.Log(Camera.main.aspect);
        Debug.Log(Camera.main.orthographicSize);
    }

    // Update is called once per frame
    void Update()
    {
        OutOfBoundsCheck();
    }

    private void OutOfBoundsCheck()
    {
        float size = Camera.main.orthographicSize;
        float cameraWidth = size * Camera.main.aspect;
        float cameraHeight = size;
        balls.ForEach(b => {
            var x = b.transform.position.x;
            var y = b.transform.position.y;
            if (System.Math.Abs(x) > cameraWidth || System.Math.Abs(y) > cameraHeight)
            {
                b.transform.position = new Vector3(0,0,0);
                var body = b.GetComponent<Rigidbody2D>();
                if (body != null)
                {
                    body.AddForce(RandomUpwardForce(), ForceMode2D.Impulse);
                }
            }
        });
    }

    private Vector2 RandomUpwardForce()
    {
        var randomDirection = Random.insideUnitCircle;
        // always upwards, and not very lateral
        randomDirection.y = 0.5f * System.Math.Abs(randomDirection.y) + 0.5f;
        randomDirection.x *= 0.5f;
        return randomDirection.normalized * initialForce;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.magenta;
        balls.ForEach(b => Gizmos.DrawWireSphere(b.transform.position, b.transform.localScale.x * 0.5f));
    }
}
