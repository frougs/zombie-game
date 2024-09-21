using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupBase : MonoBehaviour
{
 public float floatAmount = 0.5f; // Amount of vertical float
    public float floatSpeed = 1.0f;  // Speed of the float movement
    public float rotationSpeed = 50.0f; // Speed of rotation
    [SerializeField] string powerupName;
    [SerializeField] GameObject pickupParticles;

    private Vector3 startPos;

    [SerializeField] float selfDestructTime;
    float minDist = Mathf.Infinity;
    GameObject closestBarrier = null;
    public bool insideRoom = false;
    private bool detectRoom = false;
    private bool ranCollisionCheck = false;

    void Start()
    {
        startPos = this.gameObject.transform.position; // Save the starting position
        if(selfDestructTime != 0){
            Destroy(this.gameObject, selfDestructTime);
        }
        StartCoroutine(WaitForRoomDetection());
    }
    private IEnumerator WaitForRoomDetection(){
        yield return new WaitForSeconds(0.2f);
        detectRoom = true;
    }

    void Update()
    {
        // Float up and down
        float newY = Mathf.Sin(Time.time * floatSpeed) * floatAmount;
        this.gameObject.transform.position = new Vector3(startPos.x, startPos.y + newY, startPos.z);

        // Rotate
        this.gameObject.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        if(insideRoom == false && detectRoom){
            MoveToSpawnPoint();
        }
        if(!ranCollisionCheck){
            CheckCollisions();
        }
    }
    private void OnTriggerEnter(Collider other){
        if(other.gameObject.GetComponent<ThirdPersonController>() != null){
            other.gameObject.GetComponent<PowerupController>().PowerUpCollected(powerupName);
            Instantiate(pickupParticles, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
    // private void OnTriggerStay(Collider other){
    //     if(other.gameObject.GetComponent<RoomDetection>() != null){
    //         insideRoom = true;
    //         detectRoom = false;
    //     }
    // }
private void CheckCollisions() {
    RoomDetection[] roomColliders = FindObjectsOfType<RoomDetection>();
    bool isInsideRoom = false;

    foreach (RoomDetection roomCollider in roomColliders) {
        if (GetComponent<Collider>().bounds.Intersects(roomCollider.GetComponent<Collider>().bounds)) {
            isInsideRoom = true;
            break;
        }
    }

    this.insideRoom = isInsideRoom;
    ranCollisionCheck = true;
}


    private void MoveToSpawnPoint(){
            Debug.Log(this.gameObject.name + " is NOT Touching room detection");
            var barriers = FindObjectsOfType<BarrierScript>();
            foreach(var barrier in barriers){
                float dist = Vector3.Distance(barrier.gameObject.transform.position, this.transform.position);
                if(dist < minDist){
                    closestBarrier = barrier.gameObject;
                    minDist = dist;
                }
            }
            Debug.Log("Closest Barrier: " +closestBarrier.gameObject.name);
            Vector3 spawnPoint = closestBarrier.GetComponent<BarrierScript>().powerupSpawnPoint.transform.position;
            transform.position = spawnPoint;
    }
}
