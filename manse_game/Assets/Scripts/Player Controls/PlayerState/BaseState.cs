using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState : PlayerState
{
    public BaseState(GameObject playerObject)
    {
        Player = playerObject.GetComponent<PlayerController>();
    }

    public override void HandlePlayerInput()
    {  
        // Handle Movement
        if (Input.GetAxis("Vertical") != 0.0f)
        {
            velocity = new Vector3(0.0f, 0.0f, Player.Speed * Input.GetAxisRaw("Vertical"));
            Player.transform.Translate(velocity * Time.deltaTime);
        }

        // Handle Turning
        if (Input.GetAxis("Horizontal") != 0.0f)
        {
            velocity = new Vector3(0.0f, Player.TurnSpeed * Input.GetAxisRaw("Horizontal"), 0.0f);
            Player.transform.Rotate(velocity * Time.deltaTime);
        }

        Player.gameObject.BroadcastMessage("HeadBob");
    }
}