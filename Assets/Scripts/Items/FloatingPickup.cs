using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPickup : MonoBehaviour
{
    public float floatAmount = 0.5f; // Amount of vertical float
    public float floatSpeed = 1.0f;  // Speed of the float movement
    public float rotationSpeed = 50.0f; // Speed of rotation

    private Vector3 startPos;

    [SerializeField] float selfDestructTime;

    void Start()
    {
        startPos = this.gameObject.transform.position; // Save the starting position
        if(selfDestructTime != 0){
            Destroy(this.gameObject, selfDestructTime);
        }
    }

    void Update()
    {
        // Float up and down
        float newY = Mathf.Sin(Time.time * floatSpeed) * floatAmount;
        this.gameObject.transform.position = new Vector3(startPos.x, startPos.y + newY, startPos.z);

        // Rotate
        this.gameObject.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}
