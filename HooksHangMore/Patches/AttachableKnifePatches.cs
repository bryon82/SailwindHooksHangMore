using HarmonyLib;

namespace HooksHangMore
{
    internal class AttachableKnifePatches
    {
        [HarmonyPatch(typeof(ShipItemKnife))]
        private class ShipItemKnifePatches
        {
            [HarmonyPrefix]
            [HarmonyPatch("AllowOnItemClick")]
            public static bool AllowOnItemClick(ShipItemKnife __instance, GoPointerButton lookedAtButton, ref bool __result)
            {
                if (!__instance.sold)
                {
                    return true;
                }

                if (__instance.GetComponent<AttachableItem>() != null &&
                    lookedAtButton.GetComponent<AttachableItemHolder>() != null &&
                    !lookedAtButton.GetComponent<AttachableItemHolder>().IsOccupied)
                {
                    __result = true;
                    return false;
                }

                return true;
            }
        }
    }
}
