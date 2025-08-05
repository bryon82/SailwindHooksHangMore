using Crest;
using HarmonyLib;
using UnityEngine;
using static HooksHangMore.HHM_Plugin;

namespace HooksHangMore
{
    internal class FishingRodPatches
    {
        [HarmonyPatch(typeof(FishingRodFish))]
        private class FishingRodFishPatches
        {
            [HarmonyBefore(IDLE_FISHING_GUID)]
            [HarmonyPostfix]
            [HarmonyPatch("Update")]
            public static void Postfix(
                FishingRodFish __instance,
                ShipItemFishingRod ___rod,
                SimpleFloatingObject ___floater,
                ConfigurableJoint ___bobberJoint,
                ref float ___fishTimer)
            {
                if (__instance.currentFish != null ||
                    ___rod.health <= 0f ||
                    !AttachedItems.ContainsKey(___rod) ||
                    !___floater.InWater ||
                    ___bobberJoint.linearLimit.limit <= 1f ||
                    __instance.gameObject.layer == 16)
                {
                    return;
                }

                float divisor;

                if (GameState.distanceToLand < 500)
                {
                    divisor = IdleFishingFound ? 20f : 6.67f;
                }
                else
                {
                    divisor = IdleFishingFound ? 10f : 5.13f;
                }

                ___fishTimer -= Time.deltaTime;
                float value = Vector3.Distance(__instance.transform.position, ___rod.transform.position);
                float num = Mathf.InverseLerp(3f, 20f, value) * 2.5f + 0.5f;
                if (___fishTimer <= 0f)
                {
                    ___fishTimer = 1f;                    
                    num /= divisor;
                    if (Random.Range(0f, 100f) < num)
                    {
                        __instance.CatchFish();
                    }
                }
            }
        }
    }
}
