using PlayerControls.Controller;
using UnityEngine;

namespace PlayerControls.PlayerState
{
    public class Interacting : PlayerState
    {
        public Interacting(GameObject playerObject)
        {
            Player = playerObject.GetComponent<PlayerController>();
        }
    }
}