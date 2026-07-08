using HarmonyLib;

namespace HooksHangMore
{
    internal class HangableKettlePatches
    {
        [HarmonyPatch(typeof(ShipItemKettle))]
        private class ShipItemKettlePatches
        {
            [HarmonyPrefix]
            [HarmonyPatch("AllowOnItemClick")]
            public static bool AllowOnItemClick(ShipItemKettle __instance, GoPointerButton lookedAtButton, ref bool __result)
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
        }
    }
}
