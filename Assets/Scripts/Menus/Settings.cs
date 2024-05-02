using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Settings : MonoBehaviour
{
    //public int _fullscreenMode = 0;
    public void Start(){
        ScreenMode(PlayerPrefs.GetInt("ScreenMode"));
    }
    public void ScreenMode(int mode){
        //_fullscreenMode = mode;
        PlayerPrefs.SetInt("ScreenMode", mode);
                if(mode == 0){
                    Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                    Debug.Log("Exclusive Fullscreen");
                }
                if(mode == 1){
                    Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                    Debug.Log("Borderless Windowed");
                }
                if(mode == 2){
                Screen.fullScreenMode = FullScreenMode.Windowed;
                Debug.Log("Windowed");
                }
            
    }
}
