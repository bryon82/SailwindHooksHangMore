using HarmonyLib;
using UnityEngine;
using static HooksHangMore.HHM_Plugin;

namespace HooksHangMore
{
    internal class AttachablePatches
    {
        [HarmonyPatch(typeof(ShipItem))]
        private class ShipItemPatches
        {
            [HarmonyBefore(NAND_FIXES_GUID)]
            [HarmonyPostfix]
            [HarmonyPatch("OnLoad")]
            public static void AddItemHolderAndAttachable(ShipItem __instance)
            {
                if (__instance is ShipItemLampHook)
                    __instance.gameObject.AddComponent<AttachableItemHolder>();

                if (Offsets.AttachedItems.TryGetOffset(__instance.name, out var offset))
                {
                    var attachable = __instance.gameObject.AddComponent<AttachableItem>();
                    attachable.PositionOffset = offset.Position;
                    attachable.RotationOffset = offset.Rotation;
                }
            }

            [HarmonyPrefix]
            [HarmonyPatch("AllowOnItemClick")]
            public static bool AllowOnItemClick(GoPointerButton lookedAtButton, ShipItem __instance, ref bool __result)
            {
                if (!__instance.sold)
                    return true;

                if (__instance.GetComponent<AttachableItem>() != null &&
                    lookedAtButton.GetComponent<AttachableItemHolder>() != null &&
                    !lookedAtButton.GetComponent<AttachableItemHolder>().IsOccupied)
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
                if (__instance.GetComponent<AttachableItem>() != null && AttachedItems.ContainsKey(__instance))
                {
                    var holder = AttachedItems[__instance];
                    if (holder != null && holder.AttachedItem == __instance)
                        holder.DetachItem();
                }
            }
        }

        [HarmonyPatch(typeof(PickupableItem), "OnPickup")]
        private class PickupableItemOnPickupPatch
        {
            [HarmonyPostfix]
            public static void OnPickup(PickupableItem __instance)
            {
                if (__instance.GetComponent<AttachableItem>() != null && AttachedItems.ContainsKey(__instance))
                {
                    var holder = AttachedItems[__instance];
                    if (holder != null && holder.AttachedItem == __instance)
                        holder.DetachItem();
                }
            }
        }

        [HarmonyPatch(typeof(ShipItemLampHook))]
        private class ShipItemLampHookPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("OnPickup")]
            public static void OnPickup(ShipItemLampHook __instance)
            {
                var holder = __instance.GetComponent<AttachableItemHolder>();
                if (holder.IsOccupied)
                {
                    holder.DetachItem();
                }
            }

            [HarmonyPostfix]
            [HarmonyPatch("OnEnterInventory")]
            public static void OnEnterInventory(ShipItemLampHook __instance)
            {
                var holder = __instance.GetComponent<AttachableItemHolder>();
                if (holder.IsOccupied)
                {
                    holder.DetachItem();
                }
            }

            [HarmonyPrefix]
            [HarmonyPatch("OnItemClick")]
            public static bool OnItemClick(PickupableItem heldItem, ShipItemLampHook __instance, bool ___occupied, ref bool __result)
            {
                var holder = __instance.GetComponent<AttachableItemHolder>();
                if (holder.IsOccupied || ___occupied)
                {
                    __result = false;
                    return false;
                }

                if (heldItem is ShipItem shipItem)
                {
                    if (!shipItem.sold)
                        return true;

                    if ((shipItem is ShipItemSoup soup && soup.currentWater > 0) || (shipItem is ShipItemBottle bottle && bottle.health > 0))
                    {
                        NotificationUi.instance.ShowNotification("Cannot hang\nit is not empty");
                        __result = false;
                        return false;
                    }
                }

                var attachable = heldItem.GetComponent<AttachableItem>();
                if (attachable != null)
                {
                    holder.AttachItem(heldItem);
                    attachable.Holder = holder;
                    __result = true;
                    return false;
                }

                return true;
            }
        }

        [HarmonyPatch(typeof(LookUI), "ShowLookText")]
        private class LookUIPatches
        {
            public static void Postfix(
                GoPointerButton button,
                TextMesh ___controlsText,
                GoPointer ___pointer,
                TextMesh ___textLicon,
                TextMesh ___textRIcon,
                ref bool ___showingIcon)
            {
                var lampHook = button.GetComponent<ShipItemLampHook>();
                var holder = lampHook?.GetComponent<AttachableItemHolder>();
                var heldItem = ___pointer.GetHeldItem();
                var attachableItem = heldItem?.GetComponent<AttachableItem>();
                var hangableItem = heldItem?.GetComponent<HangableItem>();
                if (lampHook != null && heldItem != null && holder != null && holder.IsOccupied)
                {
                    ___textLicon.gameObject.SetActive(false);
                    ___showingIcon = false;
                    ___controlsText.text = "";

                    if ((bool)heldItem.GetComponent<ShipItemHammer>() && ShipItemHammer.CanNail(button.GetComponent<ShipItem>()))
                    {
                        ___showingIcon = true;
                        if (button.GetComponent<ShipItem>().nailed)
                        {
                            ___textRIcon.gameObject.SetActive(true);
                            ___controlsText.text = "\nunlock";
                        }
                        else
                        {
                            ___textRIcon.gameObject.SetActive(true);
                            ___controlsText.text = "\nlock";
                        }
                    }
                }
                else if (lampHook != null && heldItem != null && (attachableItem != null || hangableItem != null) && !holder.IsOccupied)
                {
                    var shipItem = heldItem.GetComponent<ShipItem>();
                    var itemName = shipItem?.name ?? "anchor";
                    ___textLicon.gameObject.SetActive(true);
                    ___showingIcon = true;
                    ___controlsText.text = $"attach {itemName}\n";

                    if ((bool)heldItem.GetComponent<ShipItemHammer>() && ShipItemHammer.CanNail(button.GetComponent<ShipItem>()))
                    {
                        if (button.GetComponent<ShipItem>().nailed)
                        {
                            ___textRIcon.gameObject.SetActive(true);
                            ___controlsText.text = $"attach {itemName}\nunlock";
                        }
                        else
                        {
                            ___textRIcon.gameObject.SetActive(true);
                            ___controlsText.text = $"attach {itemName}\nlock";
                        }
                    }
                }
            }
        }
    }
}
