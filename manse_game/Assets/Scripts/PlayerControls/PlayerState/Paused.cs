using PlayerControls.Controller;
using UnityEngine;

namespace PlayerControls.PlayerState
{
    public class Paused : PlayerState
    {
        public Paused(GameObject playerObject)
        {
            Player = playerObject.GetComponent<PlayerController>();
        }
    }
}