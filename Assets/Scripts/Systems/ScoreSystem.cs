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
    public bool doublePoints = false;
    // Update is called once per frame
    void Update()
    {
        if(uiStuff == null){
            uiStuff = FindObjectOfType<UIContainer>();
        }
    }

    public void AddToScore(int addscore){
        //Debug.Log("adding to score");
        if(!doublePoints){
            score += addscore;
            uiStuff.CreateScoreParticle("+", addscore);
        }
        else{
            score += (int)(addscore * 2);
            var doubledScore = (int)(addscore * 2);
            uiStuff.CreateScoreParticle("+", doubledScore);
        }
        
        uiStuff.UpdateScore(score);
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
