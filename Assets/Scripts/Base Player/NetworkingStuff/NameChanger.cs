using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Eventually transition to player prefs so players can update/set their name in the main menu before loading into the lobby
namespace Alteruna{
    public class NameChanger : MonoBehaviour
    {
        public string name;
        private Multiplayer Multiplayer;
        public void UpdateName(){
            Multiplayer.SetUsername(name);
        }
        public void Awake(){
            Multiplayer = FindObjectOfType<Multiplayer>();
            UpdateName();
        }
    }
}
