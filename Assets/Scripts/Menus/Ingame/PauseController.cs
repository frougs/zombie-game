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
    [SerializeField] AudioClip pauseMusic;
    public bool isPaused;
    private UIContainer uiStuff;
    public Pause playerPauseScript;
    public void OnPause(){
        pMenu.SetActive(true);
        background.SetActive(true);
        isPaused = true;
        uiStuff.PauseAllSounds();
        uiStuff.UpdateMenuMusic(pauseMusic, true);
    }
    public void UnPause(){
        pMenu.SetActive(false);
        sMenu.SetActive(false);
        rMenu.SetActive(false);
        jMenu.SetActive(false);
        journalTabs.ClosePage();
        background.SetActive(false);
        isPaused = false;
        uiStuff.UnpauseAllSounds();
        uiStuff.UpdateMenuMusic(null, false);

    }
    private void Start(){
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerPauseScript = FindObjectOfType<Pause>();
    }
    public void UnpausedFromButton(){
        //sMenu.SetActive(true);
    }
    void Update()
    {
        if(uiStuff == null){
            uiStuff = FindObjectOfType<UIContainer>();
        }
    }
    public void UnPausedFromMenu(){
        playerPauseScript.UnPause();
    }
}
