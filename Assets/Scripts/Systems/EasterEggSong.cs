using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasterEggSong : MonoBehaviour
{
    [SerializeField] AudioClip easterEggSong;
    [SerializeField] GameObject[] easterEggTargets;
    [SerializeField] int easterEggTotal;
    public void EasterEggProgress(){
        easterEggTotal += 1;
    }
    private void Update(){
        if(easterEggTotal >= easterEggTargets.Length){
            FindObjectOfType<BackgroundMusicPlayerScript>().PlayEasterEggSong(easterEggSong);
            easterEggTotal = 0;
        }
    }
}
