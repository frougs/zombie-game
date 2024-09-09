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
        pickupText.text = prompt +": [ " + getText.text +" ]";
    }
}
