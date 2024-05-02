using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinedServer : MonoBehaviour
{
    [SerializeField] GameObject chat;
    [SerializeField] public GameObject roomMenu;
    void Start()
    {
    }
    public void Joined(){
        chat.SetActive(true);
        //roomMenu.SetActive(false);
        roomMenu.GetComponent<CanvasGroup>().alpha = 0;
        roomMenu.GetComponent<CanvasGroup>().interactable = false;
        roomMenu.GetComponent<CanvasGroup>().blocksRaycasts = false;
        //Destroy(roomMenu);
    }
}
