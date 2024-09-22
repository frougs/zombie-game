using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParent : MonoBehaviour
{
    private void OnDisable(){
        Destroy(this.transform.parent.gameObject);
    }
}
