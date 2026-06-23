using HarmonyLib;
using UnityEngine;
using static HooksHangMore.Configs;
using static HooksHangMore.HHM_Plugin;

namespace HooksHangMore
{
    internal class ItemHolderPatches
    {
        [HarmonyPatch(typeof(ShipItem))]
        private class ShipItemPatches
        {
            [HarmonyBefore("com.nandbrew.nandfixes")]
            [HarmonyPostfix]
            [HarmonyPatch("OnLoad")]
            public static void OnLoadShipItemHolder(ShipItem __instance)
            {
                if (__instance is ShipItemLampHook)
                    __instance.gameObject.AddComponent<ShipItemHolder>();

                if (__instance is ShipItemBroom)
                {
                    var attachable = __instance.gameObject.AddComponent<HolderAttachable>();
                    attachable.PositionOffset = Offsets.AttachedItems.GetOffset(__instance.name).Position;
                }
                if (__instance is ShipItemHammer)
                {
                    var attachable = __instance.gameObject.AddComponent<HolderAttachable>();
                    var offset = Offsets.AttachedItems.GetOffset(__instance.name);
                    attachable.PositionOffset = offset.Position;
                    attachable.RotationOffset = offset.Rotation;
                }
                if (__instance is ShipItemOar)
                {
                    var attachable = __instance.gameObject.AddComponent<HolderAttachable>();
                    var offset = Offsets.AttachedItems.GetOffset(__instance.name);
                    attachable.PositionOffset = offset.Position;
                    attachable.RotationOffset = offset.Rotation;
                }
            }

            [HarmonyPrefix]
            [HarmonyPatch("AllowOnItemClick")]
            public static bool AllowOnItemClick(GoPointerButton lookedAtButton, ShipItem __instance, ref bool __result)
            {
                if (!__instance.sold)
                {
                    return true;
                }

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
                var offset = Offsets.AttachedItems.GetOffset(__instance.name);
                attachable.PositionOffset = offset.Position;
                attachable.RotationOffset = offset.Rotation;
            }
        }

        [HarmonyPatch(typeof(ShipItemChipLog), "OnLoad")]
        private class ShipItemChipLogPatches
        {
            public static void Postfix(ShipItemChipLog __instance)
            {
                var attachable = __instance.gameObject.AddComponent<HolderAttachable>();
                attachable.PositionOffset = Offsets.AttachedItems.GetOffset(__instance.name).Position;
            }
        }

        [HarmonyPatch(typeof(ShipItemQuadrant), "OnLoad")]
        private class ShipItemQuadrantPatches
        {
            public static void Postfix(ShipItemQuadrant __instance)
            {
                var attachable = __instance.gameObject.AddComponent<HolderAttachable>();
                var offset = Offsets.AttachedItems.GetOffset(__instance.name);
                attachable.PositionOffset = offset.Position;
                attachable.RotationOffset = offset.Rotation;
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
                var offset = Offsets.AttachedItems.GetOffset(__instance.name);
                attachable.PositionOffset = offset.Position;
                attachable.RotationOffset = offset.Rotation;
                if (flipKnifeRotation.Value)
                {
                    attachable.PositionOffset = new Vector3(-attachable.PositionOffset.x, -attachable.PositionOffset.y, attachable.PositionOffset.z);
                    attachable.RotationOffset = new Vector3(attachable.RotationOffset.x - 180, attachable.RotationOffset.y, attachable.RotationOffset.z);
                }
            }

            [HarmonyPrefix]
            [HarmonyPatch("AllowOnItemClick")]
            public static bool AllowOnItemClick(ShipItemKnife __instance, GoPointerButton lookedAtButton, ref bool __result)
            {
                if (!__instance.sold)
                {
                    return true;
                }

                if (__instance.GetComponent<HolderAttachable>() != null &&
                    lookedAtButton.GetComponent<ShipItemHolder>() != null &&
                    !lookedAtButton.GetComponent<ShipItemHolder>().IsOccupied)
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
                if ((item is ShipItemSoup soup && soup.currentWater > 0) || (item is ShipItemBottle bottle && bottle.health > 0))
                {
                    NotificationUi.instance.ShowNotification("Cannot hang\nit is not empty");
                    __result = false;
                    return false;
                }

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
                TextMesh ___textRIcon,
                ref bool ___showingIcon)
            {
                var lampHook = button.GetComponent<ShipItemLampHook>();
                var holder = lampHook != null ? lampHook.GetComponent<ShipItemHolder>() : null;
                if (lampHook != null && (bool)___pointer.GetHeldItem() &&
                    holder != null &&
                    holder.IsOccupied)
                {
                    ___textLicon.gameObject.SetActive(false);
                    ___showingIcon = false;
                    ___controlsText.text = "";

                    if ((bool)___pointer.GetHeldItem().GetComponent<ShipItemHammer>() && ShipItemHammer.CanNail(button.GetComponent<ShipItem>()))
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
                else if (lampHook != null && (bool)___pointer.GetHeldItem() &&
                    (___pointer.GetHeldItem().GetComponent<HolderAttachable>() != null || ___pointer.GetHeldItem().GetComponent<HangableItem>() != null) &&
                    !holder.IsOccupied)
                {
                    ___textLicon.gameObject.SetActive(true);
                    ___showingIcon = true;
                    ___controlsText.text = $"attach {___pointer.GetHeldItem()?.GetComponent<ShipItem>()?.name}\n";

                    if ((bool)___pointer.GetHeldItem().GetComponent<ShipItemHammer>() && ShipItemHammer.CanNail(button.GetComponent<ShipItem>()))
                    {
                        if (button.GetComponent<ShipItem>().nailed)
                        {
                            ___textRIcon.gameObject.SetActive(true);
                            ___controlsText.text = $"attach {___pointer.GetHeldItem()?.GetComponent<ShipItem>()?.name}\nunlock";
                        }
                        else
                        {
                            ___textRIcon.gameObject.SetActive(true);
                            ___controlsText.text = $"attach {___pointer.GetHeldItem()?.GetComponent<ShipItem>()?.name}\nlock";
                        }
                    }
                }
            }
        }
    }
}
