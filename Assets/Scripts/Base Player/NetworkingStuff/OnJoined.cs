using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnJoined : MonoBehaviour
{
    private void Awake(){
        if(PlayerPrefs.GetString("GameMode") != "SinglePlayer"){
            FindObjectOfType<GameMode>().GetComponent<JoinedServer>().Joined();
            //Debug.Log("DOing thing");
        }

    }
}
