using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatchNotesScript : MonoBehaviour
{
    [SerializeField] GameObject unreadObj;
    public void Read(){
        PlayerPrefs.SetInt("PatchNotesRead", 1);
        unreadObj.SetActive(false);
    }
    private void OnEnable(){
        if(PlayerPrefs.GetInt("PatchNotesRead") == 0 || !PlayerPrefs.HasKey("PatchNotesRead")){
            unreadObj.SetActive(true);
        }
        else{
            unreadObj.SetActive(false);
        }
    }
}
