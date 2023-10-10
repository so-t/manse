using System.Collections.Generic;
using System.Linq;
using PlayerControls.Controller;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerControls.PlayerState
{
    public class Paused : PlayerState
    {
        //private readonly MeshRenderer _background;
        
        private UIUtilities _uiUtils;
        private Image _background;
        
        public Paused(PlayerController playerController)
        {
            Player = playerController;

            _uiUtils = playerController.UIUtils;
            _uiUtils.DimBackground();
        }

        public override void HandlePlayerInput()
        {
            if (Input.GetKeyDown("escape"))
            {
                //_background.enabled = false;
                _uiUtils.ResetBackgroundBrightness();
                Player.Resume();
            }
        }
    }
}