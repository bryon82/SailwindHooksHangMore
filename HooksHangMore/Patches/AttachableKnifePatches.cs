using HarmonyLib;
using UnityEngine;
using static HooksHangMore.Configs;

namespace HooksHangMore
{
    internal class AttachableKnifePatches
    {
        [HarmonyPatch(typeof(ShipItemKnife))]
        private class ShipItemKnifePatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("OnLoad")]
            public static void AddComponent(ShipItemKnife __instance)
            {
                var attachable = __instance.gameObject.AddComponent<AttachableItem>();
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
