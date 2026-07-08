using HarmonyLib;
using UnityEngine;
using static HooksHangMore.HHM_Plugin;

namespace HooksHangMore
{
    internal class HangablePatches
    {
        [HarmonyPatch(typeof(ShipItem))]
        private class ShipItemPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("Awake")]
            public static void AddComponent(ShipItem __instance)
            {
                if (Offsets.HangingItems.IsHangable(__instance.transform.name))
                    __instance.gameObject.AddComponent<HangableItem>();
            }

            [HarmonyPostfix]
            [HarmonyPatch("OnPickup")]
            public static void OnPickup(ShipItem __instance)
            {
                if (!Offsets.HangingItems.IsHangable(__instance.transform.name))
                    return;

                var hangable = __instance.GetComponent<HangableItem>();
                if (hangable != null)
                    hangable.DisconnectJoint();
            }

            [HarmonyPostfix]
            [HarmonyPatch("OnEnterInventory")]
            public static void OnEnterInventory(ShipItem __instance)
            {
                if (!Offsets.HangingItems.IsHangable(__instance.transform.name))
                    return;

                var hangable = __instance.GetComponent<HangableItem>();
                if (hangable != null)
                    hangable.DisconnectJoint();
            }
        }

        [HarmonyPatch(typeof(HangableItem))]
        private class HangableItemPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("ConnectJoint")]
            public static void AdjustAnchor(HangableItem __instance)
            {
                if (GameState.currentlyLoading || GameState.loadingBoatLocalItems)
                    return;

                if (!Offsets.HangingItems.TryGetOffset(__instance.name, out Offsets offset))
                    return;

                var eulerAngles = __instance.transform.eulerAngles;
                eulerAngles.y += offset.RotY;
                __instance.transform.eulerAngles = eulerAngles;
            }

            [HarmonyPrefix]
            [HarmonyPatch("LateUpdate")]
            public static bool ApplyPositionOffset(HangableItem __instance, Collider ___currentHook)
            {
                if (!__instance.IsHanging())
                    return true;

                if (!Offsets.HangingItems.TryGetOffset(__instance.name, out Offsets offset))
                    return true;

                Vector3 eulerAngles = __instance.transform.eulerAngles;
                if (offset.LockX)
                    eulerAngles.x = offset.RotX;

                if (offset.LockZ)
                    eulerAngles.z = offset.RotZ;

                __instance.transform.eulerAngles = eulerAngles;
                __instance.transform.position = ___currentHook.transform.position +
                    ___currentHook.transform.forward * (-0.128f + offset.Position.z) + 
                    ___currentHook.transform.right * offset.Position.x;

                __instance.transform.position = 
                    new Vector3(__instance.transform.position.x, __instance.transform.position.y + offset.Position.y, __instance.transform.position.z);

                return false;
            }
        }
    }
}
