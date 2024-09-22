using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;

public class DamageableIDGenerator : MonoBehaviour
{
    public string ID;

    void Start()
    {
        Guid thisID = Guid.NewGuid();
        ID = thisID.ToString();
    }

}
