using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocialMediaLinks : MonoBehaviour
{
    [SerializeField] string socialLink;
    public void OpenLink(){
        Application.OpenURL(socialLink);
    }
}
