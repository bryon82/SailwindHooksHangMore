using UnityEngine;
using static HooksHangMore.HHM_Plugin;

namespace HooksHangMore
{
    public class ShipItemHolder : MonoBehaviour
    {
        private Transform _itemRigidBody;
        private ShipItem _attachedItem;

        private void Awake()
        {
            _itemRigidBody = transform.GetComponent<ShipItemLampHook>().itemRigidbodyC.transform;
        }

        internal void AttachItem(ShipItem item)
        {
            var localAttachOffset = Vector3.zero;
            var rotationOffset = Quaternion.identity;

            var holderAttachable = item.GetComponent<HolderAttachable>();

            if (holderAttachable != null) 
            {
                if (holderAttachable.PositionOffset != null)
                    localAttachOffset = holderAttachable.PositionOffset;
                if (holderAttachable.RotationOffset != null)
                    rotationOffset = Quaternion.Euler(holderAttachable.RotationOffset);
            }

            AttachedItems.Add(item, this);
            _attachedItem = item;

            Vector3 worldAttachPos = _itemRigidBody.TransformPoint(localAttachOffset);
            item.itemRigidbodyC.transform.position = worldAttachPos;

            Quaternion worldAttachRotation = _itemRigidBody.rotation * rotationOffset;
            item.itemRigidbodyC.transform.rotation = worldAttachRotation;

            item.itemRigidbodyC.attached = true;
            LogDebug($"Item localpos: {item.itemRigidbodyC.transform.localPosition}, Item parent: {item.itemRigidbodyC.transform.parent}");
        }

        internal void DetachItem()
        {
            if (_attachedItem == null)
                return;

            _attachedItem.itemRigidbodyC.attached = false;
            AttachedItems.Remove(_attachedItem);
            _attachedItem.GetComponent<HolderAttachable>().DetachHolder();
            _attachedItem = null;
        }

        public bool IsOccupied => _attachedItem != null;
        public ShipItem AttachedItem => _attachedItem;

        private void OnDestroy()
        {
            if (IsOccupied)
            {
                DetachItem();
            }
        }
    }
}
