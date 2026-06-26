using HarmonyLib;

namespace HooksHangMore
{
    internal class AttachableChipLogPatches
    {
        [HarmonyPatch(typeof(ShipItemChipLog), "OnLoad")]
        private class ShipItemChipLogPatches
        {
            public static void Postfix(ShipItemChipLog __instance)
            {
                var attachable = __instance.gameObject.AddComponent<AttachableItem>();
                attachable.PositionOffset = Offsets.AttachedItems.GetOffset(__instance.name).Position;
            }
        }
    }
}
