using PlayerControls.Controller;

namespace PlayerControls.PlayerState
{
    public class Interacting : PlayerState
    {
        public Interacting(PlayerController playerController)
        {
            Player = playerController;
        }
    }
}