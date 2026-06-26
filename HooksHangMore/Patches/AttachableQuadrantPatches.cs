using HarmonyLib;

namespace HooksHangMore
{
    internal class AttachableQuadrantPatches
    {
        [HarmonyPatch(typeof(ShipItemQuadrant))]
        private class ShipItemQuadrantPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("OnLoad")]
            public static void AddAttachable(ShipItemQuadrant __instance)
            {
                var attachable = __instance.gameObject.AddComponent<AttachableItem>();
                var offset = Offsets.AttachedItems.GetOffset(__instance.name);
                attachable.PositionOffset = offset.Position;
                attachable.RotationOffset = offset.Rotation;
            }

            [HarmonyPostfix]
            [HarmonyPatch("ExtraLateUpdate")]
            public static void Postfix(ShipItemQuadrant __instance)
            {
                if (!GameState.playing)
                    return;

                var holderAttachable = __instance.GetComponent<AttachableItem>();
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
