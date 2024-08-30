using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToFace : MonoBehaviour
{
    private Transform target;
    //[SerializeField] bool keepUpright
    [SerializeField] private float damping;

    private void FixedUpdate(){
        if(target != null){
            //Debug.Log(target.gameObject.name);
            var lookPos = target.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping); 
        }
        else{
            /*Debug.Log("No Target Found");
            var avatars = FindObjectsOfType<ThirdPersonController>();
            foreach(ThirdPersonController avatar in avatars){
                if(avatar.GetComponent<Avatar>() != null){
                    target = avatar.gameObject.transform;
                }
            }
            */
            //Debug.Log("No Target Found");
            target = FindObjectOfType<ThirdPersonController>().gameObject.transform;
        }
    }
}