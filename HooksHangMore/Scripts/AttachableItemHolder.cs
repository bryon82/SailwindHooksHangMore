using UnityEngine;
using static HooksHangMore.HHM_Plugin;

namespace HooksHangMore
{
    public class AttachableItemHolder : MonoBehaviour
    {
        private Transform _itemRigidBody;
        public PickupableItem AttachedItem { get; private set; }
        public bool IsOccupied => AttachedItem != null;

        private void Awake()
        {
            _itemRigidBody = transform.GetComponent<ShipItemLampHook>().itemRigidbodyC.transform;
        }

        internal void AttachItem(PickupableItem item)
        {
            var holderAttachable = item.GetComponent<AttachableItem>();
            if (holderAttachable == null)
            { 
                LogError("HolderAttachable is null, cannot hang item");
                return; 
            }

            AttachedItem = item;

            if (item is ShipItem shipItem)
            {
                Vector3 worldAttachPos = _itemRigidBody.TransformPoint(holderAttachable.PositionOffset);
                shipItem.itemRigidbodyC.transform.position = worldAttachPos;

                Quaternion worldAttachRotation = _itemRigidBody.rotation * Quaternion.Euler(holderAttachable.RotationOffset);
                shipItem.itemRigidbodyC.transform.rotation = worldAttachRotation;

                shipItem.itemRigidbodyC.attached = true;
                LogDebug($"{shipItem.name} Item localpos: {shipItem.itemRigidbodyC.transform.localPosition}, Item parent: {shipItem.itemRigidbodyC.transform.parent}");
            }
            else
            {             
                LogDebug($"{item.name} Item localpos: {item.transform.localPosition}, Item parent: {item.transform.parent}");
            }

            AttachedItems.Add(item, this);
        }

        internal void DetachItem()
        {
            if (AttachedItem == null)
                return;

            if (AttachedItem is ShipItem shipItem)
            {
                shipItem.itemRigidbodyC.attached = false;
            }
            else
            {
                var col = AttachedItem.GetComponent<CapsuleCollider>();
                if (col != null) 
                    col.isTrigger = false;
            }

            AttachedItems.Remove(AttachedItem);
            AttachedItem.GetComponent<AttachableItem>().DetachHolder();
            AttachedItem = null;
        }
                
        private void OnDestroy()
        {
            if (IsOccupied)
            {
                DetachItem();
            }
        }
    }
}
