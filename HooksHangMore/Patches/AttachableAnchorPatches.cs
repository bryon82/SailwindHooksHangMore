using HarmonyLib;
using System.Collections;
using System.Threading;
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
                var offset = Offsets.AttachedItems.GetOffset(__instance.name);
                attachable.PositionOffset = offset.Position;
                attachable.RotationOffset = offset.Rotation;
            }

            [HarmonyPostfix]
            [HarmonyPatch("ExtraFixedUpdate")]
            public static void Postfix(Anchor __instance)
            {
                var attachable = __instance.GetComponent<AttachableItem>();
                if (attachable != null && !attachable.IsAttached)
                    return;

                var holder = attachable.Holder;
                if (holder == null)
                    return;

                __instance.transform.position = holder.transform.TransformPoint(attachable.PositionOffset);
                __instance.transform.rotation = holder.transform.rotation * Quaternion.Euler(attachable.RotationOffset);
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

                LogDebug($"AllowOnItemClick: __instance={__instance.name}, lookedAtButton={lookedAtButton.name}");
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

                //if (holder == null || !holder.IsOccupied || !holder.AttachedItem.name.Contains("anchor"))
                if (holder == null || !holder.IsOccupied || !(holder.AttachedItem is Anchor))
                    return;
                
                __result.extraValue0 = 1f;
            }

            [HarmonyPrefix]
            [HarmonyPatch("Load")]
            public static void LoadAnchorAttached(SavePrefabData data, SaveablePrefab __instance)
            {
                var shipItem = __instance.GetComponent<ShipItem>();

                if (shipItem != null && shipItem is ShipItemLampHook hook && data.extraValue0 == 1f)
                {
                    LogDebug("Loading attached anchor");
                    __instance.StartCoroutine(AttachAnchor(hook));                                   
                }
            }
        }

        internal static IEnumerator AttachAnchor(ShipItemLampHook hook)
        {
            //yield return new WaitUntil(() => GameState.playing && !GameState.justStarted && !GameState.loadingBoatLocalItems);
            yield return new WaitUntil(() => hook.transform.parent?.gameObject?.GetComponentInChildren<RopeControllerAnchor>()?.joint != null);

            var winch = hook.transform.parent.gameObject.GetComponentInChildren<RopeControllerAnchor>();

            if (winch == null || winch.joint == null)
            {
                LogError($"Could not find RopeControllerAnchor or joint for hook {hook.name}");
                yield break;
            }        

            LogDebug($"Attaching {winch.joint.name} to hook on {hook.transform.parent.name}");
            var anchorPickupable = winch.joint.GetComponent<PickupableItem>();
            var holder = hook.GetComponent<AttachableItemHolder>();
            holder.AttachItem(anchorPickupable);
            anchorPickupable.GetComponent<AttachableItem>().Holder = holder;
        }
    }
}
