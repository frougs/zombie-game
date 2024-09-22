using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprintAssist : MonoBehaviour
{
    ThirdPersonController player;
    [SerializeField] float sprintFOVAddititve;
    private void Start(){
        player = this.GetComponent<ThirdPersonController>();
    }
    private void Update(){
        if(player != null){
            if(player.isSprinting){
                var cam = FindObjectOfType<FOVController>();
                float sprintFOV = PlayerPrefs.GetFloat("FOV") + sprintFOVAddititve;
                float sprintFOVTransitionTime = player._speed;
                sprintFOVTransitionTime = Mathf.Clamp(sprintFOVTransitionTime/player.SprintSpeed, 0f, 1f);
                cam.FOVChange(sprintFOV, sprintFOVTransitionTime);
            }
            else if(!player.isSprinting && player.sprintingBlocked == false){
                //CameraSingleton.instance.gameObject.GetComponent<FOVController>().canRevert = true;
                FindObjectOfType<FOVController>().canRevert = true;
            }
            
        }
    }
}
