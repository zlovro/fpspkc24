using Source.Libraries.KBLib2;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Game
{
    public class CameraController : Kb2Behaviour
    {
        public float fwdSpeed    = 10;
        public float verticalSpeed    = 10;
        public float boostFactor = 2;

        public float scrollFactor = 3;

        public bool  invertUpDown, invertLeftRight;
        public float mouseLookSpeed = 20;

        private float mCurrentSpeed = 0;
        private float mScrollMod    = 0;

        private float mXRot, mYRot;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        private void Update()
        {
            var mouseX = Input.GetAxis("Mouse X") * mouseLookSpeed;
            var mouseY = Input.GetAxis("Mouse Y") * mouseLookSpeed;

            mXRot += invertUpDown ? mouseY : -mouseY;
            mYRot += invertLeftRight ? mouseX : -mouseX;

            mXRot = Mathf.Clamp(mXRot, -90, 90);
            
            tf.rotation = Quaternion.Euler(mXRot, mYRot, 0);

            var vertical   = Input.GetAxisRaw("Vertical");
            var horizontal = Input.GetAxisRaw("Horizontal");

            float up = 0;
            if (Input.GetKey(KeyCode.Space))
            {
                up = 1;
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                up = -1;
            }

            up *= verticalSpeed;

            var hzRot  = Quaternion.Euler(0, tf.localEulerAngles.y, 0);
            var movementDir = (hzRot * Vector3.forward) * vertical + (hzRot * Vector3.right) * horizontal + (hzRot * Vector3.up) * up;

            mCurrentSpeed = fwdSpeed;
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                mCurrentSpeed *= boostFactor;
            }
            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                mCurrentSpeed /= boostFactor;
            }

            mScrollMod    += -Input.mouseScrollDelta.y * scrollFactor;
            mCurrentSpeed -= mScrollMod;

            tf.position += movementDir * (mCurrentSpeed * Time.deltaTime);
        }
    }
}