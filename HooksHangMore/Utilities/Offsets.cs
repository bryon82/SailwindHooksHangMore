using System.Collections.Generic;
using UnityEngine;

namespace HooksHangMore
{
    internal class Offsets
    {
        public Vector3 Position { get; }
        public Vector3 Rotation { get; }
        public float RotX { get; }
        public float RotY { get; }
        public float RotZ { get; }
        public bool LockX { get; }
        public bool LockZ { get; }
        public Offsets(
            Vector3 positionOffset,
            Vector3 rotationOffset = default,
            bool isFish = false,
            float rotX = 0f,
            float rotY = 0f,
            float rotZ = 0f,
            bool lockX = true,
            bool lockZ = true)
        {
            Position = positionOffset;
            Rotation = rotationOffset;
            RotX = rotX;
            RotY = rotY;
            RotZ = rotZ;
            LockX = lockX;
            LockZ = lockZ;

            if (isFish)
            {
                RotY = 90f;
                RotZ = 270f;
            }
        }

        public static readonly Dictionary<string, Offsets> HangingItems = new Dictionary<string, Offsets>
        {
            { "70 bucket(Clone)", new Offsets(new Vector3(0f, -0.245f, 0f)) },
            { "382 kettle A(Clone)", new Offsets(new Vector3(0f, -0.289f, 0f)) },
            { "383 kettle E(Clone)", new Offsets(new Vector3(0f, -0.2f, -0.01f)) },
            { "384 kettle M(Clone)", new Offsets(new Vector3(0f, -0.26f, 0f)) },
            { "156 pot(Clone)", new Offsets(new Vector3(0.02f, -0.175f, -0.11f), rotY: 270f, rotZ: 40f) },
            { "157 pot big(Clone)", new Offsets(new Vector3(0.025f, -0.205f, -0.13f), rotY: 270f, rotZ: 50f) },
            { "102 mug metal(Clone)", new Offsets(new Vector3(0f, -0.12f, -0.02f), rotY: 90f, rotZ:-28f) },
            { "102 mug metal", new Offsets(new Vector3(0f, -0.12f, -0.02f), rotY: 90f, rotZ: -28f) },
            { "103 mug metal gold(Clone)", new Offsets(new Vector3(0f, -0.12f, -0.02f), rotY: 90f, rotZ: -20f) },
            { "100 mug wood(Clone)", new Offsets(new Vector3(-0.07f, -0.075f, -0.02f), rotZ: 270f) },
            { "100 mug wood", new Offsets(new Vector3(-0.07f, -0.075f, -0.02f), rotZ: 270f) },
            { "31 templefish (A)(Clone)", new Offsets(new Vector3(-0.035f, -0.14f, 0f), isFish: true) },
            { "32 sunspot fish (A)(Clone)", new Offsets(new Vector3(-0.05f, -0.21f, 0f), isFish: true) },
            { "46 tuna (A)(Clone)", new Offsets(new Vector3(0f, -0.27f, 0f), isFish: true) },
            { "33 salmon (E)(Clone)", new Offsets(new Vector3(-0.035f, -0.3f, 0f), isFish: true) },
            { "34 eel (E)(Clone)", new Offsets(new Vector3(-0.006f, -0.65f, 0.01f), isFish: true) },
            { "35 shimmertail (E)(Clone)", new Offsets(new Vector3(-0.05f, -0.28f, 0f), isFish: true) },
            { "36 trout (M)(Clone)", new Offsets(new Vector3(-0.04f, -0.28f, 0f), isFish: true) },
            { "37 north fish (M)(Clone)", new Offsets(new Vector3(-0.03f, -0.2f, 0f), isFish: true) },
            { "38 blackfin hunter (M)(Clone)", new Offsets(new Vector3(-0.035f, -0.245f, 0f), isFish: true) },
            { "140 gold albacore(Clone)", new Offsets(new Vector3(0f, -0.27f, 0f), isFish: true) },
            { "141 swamp fish 1 (snapper(Clone)", new Offsets(new Vector3(-0.035f, -0.265f, 0f), isFish: true) }, // swamp snapper
            { "142 swamp fish 2 (bubbler)(Clone)", new Offsets(new Vector3(-0.045f, -0.19f, 0f), isFish: true) }, // blue bubbler
            { "148 swamp fish 3(Clone)", new Offsets(new Vector3(-0.065f, -0.2f, 0f), isFish: true) }, // fire fish
        };

        public static readonly Dictionary<string, Offsets> AttachedItems = new Dictionary<string, Offsets>
        {
            { "fishing rod", new Offsets(new Vector3(0.309f, 1.1f, -0.38f), new Vector3(-40f, 180f, 0f)) },
            { "broom", new Offsets(new Vector3(0f, -0.25f, -0.11f)) },
            { "chip log", new Offsets(new Vector3(0.002f, 0.25f, -0.12f)) },
            { "oar", new Offsets(new Vector3(0f, -0.65f, -0.11f), new Vector3(180f, 0f, 0f)) },
            { "quadrant", new Offsets(new Vector3(-0.0155f, 0.164f, -0.115f), new Vector3(90f, -90f, 0f)) },
            { "knife", new Offsets(new Vector3(0.05f, -0.115f, -0.182f), new Vector3(270f, 270f, 0f)) },
            { "hammer", new Offsets(new Vector3(0.0f, -0.3f, -0.22f), new Vector3(270f, 270f, 0f)) }
        };
    }

    internal static class OffsetsExtensions
    {
        public static bool TryGetOffset(this Dictionary<string, Offsets> dict, string itemName, out Offsets offset)
            => dict.TryGetValue(itemName, out offset);

        public static Offsets GetOffset(this Dictionary<string, Offsets> dict, string itemName)            
            => dict.TryGetValue(itemName, out Offsets offset) ? offset : null;

        public static bool IsHangable(this Dictionary<string, Offsets> dict, string itemName)
            => dict.ContainsKey(itemName);
    }
}
