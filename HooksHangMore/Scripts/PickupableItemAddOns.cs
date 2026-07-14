using UnityEngine;

namespace HooksHangMore
{
    internal class PickupableItemAddOns : MonoBehaviour
    {
        public float heldRotationYOffset;

        private void Start()
        {
            heldRotationYOffset = 0f;
        }

        private void Update()
        {
            float input = GameInput.GetScrollAxis() * 25f + OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).y;
            if ((double)input != 0.0)
            {
                if (GameInput.GetKey(InputName.RotateH))
                {
                    heldRotationYOffset += input * 4f;
                    heldRotationYOffset = Mathf.Clamp(heldRotationYOffset, -90, 90);
                }
            }
        }
    }
}
