using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace Alteruna{
    public class UpdatePlayerName : MonoBehaviour
    {
        [SerializeField] TextMeshPro nameText;
        private void Awake(){
            var Multiplayer = FindObjectOfType<Multiplayer>();
            nameText.text = Multiplayer.gameObject.GetComponent<NameChanger>().name;
        }
    }
}
