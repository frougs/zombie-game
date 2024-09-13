using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InteractText : MonoBehaviour
{
    [SerializeField] TextMeshPro pickupText;
    [SerializeField] Text getText;
    [SerializeField] string prompt;
    void Update()
    {
        if(prompt == null || prompt == ""){
            pickupText.text = "[ " + getText.text +" ]";
        }
        else{
            pickupText.text = prompt +": [ " + getText.text +" ]";
        }
    }
}
