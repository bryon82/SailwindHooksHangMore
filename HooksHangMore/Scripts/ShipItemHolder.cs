using UnityEngine;
using static HooksHangMore.HHM_Plugin;

namespace HooksHangMore
{
    public class ShipItemHolder : MonoBehaviour
    {
        private readonly Vector3 ROD_POSITION_OFFSET = new Vector3(0.309f, 1.1f, -0.38f);
        private readonly Vector3 ROD_ROTATION_OFFSET = new Vector3(-40f, 180f, 0f);

        private readonly Vector3 BROOM_POSITION_OFFSET = new Vector3(0f, -0.25f, -0.11f);
        private readonly Vector3 CHIP_LOG_POSITION_OFFSET = new Vector3(0.002f, 0.25f, -0.12f);

        private readonly Vector3 QUADRANT_POSITION_OFFSET = new Vector3(-0.0155f, 0.164f, -0.115f);
        private readonly Vector3 QUADRANT_ROTATION_OFFSET = new Vector3(90f, -90f, 0f);

        private readonly Vector3 KNIFE_POSITION_OFFSET = new Vector3(0.05f, -0.115f, -0.182f);
        private readonly Vector3 KNIFE_ROTATION_OFFSET = new Vector3(270f, 270f, 0f);

        private readonly Vector3 HAMMER_POSITION_OFFSET = new Vector3(0.02f, -0.15f, -0.12f);
        private readonly Vector3 HAMMER_ROTATION_OFFSET = new Vector3(270f, 270f, 0f);

        //private Type shipItemHammer = AccessTools.TypeByName("ShipItemHammer");
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
            if (item is ShipItemFishingRod)
            {
                localAttachOffset = ROD_POSITION_OFFSET;
                rotationOffset = Quaternion.Euler(ROD_ROTATION_OFFSET);
            }
            else if (item is ShipItemBroom)
            {
                localAttachOffset = BROOM_POSITION_OFFSET;
            }
            else if (item is ShipItemChipLog)
            {
                localAttachOffset = CHIP_LOG_POSITION_OFFSET;
            }
            else if (item is ShipItemQuadrant)
            {
                localAttachOffset = QUADRANT_POSITION_OFFSET;
                rotationOffset = Quaternion.Euler(QUADRANT_ROTATION_OFFSET);
            }
            else if (item is ShipItemKnife)
            {
                localAttachOffset = KNIFE_POSITION_OFFSET;
                rotationOffset = Quaternion.Euler(KNIFE_ROTATION_OFFSET);
            }
            else if (item.name is "hammer")
            {
                localAttachOffset = HAMMER_POSITION_OFFSET;
                rotationOffset = Quaternion.Euler(HAMMER_ROTATION_OFFSET);
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
