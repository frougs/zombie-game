using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MapHighScore : MonoBehaviour
{
    [SerializeField] string sceneName;
    [SerializeField] TextMeshProUGUI hsText;
    [SerializeField] GameObject crownObj;
    private void OnEnable(){
        if(PlayerPrefs.HasKey(sceneName)){
            hsText.text = ": " +PlayerPrefs.GetInt(sceneName).ToString();
            crownObj.SetActive(true);
        }
        else{
            hsText.gameObject.SetActive(false);
            crownObj.SetActive(false);
        }
    }
}
