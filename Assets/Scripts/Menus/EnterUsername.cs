using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EnterUsername : MonoBehaviour
{
    private TMP_InputField textBox;
    [SerializeField] TextMeshProUGUI exampleName;
   void Start ()
    {
        textBox = this.GetComponent<TMP_InputField>();
        var name = PlayerPrefs.GetString("Username");
        Debug.Log(name);
            if(PlayerPrefs.GetInt("HasUsername") == 1){
                Debug.Log("Username Found, Loading...");
                exampleName.text = "USERNAME: " + name;
            }
            else{
                Debug.Log("No username found");
            }
    }

    public void SubmitName()
    {
        //Debug.Log(textBox.text);
        
        exampleName.text = "USERNAME: " +textBox.text;
        PlayerPrefs.SetString("Username", textBox.text);
        PlayerPrefs.SetInt("HasUsername", 1);
        textBox.text = null;
    }
    public void ResetName(){
        PlayerPrefs.SetInt("HasUsername", 0);
        PlayerPrefs.DeleteKey("Username");
        exampleName.text = "USERNAME: RANDOM";
    }
    
}
