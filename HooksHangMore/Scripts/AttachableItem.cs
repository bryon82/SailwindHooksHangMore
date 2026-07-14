using UnityEngine;
using static HooksHangMore.HHM_Plugin;

namespace HooksHangMore
{
    public class AttachableItem : MonoBehaviour
    {
        private ShipItem _shipItem;
        private PickupableItem _pickupable;
        internal AttachableItemHolder Holder { get; set; }
        private bool _disallowHangingOnTrigger;
        private float _framesAfterAwake;

        public Vector3 PositionOffset { get; set; }
        public Vector3 RotationOffset { get; set; }
        public bool IsAttached => Holder != null;

        private void Awake()
        {
            _pickupable = GetComponent<PickupableItem>();
            _shipItem = GetComponent<ShipItem>();
            Holder = null;
            _disallowHangingOnTrigger = false;
            _framesAfterAwake = 0f;
            PositionOffset = Vector3.zero;
            RotationOffset = Vector3.zero;
        }

        private void LateUpdate()
        {
            if (_pickupable != null && _pickupable is Anchor anchor && IsAttached)
            {
                anchor.transform.position = Holder.transform.TransformPoint(PositionOffset);
                anchor.transform.rotation = Holder.transform.rotation * Quaternion.Euler(RotationOffset);
            }
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
            var holder = other.GetComponent<AttachableItemHolder>();
            bool canAttach = 
                (_framesAfterAwake < 3f ||
                GameState.currentlyLoading ||
                GameState.loadingBoatLocalItems) &&
                !_pickupable.held &&
                !_disallowHangingOnTrigger &&
                other.CompareTag("Hook") &&
                holder != null &&
                !holder.IsOccupied &&
                !IsAttached;

            if (_shipItem != null)
                canAttach = canAttach && _shipItem.sold && _shipItem.GetCurrentInventorySlot() == -1;

            if (canAttach)
            {
                LogDebug($"Attaching {_pickupable.name} during load.");
                holder.AttachItem(_pickupable);
                Holder = holder;
            }
        }

        public void DetachHolder()
        {
            if (Holder != null)
                Holder = null;
        }

        public void OnDestroy()
        {
            Holder?.DetachItem();
            _disallowHangingOnTrigger = false;
        }
    }
}


//public void OnTriggerExit(Collider other)
//{
//    if (other.CompareTag("Hook") && _shipItemHolder != null && other.GetComponent<BF_ShipItemHolder>() == _shipItemHolder)
//        _shipItemHolder.DetachItem();
//}
