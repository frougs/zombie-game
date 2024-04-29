using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pause : MonoBehaviour
{
    private bool paused;

    void Update()
    {
        if(UserInputs.instance.pause.triggered){
            if(!paused){
                //Debug.Log("Opened Pause Menu");
                Cursor.lockState = CursorLockMode.None;
                GetComponentInChildren<FirstPersonLook>().ToggleLook(false);
                paused = true;
            }
            else if(paused){
                //Debug.Log("Closed Pause Menu");
                paused = false;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.lockState = CursorLockMode.Confined;
                GetComponentInChildren<FirstPersonLook>().ToggleLook(true);
            }
        }
    }
}
