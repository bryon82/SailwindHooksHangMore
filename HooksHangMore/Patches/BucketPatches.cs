using HarmonyLib;
using UnityEngine;

namespace HooksHangMore
{
    internal class BucketPatches
    {
        [HarmonyPatch(typeof(Mug), "Update")]
        private class MugPatches
        {
            public static void Postfix(Mug __instance)
            {
                if (!GameState.playing || !__instance.name.Contains("bucket"))
                    return;

                var holderAttachable = __instance.GetComponent<HolderAttachable>();
                if (holderAttachable != null && holderAttachable.IsAttached)
                {
                    __instance.bucketHandle.localRotation = Quaternion.Euler(Vector3.zero);
                }
            }
        }
    }
}
