using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreSystem : MonoBehaviour
{
    private UIContainer uiStuff;
    // [SerializeField] Color gainMoneyColor;
    // [SerializeField] Color loseMoneyColor;
    [SerializeField] public int score = 0;
    // Update is called once per frame
    void Update()
    {
        if(uiStuff == null){
            uiStuff = FindObjectOfType<UIContainer>();
        }
    }

    public void AddToScore(int addscore){
        //Debug.Log("adding to score");
        score += addscore;
        uiStuff.UpdateScore(score);
        uiStuff.CreateScoreParticle("+", addscore);
    }
    public void SubtractScore(int subscore){
        score -= subscore;
        uiStuff.UpdateScore(score);
        uiStuff.CreateScoreParticle("-", subscore);
    }
    public void OpenDoor(AudioClip sound){
        uiStuff.OpenDoor(sound);
    }
}
