using UnityEngine;
using static HooksHangMore.HHM_Plugin;

namespace HooksHangMore
{
    public class HolderAttachable : MonoBehaviour
    {
        private ShipItem _shipItem;
        internal ShipItemHolder ShipItemHolder { private get; set; }
        private bool _disallowHangingOnTrigger;
        private float _framesAfterAwake;

        public Vector3 PositionOffset { get; set; }
        public Vector3 RotationOffset { get; set; }
        public bool IsAttached => ShipItemHolder != null;

        private void Awake()
        {
            _shipItem = GetComponent<ShipItem>();
            ShipItemHolder = null;
            _disallowHangingOnTrigger = false;
            _framesAfterAwake = 0f;
            PositionOffset = Vector3.zero;
            RotationOffset = Vector3.zero;
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
            var holder = other.GetComponent<ShipItemHolder>();
            var canAttach = _framesAfterAwake < 3f &&
                _shipItem.sold &&
                !_shipItem.held &&
                _shipItem.GetCurrentInventorySlot() == -1
                && !_disallowHangingOnTrigger &&
                other.CompareTag("Hook") &&
                holder != null &&
                !holder.IsOccupied;

            if (canAttach)
            {
                holder.AttachItem(_shipItem);
                ShipItemHolder = holder;
            }
        }

        //public void OnTriggerExit(Collider other)
        //{
        //    if (other.CompareTag("Hook") && _shipItemHolder != null && other.GetComponent<BF_ShipItemHolder>() == _shipItemHolder)
        //        _shipItemHolder.DetachItem();
        //}

        public void DetachHolder()
        {
            if (ShipItemHolder != null)
                ShipItemHolder = null;
        }

        public void OnDestroy()
        {
            ShipItemHolder?.DetachItem();
            _disallowHangingOnTrigger = false;
        }
    }
}
