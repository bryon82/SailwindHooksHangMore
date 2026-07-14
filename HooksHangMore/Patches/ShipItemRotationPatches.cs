using HarmonyLib;
using UnityEngine;

namespace HooksHangMore
{
    internal class ShipItemRotationPatches
    {
        [HarmonyPatch(typeof(ShipItem))]
        private class ShipItemPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("OnLoad")]
            public static void RemoveDescription(ShipItem __instance)
            {
                if (__instance is ShipItemLampHook)
                    __instance.gameObject.AddComponent<PickupableItemAddOns>();
            }

            [HarmonyPrefix]
            [HarmonyPatch("Update")]
            public static bool RotateAttachments(
                ShipItem __instance,
                Transform ___itemRigidbody,
                ref bool ___inRangeOfWall,
                ref Vector3 ___attachPos,
                ref Quaternion ___attachRot)
            {
                if ((bool)__instance.held && __instance.wallAttachment)
                {
                    var addOns = __instance.GetComponent<PickupableItemAddOns>();
                    if (addOns == null)
                        return true;

                    var tiltAngle = addOns.heldRotationYOffset;
                    var forward = ___itemRigidbody.forward;
                    if (Physics.Raycast(new Ray(___itemRigidbody.position, forward), out var hitInfo, 1.3f))
                    {
                        if (hitInfo.distance < 0.1f)
                            ___inRangeOfWall = false;

                        else
                        {
                            ___attachPos = hitInfo.point;
                            var baseRot = Quaternion.LookRotation(-hitInfo.normal, Vector3.up);

                            if (hitInfo.normal.y > 0.8f)
                            {
                                baseRot = Quaternion.LookRotation(-hitInfo.normal, __instance.transform.up);
                            }
                            var tilt = Quaternion.AngleAxis(tiltAngle, Vector3.forward);
                            ___attachRot = baseRot * tilt;

                            ___inRangeOfWall = true;
                            __instance.InvokePrivateMethod(
                                "SetUpTargeter",
                                ___attachPos,
                                ___attachRot,
                                __instance.GetComponent<MeshFilter>().sharedMesh);
                        }
                    }
                    else
                        ___inRangeOfWall = false;

                }
                else
                    ___inRangeOfWall = false;

                if (__instance.GetPrivateField<bool>("isLookedAt"))
                    __instance.UpdateLookText();

                __instance.InvokePrivateMethod("ProcessSaveable");
                return false;
            }
        }

        [HarmonyPatch(typeof(GoPointer), "LateUpdate")]
        private class GoPointerPatches
        {
            public static void Postfix(PickupableItem ___heldItem)
            {
                if (___heldItem == null || ___heldItem.big)
                    return;

                var addOns = ___heldItem.GetComponent<PickupableItemAddOns>();
                if (addOns == null || addOns.heldRotationYOffset == 0f)
                    return;

                ___heldItem.transform.rotation *=
                    Quaternion.AngleAxis(addOns.heldRotationYOffset, Vector3.forward);
            }
        }
    }
}
