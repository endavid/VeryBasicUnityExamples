using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float minRotation = 30f;
    public float maxRotation = 300f;
    public float acceleration = 0.1f;

    float a = 0.1f;
    float rotationSpeed = 30f;

    // Start is called before the first frame update
    void Start()
    {
        a = acceleration;
        rotationSpeed = minRotation;
    }

    // Update is called once per frame
    void Update()
    {
        rotationSpeed += a;
        if (rotationSpeed > maxRotation || rotationSpeed < minRotation)
        {
            a = -a;
        }
        transform.Rotate(new Vector3(0,0,1), rotationSpeed * Time.deltaTime);
    }
}
