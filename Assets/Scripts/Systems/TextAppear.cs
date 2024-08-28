using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextAppear : MonoBehaviour
{
    [SerializeField] private float appearDistance;
    private Transform player;
    private void Start(){
        player = FindObjectOfType<ThirdPersonController>().transform;
    }
    private void FixedUpdate(){
        if(Vector3.Distance(player.position, this.transform.position) <= appearDistance){
            this.GetComponent<MeshRenderer>().enabled = true;
        }
        else{
            this.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
