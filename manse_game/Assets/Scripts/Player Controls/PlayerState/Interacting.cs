using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interacting : PlayerState
{
    private float creationTime;

    public Interacting(GameObject playerObject)
    {
        Player = playerObject.GetComponent<PlayerController>();
    }
}