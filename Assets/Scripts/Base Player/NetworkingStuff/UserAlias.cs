using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Alteruna;
public class UserAlias : AttributesSync
{
    private Alteruna.Avatar _avatar;
    //private Multiplayer Multiplayer;
    public TextMeshPro nameText;
    [SynchronizableField] private string name;

    private void Start(){
        _avatar = GetComponentInParent<Alteruna.Avatar>();
        Multiplayer = FindObjectOfType<Multiplayer>();
        if (!_avatar.IsMe)
            return;
        if(PlayerPrefs.GetInt("HasUsername") == 0){
            string objectName = _avatar.gameObject.name;
            int openParenIndex = objectName.IndexOf("(");
            int closeParenIndex = objectName.IndexOf(")");
            if(openParenIndex != -1 && closeParenIndex != -1){
                string substring = objectName.Substring(openParenIndex + 1, closeParenIndex - openParenIndex - 1);
                name=substring;
            } 
        }
        else{
            name=PlayerPrefs.GetString("Username");
        }
    }
    private void FixedUpdate(){
        if(name!= null){
            nameText.text = name;
        }
        
    }
}

