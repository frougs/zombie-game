using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    private UIContainer uiStuff;
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
    }
    public void SubtractScore(int subscore){
        score -= subscore;
        uiStuff.UpdateScore(score);
    }
}
