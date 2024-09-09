using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CloseFromDistance : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform player;
    [SerializeField] float closeDistance;
    public UnityEvent close;
    private void Start(){
        player = FindObjectOfType<ThirdPersonController>().transform;
    }
    private void FixedUpdate(){
        if(Vector3.Distance(player.position, this.transform.position) >= closeDistance){
            close?.Invoke();
        }
    }
}
