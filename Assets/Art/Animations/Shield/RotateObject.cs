using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    // Rotation speed in degrees per second
    public float rotationSpeed = 360f;

    void Update()
    {
        // Rotate around the Y axis
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}
