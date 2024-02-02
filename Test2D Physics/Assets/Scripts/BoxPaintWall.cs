using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPaintWall : MonoBehaviour
{
    public float marginTop = 0;
    public float marginLeft = 0;
    public float marginBottom = 0;
    public float marginRight = 0;
    public Color paintColor = Color.white;

    // Start is called before the first frame update
    void Start()
    {
        var collider = gameObject.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(1f + marginLeft + marginRight, 1f + marginTop + marginBottom);
        collider.offset = new Vector2((marginRight-marginLeft)*0.5f,(marginTop-marginBottom) * 0.5f);
        PhysicsMaterial2D physicsMaterial = new()
        {
            friction = 0.1f,
            bounciness = 0.8f
        };
        var body = gameObject.AddComponent<Rigidbody2D>();
        body.bodyType = RigidbodyType2D.Kinematic;
        body.gravityScale = 0;
        collider.sharedMaterial = physicsMaterial;
        // we need a separate collider for the triggers
        var triggerCollider = gameObject.AddComponent<BoxCollider2D>();
        triggerCollider.size = collider.size;
        triggerCollider.offset = collider.offset;
        triggerCollider.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball"))
        {
            var renderer = other.gameObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = paintColor;
            }
        }
    }
}
