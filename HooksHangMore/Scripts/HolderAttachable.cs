using UnityEngine;
using static HooksHangMore.HHM_Plugin;

namespace HooksHangMore
{
    public class HolderAttachable : MonoBehaviour
    {
        private ShipItem _shipItem;
        private ShipItemHolder _shipItemHolder;
        private bool _disallowHangingOnTrigger;
        private float _framesAfterAwake;

        public Vector3 PositionOffset { get; set; }
        public Vector3 RotationOffset { get; set; }

        private void Awake()
        {
            _shipItem = GetComponent<ShipItem>();
            _shipItemHolder = null;
            _disallowHangingOnTrigger = false;
            _framesAfterAwake = 0f;
        }

        private void FixedUpdate()
        {
            if (_framesAfterAwake < 3f)
                _framesAfterAwake += 1f;
        }

        public void LoadInInventory()
        {
            _disallowHangingOnTrigger = true;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (!(_framesAfterAwake >= 3f) && !_shipItem.held && _shipItem.GetCurrentInventorySlot() == -1 && !_disallowHangingOnTrigger && other.CompareTag("Hook") && !other.GetComponent<ShipItemHolder>().IsOccupied)
            {
                var holder = other.GetComponent<ShipItemHolder>();
                holder.AttachItem(_shipItem);
                _shipItemHolder = holder;
            }
        }

        //public void OnTriggerExit(Collider other)
        //{
        //    if (other.CompareTag("Hook") && _shipItemHolder != null && other.GetComponent<BF_ShipItemHolder>() == _shipItemHolder)
        //        _shipItemHolder.DetachItem();
        //}

        public void DetachHolder()
        {
            if (_shipItemHolder != null)
                _shipItemHolder = null;
        }

        public void OnDestroy()
        {
            _shipItemHolder?.DetachItem();
            _disallowHangingOnTrigger = false;
        }
    }
}
