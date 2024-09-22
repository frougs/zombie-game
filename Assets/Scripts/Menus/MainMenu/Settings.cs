using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Cinemachine;
using UnityEngine.SceneManagement;
public class Settings : MonoBehaviour
{
    //public int _fullscreenMode = 0;
    [Header("Default Values")]
    float defaultSens = 4f;
    float defaultMusicVolume = 0.5f;
    float defaultTrackVolume = 0.25f;
    float defaultSFXVolume = 0.5f;
    int defaultScreenMode = 0;
    float defaultFOV = 60f;
    [Header("Slider Objects")]
    [SerializeField] Slider sfxSliderObject;
    [SerializeField] Slider musicSliderObject;
    [SerializeField] Slider sensSliderObject;
    [SerializeField] Slider ingameTrackSliderObject;
    [SerializeField] Slider fovSliderObject;
    public void Start(){
        ScreenMode(PlayerPrefs.GetInt("ScreenMode"));

        //Check to see if saved player prefs exist, if not create them and assign default values, if they do, load all settings with the saved values
        if(PlayerPrefs.HasKey("Sensitivity")){
            UpdateSens(PlayerPrefs.GetFloat("Sensitivity"));
        }
        else{
            UpdateSens(defaultSens);
        }
        if(PlayerPrefs.HasKey("MusicVolume")){
            MusicVolumeSlider(PlayerPrefs.GetFloat("MusicVolume"));
        }
        else{
            MusicVolumeSlider(defaultMusicVolume);
        }
        if(PlayerPrefs.HasKey("SFXVolume")){
            SFXVolumeSlider(PlayerPrefs.GetFloat("SFXVolume"));
        }
        else{
            SFXVolumeSlider(defaultSFXVolume);
        }
        if(PlayerPrefs.HasKey("TrackVolume")){
            IngameTrackVolumeSlider(PlayerPrefs.GetFloat("TrackVolume"));
        }
        else{
            IngameTrackVolumeSlider(defaultTrackVolume);
        }
        if(PlayerPrefs.HasKey("FOV")){
            FOVSlider(PlayerPrefs.GetFloat("FOV"));
        }
        else{
            FOVSlider(defaultFOV);
        }
    }
    public void ScreenMode(int mode){
        //_fullscreenMode = mode;
        PlayerPrefs.SetInt("ScreenMode", mode);
                if(mode == 0){
                    Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                    //Debug.Log("Exclusive Fullscreen");
                }
                if(mode == 1){
                    Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                    //Debug.Log("Borderless Windowed");
                }
                if(mode == 2){
                    Screen.fullScreenMode = FullScreenMode.Windowed;
                    //Debug.Log("Windowed");
                }
            
    }
    public void FOVSlider(float value){
        PlayerPrefs.SetFloat("FOV", value);
        fovSliderObject.value = PlayerPrefs.GetFloat("FOV");
    }
    public void IngameTrackVolumeSlider(float value){
        PlayerPrefs.SetFloat("TrackVolume", value);
        AudioSource[] audioSources = Resources.FindObjectsOfTypeAll<AudioSource>();
        foreach(AudioSource a in audioSources){
            if(a.tag == "TrackMusic"){
                a.volume = PlayerPrefs.GetFloat("TrackVolume");
            }
        }
        ingameTrackSliderObject.value = PlayerPrefs.GetFloat("TrackVolume");
    }
    public void MusicVolumeSlider(float value){
        PlayerPrefs.SetFloat("MusicVolume", value);
        AudioSource[] audioSources = Resources.FindObjectsOfTypeAll<AudioSource>();
        foreach (AudioSource a in audioSources){
            if(a.tag == "Music"){
                a.volume = PlayerPrefs.GetFloat("MusicVolume");
                
                
            }
        }
        musicSliderObject.value = PlayerPrefs.GetFloat("MusicVolume");
    }

    public void SFXVolumeSlider(float value){
        PlayerPrefs.SetFloat("SFXVolume", value);
        AudioSource[] audioSources = Resources.FindObjectsOfTypeAll<AudioSource>();
        foreach (AudioSource a in audioSources){
            if(a.tag == "SFX"){
                a.volume = PlayerPrefs.GetFloat("SFXVolume");
                
            }
        }
        sfxSliderObject.value = PlayerPrefs.GetFloat("SFXVolume");
    }
    public void UpdateSens(float value){
        PlayerPrefs.SetFloat("Sensitivity", value);
        var player = FindObjectOfType<ThirdPersonController>();
        if(player != null){
            player.lookSens = PlayerPrefs.GetFloat("Sensitivity");
        }
        sensSliderObject.value = PlayerPrefs.GetFloat("Sensitivity");
    }
    public void ResetToDefault(){
        ScreenMode(defaultScreenMode);
        MusicVolumeSlider(defaultMusicVolume);
        SFXVolumeSlider(defaultSFXVolume);
        UpdateSens(defaultSens);
        IngameTrackVolumeSlider(defaultTrackVolume);
        FOVSlider(defaultFOV);
    }
}
