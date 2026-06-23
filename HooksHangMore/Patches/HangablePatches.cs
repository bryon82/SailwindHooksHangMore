using HarmonyLib;
using UnityEngine;
using static HooksHangMore.HHM_Plugin;

namespace HooksHangMore
{
    internal class HangablePatches
    {
        [HarmonyPatch(typeof(ShipItem))]
        private class ShipItemPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("OnPickup")]
            public static void OnPickup(ShipItem __instance)
            {
                if (!Offsets.HangingItems.IsHangable(__instance.transform.name))
                    return;

                var hangable = __instance.GetComponent<HangableItem>();
                if (hangable != null)
                    hangable.DisconnectJoint();
            }

            [HarmonyPostfix]
            [HarmonyPatch("OnEnterInventory")]
            public static void OnEnterInventory(ShipItem __instance)
            {
                if (!Offsets.HangingItems.IsHangable(__instance.transform.name))
                    return;

                var hangable = __instance.GetComponent<HangableItem>();
                if (hangable != null)
                    hangable.DisconnectJoint();
            }
        }

        [HarmonyPatch(typeof(HangableItem))]
        private class HangableItemPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("ConnectJoint")]
            public static void AdjustAnchor(HangableItem __instance, Collider hook)
            {
                if (GameState.currentlyLoading || GameState.loadingBoatLocalItems)
                    return;

                if (!Offsets.HangingItems.TryGetOffset(__instance.name, out Offsets offset))
                    return;

                var eulerAngles = __instance.transform.eulerAngles;
                eulerAngles.y += offset.RotY;
                __instance.transform.eulerAngles = eulerAngles;
            }

            [HarmonyPrefix]
            [HarmonyPatch("LateUpdate")]
            public static bool ApplyPositionOffset(HangableItem __instance, Collider ___currentHook)
            {
                if (!__instance.IsHanging())
                    return true;

                if (!Offsets.HangingItems.TryGetOffset(__instance.name, out Offsets offset))
                    return true;
                
                Vector3 eulerAngles = __instance.transform.eulerAngles;
                if (offset.LockX)                
                    eulerAngles.x = offset.RotX;

                if (offset.LockZ)                
                    eulerAngles.z = offset.RotZ;
                
                __instance.transform.eulerAngles = eulerAngles;
                __instance.transform.position = ___currentHook.transform.position +
                    ___currentHook.transform.forward * (-0.128f + offset.Position.z) + 
                    ___currentHook.transform.right * offset.Position.x;

                __instance.transform.position = 
                    new Vector3(__instance.transform.position.x, __instance.transform.position.y + offset.Position.y, __instance.transform.position.z);

                return false;
            }
        }

        [HarmonyPatch(typeof(ShipItemKettle))]
        private class ShipItemKettlePatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("OnLoad")]
            public static void AddComponent(ShipItemKettle __instance)
            {
                __instance.gameObject.AddComponent<HangableItem>();
            }

            [HarmonyPrefix]
            [HarmonyPatch("AllowOnItemClick")]
            public static bool AllowOnItemClick(ShipItemKettle __instance, GoPointerButton lookedAtButton, ref bool __result)
            {
                if (!__instance.sold)
                    return true;

                if (__instance.GetComponent<HangableItem>() != null &&
                    lookedAtButton.GetComponent<ShipItemHolder>() != null &&
                    !lookedAtButton.GetComponent<ShipItemHolder>().IsOccupied)
                {
                    __result = true;
                    return false;
                }

                return true;
            }
        }

        [HarmonyPatch(typeof(ShipItemBottle))]
        private class ShipItemBottlePatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("OnLoad")]
            public static void AddComponent(ShipItemBottle __instance)
            {
                if (Offsets.HangingItems.IsHangable(__instance.transform.name))
                    __instance.gameObject.AddComponent<HangableItem>();
            }

            [HarmonyPrefix]
            [HarmonyPatch("AllowOnItemClick")]
            public static bool AllowOnItemClick(ShipItemBottle __instance, GoPointerButton lookedAtButton, ref bool __result)
            {
                if (!Offsets.HangingItems.IsHangable(__instance.transform.name) || !__instance.sold)
                    return true;

                if (__instance.GetComponent<HangableItem>() != null &&
                    lookedAtButton.GetComponent<ShipItemHolder>() != null &&
                    !lookedAtButton.GetComponent<ShipItemHolder>().IsOccupied)
                {
                    __result = true;
                    return false;
                }

                return true;
            }

            [HarmonyPrefix]
            [HarmonyPatch("OnItemClick")]
            public static bool OnItemClick(ShipItemSoup __instance, PickupableItem heldItem, ref bool __result)
            {
                if (!Offsets.HangingItems.IsHangable(__instance.transform.name) || !__instance.sold)
                    return true;

                var holderAttachable = __instance.GetComponent<HolderAttachable>();
                if (holderAttachable != null &&
                    holderAttachable.IsAttached)
                {
                    if (heldItem.GetComponent<ShipItemBottle>())
                    {
                        NotificationUi.instance.ShowNotification("Cannot fill\nwhile it is hanging");
                        __result = false;
                        return false;
                    }
                }

                return true;
            }
        }        

        [HarmonyPatch(typeof(ShipItemSoup))]
        private class ShipItemSoupPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("OnLoad")]
            public static void AddComponent(ShipItemSoup __instance)
            {
                if (Offsets.HangingItems.IsHangable(__instance.transform.name))
                    __instance.gameObject.AddComponent<HangableItem>();
            }

            [HarmonyPrefix]
            [HarmonyPatch("AllowOnItemClick")]
            public static bool AllowOnItemClick(ShipItemSoup __instance, GoPointerButton lookedAtButton, ref bool __result)
            {
                if (!__instance.sold)
                    return true;

                if (__instance.GetComponent<HangableItem>() != null &&
                    lookedAtButton.GetComponent<ShipItemHolder>() != null &&
                    !lookedAtButton.GetComponent<ShipItemHolder>().IsOccupied)
                {
                    __result = true;
                    return false;
                }

                return true;
            }

            [HarmonyPrefix]
            [HarmonyPatch("OnItemClick")]
            public static bool OnItemClick(ShipItemSoup __instance, PickupableItem heldItem, ref bool __result)
            {
                if (!__instance.sold)
                    return true;

                var hangable = __instance.GetComponent<HangableItem>();
                if (hangable != null &&
                    hangable.IsHanging())
                {
                    if (heldItem.GetComponent<ShipItemBottle>())
                    {
                        NotificationUi.instance.ShowNotification("Cannot fill\nwhile it is hanging");
                        __result = false;
                        return false;
                    }
                }

                return true;
            }
        }

        [HarmonyPatch(typeof(ShipItemFood))]
        private class ShipItemFoodPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("OnLoad")]
            public static void AddComponent(ShipItemFood __instance)
            {
                if (Offsets.HangingItems.IsHangable(__instance.transform.name))
                    __instance.gameObject.AddComponent<HangableItem>();
            }

            [HarmonyPrefix]
            [HarmonyPatch("AllowOnItemClick")]
            public static bool AllowOnItemClick(GoPointerButton lookedAtButton, ShipItemFood __instance, ref bool __result)
            {
                if (!__instance.sold)
                    return true;

                if (__instance.GetComponent<HangableItem>() != null &&
                    lookedAtButton.GetComponent<ShipItemHolder>() != null &&
                    !lookedAtButton.GetComponent<ShipItemHolder>().IsOccupied)
                {
                    __result = true;
                    return false;
                }

                return true;
            }
        }
    }
}
