using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameMode : MonoBehaviour
{
    public UnityEvent SinglePlayer;
    public UnityEvent MultiPlayer;
    private string _gameM;
    public void SetGameMode(string mode){
        PlayerPrefs.SetString("GameMode", mode);
    }
    private void GetGameMode(){
        _gameM = PlayerPrefs.GetString("GameMode");
    }
    private void Awake(){
        GetGameMode();
        if(_gameM == "SinglePlayer"){
            SinglePlayer?.Invoke();
        }
        if(_gameM == "MultiPlayer"){
            MultiPlayer?.Invoke();
        }
    }

}
