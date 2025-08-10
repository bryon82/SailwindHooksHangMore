using HarmonyLib;

namespace HooksHangMore.Patches
{
    internal class QuadrantPatches
    {
        [HarmonyPatch(typeof(ShipItemQuadrant), "ExtraLateUpdate")]
        private class ShipItemQuadrantPatches
        {
            public static void Postfix(ShipItemQuadrant __instance)
            {
                if (!GameState.playing)
                    return;

                var holderAttachable = __instance.GetComponent<HolderAttachable>();
                if (holderAttachable != null && holderAttachable.IsAttached)
                {
                    __instance.lockX = false;
                }
                else
                {
                    __instance.lockX = true;
                }
            }
        }
    }
}
