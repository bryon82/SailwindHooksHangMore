using HarmonyLib;
using UnityEngine;
using static HooksHangMore.HHM_Plugin;
using static HooksHangMore.Configs;

namespace HooksHangMore
{
    internal class ItemHolderPatches
    {
        private static readonly Vector3 ROD_POSITION_OFFSET = new Vector3(0.309f, 1.1f, -0.38f);
        private static readonly Vector3 ROD_ROTATION_OFFSET = new Vector3(-40f, 180f, 0f);

        private static readonly Vector3 BROOM_POSITION_OFFSET = new Vector3(0f, -0.25f, -0.11f);
        private static readonly Vector3 CHIP_LOG_POSITION_OFFSET = new Vector3(0.002f, 0.25f, -0.12f);

        private static readonly Vector3 QUADRANT_POSITION_OFFSET = new Vector3(-0.0155f, 0.164f, -0.115f);
        private static readonly Vector3 QUADRANT_ROTATION_OFFSET = new Vector3(90f, -90f, 0f);

        private static readonly Vector3 KNIFE_POSITION_OFFSET = new Vector3(0.05f, -0.115f, -0.182f);
        private static readonly Vector3 KNIFE_ROTATION_OFFSET = new Vector3(270f, 270f, 0f);

        [HarmonyPatch(typeof(ShipItem))]
        private class ShipItemPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("OnLoad")]
            public static void OnLoadAddSavedFishingRodHolder(ShipItem __instance)
            {
                if (__instance is ShipItemLampHook)
                    __instance.gameObject.AddComponent<ShipItemHolder>();

                if (__instance is ShipItemBroom broom)
                {
                    var attachable = broom.gameObject.AddComponent<HolderAttachable>();
                    attachable.PositionOffset = BROOM_POSITION_OFFSET;
                }
            }

            [HarmonyPrefix]
            [HarmonyPatch("AllowOnItemClick")]
            public static bool AllowOnItemClick(GoPointerButton lookedAtButton, ShipItem __instance, ref bool __result)
            {
                if (__instance.GetComponent<HolderAttachable>() != null &&
                    lookedAtButton.GetComponent<ShipItemHolder>() != null &&
                    !lookedAtButton.GetComponent<ShipItemHolder>().IsOccupied)
                {
                    __result = true;
                    return false;
                }

                return true;
            }

            [HarmonyPrefix]
            [HarmonyPatch("OnPickup")]
            public static void OnShipItemPickup(ShipItem __instance)
            {
                if (__instance.GetComponent<HolderAttachable>() != null && AttachedItems.ContainsKey(__instance))
                {
                    var holder = AttachedItems[__instance];
                    if (holder != null && holder.AttachedItem == __instance)
                        holder.DetachItem();
                }
            }
        }

        [HarmonyPatch(typeof(ShipItemFishingRod), "OnLoad")]
        private class ShipItemFishingRodPatches
        {
            public static void Postfix(ShipItemFishingRod __instance)
            {
                var attachable = __instance.gameObject.AddComponent<HolderAttachable>();
                attachable.PositionOffset = ROD_POSITION_OFFSET;
                attachable.RotationOffset = ROD_ROTATION_OFFSET;
            }
        }

        [HarmonyPatch(typeof(ShipItemChipLog), "OnLoad")]
        private class ShipItemChipLogPatches
        {
            public static void Postfix(ShipItemChipLog __instance)
            {
                var attachable = __instance.gameObject.AddComponent<HolderAttachable>();
                attachable.PositionOffset = CHIP_LOG_POSITION_OFFSET;
            }
        }

        [HarmonyPatch(typeof(ShipItemQuadrant), "OnLoad")]
        private class ShipItemQuadrantPatches
        {
            public static void Postfix(ShipItemQuadrant __instance)
            {
                var attachable = __instance.gameObject.AddComponent<HolderAttachable>();
                attachable.PositionOffset = QUADRANT_POSITION_OFFSET;
                attachable.RotationOffset = QUADRANT_ROTATION_OFFSET;
            }
        }

        [HarmonyPatch(typeof(ShipItemKnife))]
        private class ShipItemKnifePatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("OnLoad")]
            public static void AddComponent(ShipItemKnife __instance)
            {
                var attachable = __instance.gameObject.AddComponent<HolderAttachable>();
                attachable.PositionOffset = KNIFE_POSITION_OFFSET;
                attachable.RotationOffset = KNIFE_ROTATION_OFFSET;
                if (flipKnifeRotation.Value)
                {
                    attachable.PositionOffset = new Vector3(-attachable.PositionOffset.x, -attachable.PositionOffset.y, attachable.PositionOffset.z);
                    attachable.RotationOffset = new Vector3(attachable.RotationOffset.x - 180, attachable.RotationOffset.y, attachable.RotationOffset.z);
                }
            }

            [HarmonyPrefix]
            [HarmonyPatch("AllowOnItemClick")]
            public static bool AllowOnItemClick(GoPointerButton lookedAtButton, ref bool __result)
            {
                if ((bool)lookedAtButton.GetComponent<ShipItemHolder>() && !lookedAtButton.GetComponent<ShipItemHolder>().IsOccupied)
                {
                    __result = true;
                    return false;
                }

                return true;
            }
        }

        [HarmonyPatch(typeof(ShipItemLampHook))]
        private class ShipItemLampHookPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("OnPickup")]
            public static void OnPickup(ShipItemLampHook __instance)
            {
                var holder = __instance.GetComponent<ShipItemHolder>();
                if (holder.IsOccupied)
                {
                    holder.DetachItem();
                }
            }

            [HarmonyPostfix]
            [HarmonyPatch("OnEnterInventory")]
            public static void OnEnterInventory(ShipItemLampHook __instance)
            {
                var holder = __instance.GetComponent<ShipItemHolder>();
                if (holder.IsOccupied)
                {
                    holder.DetachItem();
                }
            }

            [HarmonyPrefix]
            [HarmonyPatch("OnItemClick")]
            public static bool OnItemClick(PickupableItem heldItem, ShipItemLampHook __instance, bool ___occupied, ref bool __result)
            {
                var holder = __instance.GetComponent<ShipItemHolder>();
                if (holder.IsOccupied || ___occupied)
                {
                    __result = false;
                    return false;
                }

                var item = heldItem.GetComponent<ShipItem>();
                if (item.GetComponent<HolderAttachable>() != null && item.sold)
                {
                    holder.AttachItem(item);
                    item.GetComponent<HolderAttachable>().ShipItemHolder = holder;
                    __result = true;
                    return false;
                }

                return true;
            }
        }

        [HarmonyPatch(typeof(LookUI))]
        private class LookUIPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("ShowLookText")]
            public static void ShowLookText(
                GoPointerButton button,
                TextMesh ___controlsText,
                GoPointer ___pointer,
                TextMesh ___textLicon,
                ref bool ___showingIcon)
            {
                var lampHook = button.GetComponent<ShipItemLampHook>();
                if (lampHook != null && (bool)___pointer.GetHeldItem() && lampHook.GetComponent<ShipItemHolder>().IsOccupied)
                {
                    ___textLicon.gameObject.SetActive(false);
                    ___showingIcon = false;
                    ___controlsText.text = "";
                }
                else if (lampHook != null && (bool)___pointer.GetHeldItem() &&
                    ___pointer.GetHeldItem().GetComponent<HolderAttachable>() != null &&
                    !lampHook.GetComponent<ShipItemHolder>().IsOccupied)
                {
                    ___textLicon.gameObject.SetActive(true);
                    ___showingIcon = true;
                    ___controlsText.text = $"attach {___pointer.GetHeldItem()?.GetComponent<ShipItem>()?.name}\n";
                }
            }
        }
    }
}
