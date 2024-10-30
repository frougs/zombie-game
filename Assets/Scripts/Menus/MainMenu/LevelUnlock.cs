using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelUnlock : MonoBehaviour
{
[SerializeField] string previousSceneName;
public UnityEvent onLevelUnlock;
    private void OnEnable(){
        if(PlayerPrefs.HasKey(previousSceneName)){
            if(PlayerPrefs.GetInt(previousSceneName) >= 0){
                onLevelUnlock?.Invoke();
            }
        }
    }
}
