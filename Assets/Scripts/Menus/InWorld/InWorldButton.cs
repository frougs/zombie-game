using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InWorldButton : MonoBehaviour, IInteractable
{
    public UnityEvent triggered;
    public void Interacted(GameObject gunRoot, InteractionController interactionCon){
        Debug.Log("Interacted with");
        triggered?.Invoke();
    }
}
