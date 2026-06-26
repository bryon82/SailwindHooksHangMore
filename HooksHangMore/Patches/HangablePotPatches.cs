using HarmonyLib;

namespace HooksHangMore
{
    internal class HangablePotPatches
    {
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
    }
}
