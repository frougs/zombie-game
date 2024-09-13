using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideCurrentlySelectedPage : MonoBehaviour
{
    [SerializeField] GameObject currentPage;
    [SerializeField] GameObject defaultPage;
    public void OnDisable(){
        currentPage.SetActive(false);
        defaultPage.SetActive(true);
    }
}
