using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public GameObject LoadingScreen;
    public Slider bar;
    public TextMeshProUGUI percent;
    public void QuitGame(){
        Application.Quit();

    }
    public void LoadScene(string sceneName){
        StartCoroutine(LoadSceneAsync(sceneName));
    }
    IEnumerator LoadSceneAsync(string scene_name)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene_name);

        LoadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progressValue=Mathf.Clamp01(operation.progress/0.9f);

            //LoadingBarFill.fillAmount = progressValue;
            bar.value = progressValue;
            percent.text = progressValue.ToString() +"%";

            yield return null;
        }
    }
}
