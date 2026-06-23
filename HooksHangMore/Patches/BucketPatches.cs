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

                var hangable = __instance.GetComponent<HangableItem>();
                if (hangable != null && hangable.IsHanging())
                {
                    __instance.bucketHandle.localRotation = Quaternion.Euler(Vector3.zero);
                }
            }
        }
    }
}
