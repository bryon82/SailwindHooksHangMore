using HarmonyLib;
using System.Collections;
using UnityEngine;
using static HooksHangMore.HHM_Plugin;

namespace HooksHangMore
{
    internal class HangableBottlePatches
    {
        [HarmonyPatch(typeof(ShipItemBottle))]
        private class ShipItemBottlePatches
        {
            [HarmonyPrefix]
            [HarmonyPatch("AllowOnItemClick")]
            public static bool AllowOnItemClick(ShipItemBottle __instance, GoPointerButton lookedAtButton, ref bool __result)
            {
                if (!Offsets.HangingItems.IsHangable(__instance.transform.name) || !__instance.sold)
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
                if (!Offsets.HangingItems.IsHangable(__instance.transform.name) || !__instance.sold)
                    return true;

                var holderAttachable = __instance.GetComponent<AttachableItem>();
                if (holderAttachable != null &&
                    holderAttachable.IsAttached)
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

        [HarmonyPatch(typeof(Mug), "Update")]
        private class MugPatches
        {
            public static void Postfix(Mug __instance)
            {
                if (!GameState.playing || !__instance.name.Contains("bucket"))
                    return;

                var hangable = __instance.GetComponent<HangableItem>();
                if (hangable != null && hangable.IsHanging())
                {
                    __instance.bucketHandle.localRotation = Quaternion.Euler(Vector3.zero);
                }
            }
        }

        [HarmonyPatch(typeof(SaveablePrefab))]
        private class SaveablePrefabPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("PrepareSaveData")]
            public static void SaveBucketHanging(ShipItem ___item, ref SavePrefabData __result)
            {
                if (!(___item is ShipItemBottle) || ___item.name != "bucket")
                    return;

                var attachable = ___item.GetComponent<HangableItem>();
                if (attachable == null || !attachable.IsHanging())
                    return;

                __result.extraValue0 = 1f;
            }

            [HarmonyPrefix]
            [HarmonyPatch("Load")]
            public static void LoadBucketHanging(SavePrefabData data, SaveablePrefab __instance)
            {
                var bottle = __instance.GetComponent<ShipItemBottle>();

                if (bottle == null || bottle.name != "bucket" || data.extraValue0 != 1f)
                    return;

                LogDebug("Loading hanging bucket");
                __instance.StartCoroutine(HangBucket(bottle));
            }
        }

        internal static IEnumerator HangBucket(ShipItemBottle bucket)
        {
            LogDebug("Shifting bucket box collider up");
            var col = bucket.GetComponent<BoxCollider>();
            col.center = new Vector3(col.center.x, 0.105417f, col.center.z);

            yield return new WaitUntil(() => bucket.GetComponent<HangableItem>() != null && bucket.GetComponent<HangableItem>().IsHanging());

            col.center = new Vector3(col.center.x, -0.055417f, col.center.z);

            LogDebug("Shifted bucket box collider back down");
        }
    }
}
