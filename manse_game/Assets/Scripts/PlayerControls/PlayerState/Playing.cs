using PlayerControls.Camera;
using PlayerControls.Controller;
using UnityEngine;

namespace PlayerControls.PlayerState
{
    public class Playing : PlayerState
    {

        private AudioSource _audioSource;
        
        public Playing(PlayerController playerController)
        {
            Player = playerController;
            _audioSource = Player.footstepAudioSource;
        }

        public override void HandlePlayerInput()
        {
        
            if (Input.GetAxis("Horizontal") != 0.0f || Input.GetAxis("Vertical") != 0.0f)
            {  
                // Handle Forward/Backwards Movement
                if (Input.GetAxis("Vertical") != 0.0f)
                {
                    Velocity = new Vector3(0.0f, 0.0f, Player.speed * Input.GetAxisRaw("Vertical"));
                    Player.transform.Translate(Velocity * Time.deltaTime);
                }

                // Handle Turning
                if (Input.GetAxis("Horizontal") != 0.0f)
                {
                    Velocity = new Vector3(0.0f, Player.turnSpeed * Input.GetAxisRaw("Horizontal"), 0.0f);
                    Player.transform.Rotate(Velocity * Time.deltaTime);
                }

                // Play footstep sfx if not already playing
                if(!_audioSource.isPlaying)
                {
                    _audioSource.Play();
                }
            }
            else
            {
                // Stop footstep sfx if playing
                if(_audioSource.isPlaying)
                {
                    _audioSource.Pause();
                }
            }
        }
    }
}