using HarmonyLib;
using System.Collections;
using UnityEngine;
using static HooksHangMore.HHM_Plugin;

namespace HooksHangMore
{
    internal class AttachableAnchorPatches
    {
        [HarmonyPatch(typeof(Anchor))]
        private class AnchorPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("Awake")]
            public static void AwakePatch(Anchor __instance)
            {
                var attachable = __instance.gameObject.AddComponent<AttachableItem>();
                if (Offsets.AttachedItems.TryGetOffset(__instance.name, out var offset))
                {
                    attachable.PositionOffset = offset.Position;
                    attachable.RotationOffset = offset.Rotation;
                }
                else
                {
                    LogWarning($"Attachable anchor offsets not found for {__instance.name}. Using default offsets.");
                    attachable.PositionOffset = new Vector3(0f, 0.2f, -0.13f);
                    attachable.RotationOffset = new Vector3(270f, 0f, 0f);
                }
            }

            [HarmonyPostfix]
            [HarmonyPatch("ExtraFixedUpdate")]
            public static void Postfix(Anchor __instance)
            {
                var attachable = __instance.GetComponent<AttachableItem>();
                if (attachable != null && !attachable.IsAttached)
                    return;

                __instance.GetComponent<Rigidbody>().isKinematic = true;
                __instance.GetComponent<CapsuleCollider>().isTrigger = true;
            }
        }

        [HarmonyPatch(typeof(GoPointerButton))]
        private class GoPointerButtonPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("AllowOnItemClick")]
            public static void AllowOnItemClick(GoPointerButton __instance, GoPointerButton lookedAtButton, ref bool __result)
            {
                if (!(__instance is Anchor))
                    return;

                if (__instance.GetComponent<AttachableItem>() != null &&
                    lookedAtButton.GetComponent<AttachableItemHolder>() != null &&
                    !lookedAtButton.GetComponent<AttachableItemHolder>().IsOccupied)
                {
                    __result = true;
                }
            }
        }

        [HarmonyPatch(typeof(GPButtonRopeWinch), "LimitTurning")]
        private class GPButtonRopeWinchLimitTurningPatch
        {
            public static void Postfix(GPButtonRopeWinch __instance, ref float ___currentInput)
            {
                if (__instance.name.Contains("anchor"))
                {
                    var ropeControllerAnchor = __instance.rope as RopeControllerAnchor;
                    var anchor = ropeControllerAnchor?.joint?.GetComponent<PickupableItem>();
                    var holder = anchor?.GetComponent<AttachableItem>()?.Holder;

                    if (holder == null)
                        return;

                    var distance = Vector3.Distance(ropeControllerAnchor.transform.position, holder.transform.position);
                    var num = Mathf.InverseLerp(0f, ropeControllerAnchor.maxLength, distance);

                    if (__instance.rope.currentLength - num < 0.02f && ___currentInput > 0f)
                    {
                        ___currentInput = 0f;
                    }
                }
            }
        }

        [HarmonyPatch(typeof(SaveablePrefab))]
        private class SaveablePrefabPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch("PrepareSaveData")]
            public static void SaveAnchorAttached(ShipItem ___item, ref SavePrefabData __result)
            {
                if (!(___item is ShipItemLampHook))
                    return;

                var holder = ___item.GetComponent<AttachableItemHolder>();
                if (holder == null || !holder.IsOccupied || !(holder.AttachedItem is Anchor))
                    return;

                __result.extraValue0 = 1f;
            }

            [HarmonyPrefix]
            [HarmonyPatch("Load")]
            public static void LoadAnchorAttached(SavePrefabData data, SaveablePrefab __instance)
            {
                var hook = __instance.GetComponent<ShipItemLampHook>();

                if (hook == null || data.extraValue0 != 1f)
                    return;

                LogDebug("Loading attached anchor");
                __instance.StartCoroutine(AttachAnchor(hook));
            }
        }

        internal static IEnumerator AttachAnchor(ShipItemLampHook hook)
        {
            yield return new WaitUntil(() => GetAnchor(hook) != null || !GameState.loadingBoatLocalItems);

            var anchor = GetAnchor(hook);

            if (anchor == null)
            {
                LogError($"Could not find anchor for hook {hook.name}");
                yield break;
            }

            LogDebug($"Attaching {anchor.name} to hook on {hook.transform.parent.name}");
            var anchorPickupable = anchor.GetComponent<PickupableItem>();
            var holder = hook.GetComponent<AttachableItemHolder>();
            holder.AttachItem(anchorPickupable);
            anchorPickupable.GetComponent<AttachableItem>().Holder = holder;
        }

        internal static ConfigurableJoint GetAnchor(ShipItemLampHook hook)
        {
            var shipGameObject = hook.transform.parent == null ? null : hook.transform.parent.gameObject;
            var winch = shipGameObject == null ? null : shipGameObject.GetComponentInChildren<RopeControllerAnchor>();
            var joint = winch == null ? null : winch.joint;
            return joint;
        }
    }
}
