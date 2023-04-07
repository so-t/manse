using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    public Vector3 velocity;
    public PlayerController Player;

    public PlayerState(){}

    public virtual void HandlePlayerInput(){}
}