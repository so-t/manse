using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playing : PlayerState
{
    public Playing(GameObject playerObject)
    {
        Player = playerObject.GetComponent<PlayerController>();
    }

    public override void HandlePlayerInput()
    {
        
        if (Input.GetAxis("Horizontal") != 0.0f || Input.GetAxis("Vertical") != 0.0f)
        {  
            // Handle Forward/Backwards Movement
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

            // Play footstep sfx if not already playing
            if(!Player.gameObject.GetComponent<AudioSource>().isPlaying)
            {
                Player.gameObject.GetComponent<AudioSource>().Play();
            }
        }
        else
        {
            // Stop footstep sfx if playing
            if(Player.gameObject.GetComponent<AudioSource>().isPlaying)
            {
                Player.gameObject.GetComponent<AudioSource>().Pause();
            }
        }

        Player.gameObject.BroadcastMessage("HeadBob");
    }
}