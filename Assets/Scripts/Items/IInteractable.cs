using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable{
    void Interacted(GameObject gunRoot, InteractionController interactionCon);
}
