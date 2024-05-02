using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    [SerializeField] GameObject pMenu;
    [SerializeField] GameObject sMenu;
    [SerializeField] GameObject rMenu;
    [SerializeField] GameObject background;
    public void OnPause(){
        pMenu.SetActive(true);
        background.SetActive(true);
    }
    public void UnPause(){
        pMenu.SetActive(false);
        sMenu.SetActive(false);
        rMenu.SetActive(false);
        background.SetActive(false);
    }
    public void UnpausedFromButton(){
        //sMenu.SetActive(true);
    }
}
