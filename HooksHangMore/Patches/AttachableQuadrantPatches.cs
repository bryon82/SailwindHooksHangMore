using HarmonyLib;

namespace HooksHangMore
{
    internal class AttachableQuadrantPatches
    {
        [HarmonyPatch(typeof(ShipItemQuadrant))]
        private class ShipItemQuadrantPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("ExtraLateUpdate")]
            public static void Postfix(ShipItemQuadrant __instance)
            {
                if (!GameState.playing)
                    return;

                var holderAttachable = __instance.GetComponent<AttachableItem>();
                if (holderAttachable != null && holderAttachable.IsAttached)
                    __instance.lockX = false;
                else
                    __instance.lockX = true;
            }
        }
    }
}
