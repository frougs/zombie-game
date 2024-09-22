using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteAmmoPowerup : MonoBehaviour
{
    private void Update(){
        this.GetComponent<InteractionController>().gunRoot.transform.GetChild(0).gameObject.GetComponent<BaseGun>().infiniteAmmo = true;
        this.GetComponent<InteractionController>().gunRoot.transform.GetChild(0).gameObject.GetComponent<BaseGun>().RefillAmmo();
    }
}
