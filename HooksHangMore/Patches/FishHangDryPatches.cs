using HarmonyLib;
using System;
using UnityEngine;
using static HooksHangMore.HHM_Plugin;

namespace HooksHangMore
{
    internal class FishHangDryPatches
    {
        private delegate void NonBurnableDescDelegate(ref string description, float amount);
        private static NonBurnableDescDelegate _nonBurnableDescription;

        [HarmonyPatch(typeof(FoodState))]
        private class FoodStatePatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("Update")]
            public static void HangDry(FoodState __instance, ShipItemFood ___food)
            {
                var holderAttachable = __instance.GetComponent<HolderAttachable>();
                if (!GameState.playing || __instance.inWater || !___food.sold || holderAttachable == null || !holderAttachable.IsAttached)
                    return;

                float num = Time.deltaTime * Sun.sun.timescale / (30f * ___food.GetEnergyPerBite());
                num *= Mathf.Lerp(1f, 10f, __instance.salted);
                num *= 1.5f; //equivalent to *= 2.5f as game adds 1x drying rate already
                if (___food.name == "eel")
                    num *= 1.5f; //bump up higher only for eel to not be too op for other fish, 3.25

                if (__instance.gameObject.layer == 26)
                    num = 0f;

                __instance.dried += num;
            }

            [HarmonyPrefix]
            [HarmonyPatch("UpdateLookText")]
            public static bool UpdateDescription(FoodState __instance, ShipItemFood ___food)
            {
                if (__instance.spoiled > 0.9f ||
                    ___food.amount >= 1.5f ||
                    (__instance.salted >= 0.99f && __instance.smoked >= 0.99f) ||
                    __instance.smoked >= 0.99f)
                {
                    return true;
                }
                else if (__instance.salted >= 0.99f && __instance.dried >= 0.99f)
                {
                    ___food.description = $"salted dried {___food.name}";
                    return false;
                }

                return true;
            }

            [HarmonyBefore(COOKED_INFO_GUID)]
            [HarmonyPostfix]
            [HarmonyPatch("UpdateLookText")]
            public static void UpdateCookedInfo(FoodState __instance, ShipItemFood ___food)
            {
                if (_nonBurnableDescription == null)
                    return;

                var holderAttachable = __instance.GetComponent<HolderAttachable>();
                if (holderAttachable == null || !holderAttachable.IsAttached)
                    return;

                string desc = ___food.description;
                _nonBurnableDescription(ref desc, __instance.dried / 0.99f);
                ___food.description = desc;
            }
        }

        internal static void GetNonBurnableDescription()
        {
            var descriptionBuilder = AccessTools.TypeByName("DescriptionBuilder");
            var method = AccessTools.Method(descriptionBuilder, "NonBurnableDescription");
            if (method == null)
                return;

            _nonBurnableDescription = (NonBurnableDescDelegate)Delegate.CreateDelegate(
                typeof(NonBurnableDescDelegate), method);
        }
    }
}
