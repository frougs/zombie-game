using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuSingleton : MonoBehaviour
{
    public static PauseMenuSingleton instance;
    private void Awake(){
        if(instance == null){
            instance = this;
        }
    }
}
