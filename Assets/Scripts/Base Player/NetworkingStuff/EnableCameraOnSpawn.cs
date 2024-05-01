/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableCameraOnSpawn : MonoBehaviour
{
public void EnableCamera(){
    CameraSingleton.instance.GetComponentInChildren<FirstPersonLook>().enabled = true;
    CameraSingleton.instance.GetComponentInChildren<FirstPersonLook>().ToggleLook(true);
}
public void DisableCamera(){
    CameraSingleton.instance.gameObject.transform.parent = null;
    CameraSingleton.instance.GetComponentInChildren<FirstPersonLook>().ToggleLook(false);
    CameraSingleton.instance.GetComponentInChildren<FirstPersonLook>().enabled = false;
}

}*/
