using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    [SerializeField] GameObject pMenu;
    [SerializeField] GameObject sMenu;
    [SerializeField] GameObject rMenu;
    [SerializeField] GameObject jMenu;
    [SerializeField] TabGroup journalTabs;
    [SerializeField] GameObject background;
    public bool isPaused;
    public void OnPause(){
        pMenu.SetActive(true);
        background.SetActive(true);
        isPaused = true;
    }
    public void UnPause(){
        pMenu.SetActive(false);
        sMenu.SetActive(false);
        rMenu.SetActive(false);
        jMenu.SetActive(false);
        journalTabs.ClosePage();
        background.SetActive(false);
        isPaused = false;

    }
    private void Start(){
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void UnpausedFromButton(){
        //sMenu.SetActive(true);
    }
}
