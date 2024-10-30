using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemBoss : Dummy
{
    private void LateUpdate(){
        uiStuff.UpdateGolemHealth(currentHealth, maxHealth);
    }
    public void DisableHealthBar(){
        uiStuff.ToggleGolemParent(false);
    }
    public void OnDestroy(){
        uiStuff.ToggleGolemParent(false);
    }
}
