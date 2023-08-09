using PlayerControls.Controller;
using UnityEngine;

namespace PlayerControls.PlayerState
{
    public class Playing : PlayerState
    {
        public Playing(PlayerController playerController)
        {
            Player = playerController;
        }

        public override void HandlePlayerInput()
        {
            var velocity = Vector3.zero;
            if (Input.GetAxis("Horizontal") != 0.0f || Input.GetAxis("Vertical") != 0.0f)
            {
                velocity.y = Input.GetAxisRaw("Horizontal");
                velocity.z = Input.GetAxisRaw("Vertical");

                // Play footstep sfx if not already playing
                if(!Player.footstepAudioSource.isPlaying)
                {
                    Player.footstepAudioSource.Play();
                }
            }
            else
            {
                // Stop footstep sfx if playing
                if(Player.footstepAudioSource.isPlaying)
                {
                    Player.footstepAudioSource.Pause();
                }
            }
            Player.Velocity = velocity;
            
            if (Input.GetKeyDown("escape"))
            {
                Player.Pause();
            }
        }
    }
}