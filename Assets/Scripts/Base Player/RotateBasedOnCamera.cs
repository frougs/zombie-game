using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBasedOnCamera : MonoBehaviour
{
    private void FixedUpdate(){
        this.gameObject.transform.rotation = Quaternion.Euler(0, CameraSingleton.instance.transform.rotation.x, 0);
    }
}
