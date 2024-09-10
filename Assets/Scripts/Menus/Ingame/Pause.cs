using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pause : MonoBehaviour
{
    private bool paused;
    public PlayerInput _pInput;
    [HideInInspector] public InputAction pause;
    private Alteruna.Avatar _avatar;
    [HideInInspector] public bool inMenu;

    void Update()
    {
        // if (!_avatar.IsMe)
        //     return;
        if(pause.triggered){
            if(!paused && !inMenu){
                //Debug.Log("Opened Pause Menu");
                PauseMenuSingleton.instance.GetComponent<PauseController>().OnPause();
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                //GetComponent<ThirdPersonController>().LockCameraPosition = true;
                this.GetComponent<ThirdPersonController>().enabled = false;
                paused = true;
                Time.timeScale = 0f;
            }
            else if(paused){
                PauseMenuSingleton.instance.GetComponent<PauseController>().UnPause();
                PauseMenuSingleton.instance.GetComponent<PauseController>().UnpausedFromButton();
                //Debug.Log("Closed Pause Menu");
                paused = false;
                this.GetComponent<ThirdPersonController>().enabled = true;
                GetComponent<ThirdPersonController>().LockCameraPosition = false;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Time.timeScale = 1.0f;
            }
        }
    }
    void OnEnable(){
    }
    void Start(){
        // _avatar = GetComponent<Alteruna.Avatar>();
        //         if (!_avatar.IsMe)
        //     return;
        _pInput = GetComponent<PlayerInput>();
        pause = _pInput.actions["Pause"];
    }
}

