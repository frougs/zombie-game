using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
namespace StarterAssets
{
    public class UserInputs : MonoBehaviour
    {
        public static UserInputs instance;
        private PlayerInput pInput;
        //All Input Actions
        [HideInInspector] public InputAction move;
        [HideInInspector] public InputAction attack;
        [HideInInspector] public InputAction jump;
        [HideInInspector] public InputAction sprint;
        [HideInInspector] public InputAction look;
        [HideInInspector] public InputAction aim;
        [HideInInspector] public InputAction crouch;
        [HideInInspector] public InputAction pause;

        private void Awake(){
            if(instance == null){
                instance = this;
            }
            pInput = GetComponent<PlayerInput>();
        }
        private void OnEnable(){
            move = pInput.actions["Move"];
            attack = pInput.actions["Attack"];
            jump = pInput.actions["Jump"];
            sprint = pInput.actions["Sprint"];
            look = pInput.actions["Look"];
            aim = pInput.actions["Aim"];
            crouch = pInput.actions["Crouch"];
            pause = pInput.actions["Pause"];
        }
    }
}
