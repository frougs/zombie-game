using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingForm : MonoBehaviour
{
    [SerializeField] string formLink;
    public void Pressed(){
        Application.OpenURL(formLink);
    }
}
