using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
namespace StarterAssets
{
public class Pause : MonoBehaviour
{
    private bool paused;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(!paused){
                //Debug.Log("Opened Pause Menu");
                Cursor.lockState = CursorLockMode.None;
                GetComponent<ThirdPersonController>().LockCameraPosition = true;
                paused = true;
            }
            else if(paused){
                //Debug.Log("Closed Pause Menu");
                paused = false;
                GetComponent<ThirdPersonController>().LockCameraPosition = false;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}
}
