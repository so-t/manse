using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paused : PlayerState
{
    public Paused(GameObject playerObject)
    {
        Player = playerObject.GetComponent<PlayerController>();
    }
}