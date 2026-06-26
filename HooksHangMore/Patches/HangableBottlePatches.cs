using HarmonyLib;

namespace HooksHangMore
{
    internal class HangableBottlePatches
    {
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
                    lookedAtButton.GetComponent<AttachableItemHolder>() != null &&
                    !lookedAtButton.GetComponent<AttachableItemHolder>().IsOccupied)
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

                var holderAttachable = __instance.GetComponent<AttachableItem>();
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
    }
}
