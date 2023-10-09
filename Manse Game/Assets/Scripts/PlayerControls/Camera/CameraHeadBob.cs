using PlayerControls.Controller;
using PlayerControls.PlayerState;
using UnityEngine;

namespace PlayerControls.Camera
{
    public class CameraHeadBob : MonoBehaviour
    {
        public float walkingBobbingSpeed = 14f;
        public float bobbingAmount = 0.05f;

        private float _defaultPosY;
        private float _timer;
        public PlayerController playerController;

        // Start is called before the first frame update
        private void Awake()
        {
            _defaultPosY = transform.localPosition.y;
            playerController = gameObject.GetComponentInParent<PlayerController>();
        }

        private void HeadBob()
        {
            if(Input.GetAxis("Vertical") != 0.0f)
            {
                //Player is moving
                _timer += Time.deltaTime * walkingBobbingSpeed;
                var localPosition = transform.localPosition;
                localPosition = new Vector3(localPosition.x, _defaultPosY + Mathf.Sin(_timer) * 
                    bobbingAmount, localPosition.z);
                transform.localPosition = localPosition;
            }
            else
            {
                //Idle
                _timer = 0;
                var localPosition = transform.localPosition;
                localPosition = new Vector3(localPosition.x, 
                                                        Mathf.Lerp(localPosition.y, _defaultPosY, 
                                                                    Time.deltaTime * walkingBobbingSpeed), 
                                                        localPosition.z);
                transform.localPosition = localPosition;
            }
        }

        private void Update()
        {
            if (playerController.state.GetType() == typeof(Playing)) HeadBob();
        }
    }
}
