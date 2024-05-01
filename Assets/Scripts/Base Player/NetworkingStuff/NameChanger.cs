using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Eventually transition to player prefs so players can update/set their name in the main menu before loading into the lobby
namespace Alteruna{
    public class NameChanger : MonoBehaviour
    {
        //public string name;
        private Multiplayer Multiplayer;
        public void UpdateName(){
            var name = PlayerPrefs.GetString("Username");
            if(PlayerPrefs.GetInt("HasUsername") == 1){
                Debug.Log("Changing name");
                Multiplayer.SetUsername(name);
            }
        }
        public void Awake(){
            Multiplayer = FindObjectOfType<Multiplayer>();
            if(Multiplayer == null){
                Debug.Log("Multiplayer found");
                UpdateName();
            }
        }
    }
}
    


