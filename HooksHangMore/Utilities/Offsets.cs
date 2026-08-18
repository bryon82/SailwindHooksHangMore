using System.Collections.Generic;
using UnityEngine;
using static HooksHangMore.HHM_Plugin;

namespace HooksHangMore
{
    public class Offsets
    {
        public Vector3 Position { get; }
        public Vector3 Rotation { get; }
        public bool LockX { get; }
        public bool LockZ { get; }
        public Offsets(
            Vector3 positionOffset,
            Vector3 rotationOffset = default,
            bool lockX = true,
            bool lockZ = true,
            bool isFish = false)
        {
            Position = positionOffset;
            Rotation = isFish ? new Vector3(0f, 90f, 270f) : rotationOffset;
            LockX = lockX;
            LockZ = lockZ;
        }

        public static Dictionary<string, Offsets> HangingItems = new Dictionary<string, Offsets>
        {
            { "70 bucket(Clone)", new Offsets(new Vector3(0f, -0.245f, 0f)) },
            { "382 kettle A(Clone)", new Offsets(new Vector3(0f, -0.289f, 0f)) },
            { "383 kettle E(Clone)", new Offsets(new Vector3(0f, -0.2f, -0.01f)) },
            { "384 kettle M(Clone)", new Offsets(new Vector3(0f, -0.26f, 0f)) },
            { "156 pot(Clone)", new Offsets(new Vector3(0.02f, -0.175f, -0.11f), new Vector3(0f, 270f, 40f)) },
            { "157 pot big(Clone)", new Offsets(new Vector3(0.025f, -0.205f, -0.13f), new Vector3(0f, 270f, 50f)) },
            { "102 mug metal(Clone)", new Offsets(new Vector3(0f, -0.12f, -0.02f), new Vector3(0f, 90f, -28f)) },
            { "102 mug metal", new Offsets(new Vector3(0f, -0.12f, -0.02f), new Vector3(0f, 90f, -28f)) },
            { "103 mug metal gold(Clone)", new Offsets(new Vector3(0f, -0.12f, -0.02f), new Vector3(0f, 90f, -20f)) },
            { "100 mug wood(Clone)", new Offsets(new Vector3(-0.07f, -0.075f, -0.02f), new Vector3(0f, 0f, 270f)) },
            { "100 mug wood", new Offsets(new Vector3(-0.07f, -0.075f, -0.02f), new Vector3(0f, 0f, 270f)) },
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

        //public static Dictionary<int, Offsets> HangingItems = new Dictionary<int, Offsets>
        //{
        //    { 70, new Offsets(new Vector3(0f, -0.245f, 0f)) }, // bucket
        //    { 382, new Offsets(new Vector3(0f, -0.289f, 0f)) }, // kettle A
        //    { 383, new Offsets(new Vector3(0f, -0.2f, -0.01f)) }, // kettle E
        //    { 384, new Offsets(new Vector3(0f, -0.26f, 0f)) }, // kettle M
        //    { 156, new Offsets(new Vector3(0.02f, -0.175f, -0.11f), new Vector3(0f, 270f, 40f)) }, // pot
        //    { 157, new Offsets(new Vector3(0.025f, -0.205f, -0.13f), new Vector3(0f, 270f, 50f)) }, // pot big
        //    { 102, new Offsets(new Vector3(0f, -0.12f, -0.02f), new Vector3(0f, 90f, -28f)) }, // mug metal
        //    { 103, new Offsets(new Vector3(0f, -0.12f, -0.02f), new Vector3(0f, 90f, -20f)) }, // mug metal gold
        //    { 100, new Offsets(new Vector3(-0.07f, -0.075f, -0.02f), new Vector3(0f, 0f, 270f)) }, // mug wood
        //    { 31, new Offsets(new Vector3(-0.035f, -0.14f, 0f), isFish: true) }, // templefish (A)
        //    { 32, new Offsets(new Vector3(-0.05f, -0.21f, 0f), isFish: true) }, // sunspot fish (A)
        //    { 46, new Offsets(new Vector3(0f, -0.27f, 0f), isFish: true) }, // tuna (A)
        //    { 33, new Offsets(new Vector3(-0.035f, -0.3f, 0f), isFish: true) }, // salmon (E)
        //    { 34, new Offsets(new Vector3(-0.006f, -0.65f, 0.01f), isFish: true) }, // eel (E)
        //    { 35, new Offsets(new Vector3(-0.05f, -0.28f, 0f), isFish: true) }, // shimmertail (E)
        //    { 36, new Offsets(new Vector3(-0.04f, -0.28f, 0f), isFish: true) }, // trout (M)
        //    { 37, new Offsets(new Vector3(-0.03f, -0.2f, 0f), isFish: true) }, // north fish (M)
        //    { 38, new Offsets(new Vector3(-0.035f, -0.245f, 0f), isFish: true) }, // blackfin hunter (M)
        //    { 140, new Offsets(new Vector3(0f, -0.27f, 0f), isFish: true) }, // gold albacore
        //    { 141, new Offsets(new Vector3(-0.035f, -0.265f, 0f), isFish: true) }, // swamp snapper
        //    { 142, new Offsets(new Vector3(-0.045f, -0.19f, 0f), isFish: true) }, // blue bubbler
        //    { 148, new Offsets(new Vector3(-0.065f, -0.2f, 0f), isFish: true) }, // fire fish
        //};

        public static Dictionary<string, Offsets> AttachedItems = new Dictionary<string, Offsets>
        {
            { "fishing rod", new Offsets(new Vector3(0.309f, 1.1f, -0.38f), new Vector3(-40f, 180f, 0f)) },
            { "broom", new Offsets(new Vector3(0f, -0.25f, -0.11f)) },
            { "chip log", new Offsets(new Vector3(0.002f, 0.25f, -0.12f)) },
            { "oar", new Offsets(new Vector3(0f, -0.65f, -0.11f), new Vector3(180f, 0f, 0f)) },
            { "quadrant", new Offsets(new Vector3(-0.0155f, 0.164f, -0.115f), new Vector3(90f, -90f, 0f)) },
            { "knife", new Offsets(new Vector3(0.05f, -0.115f, -0.182f), new Vector3(270f, 270f, 0f)) },
            { "hammer", new Offsets(new Vector3(0.0f, -0.3f, -0.22f), new Vector3(270f, 270f, 0f)) },
            { "anchor_E (1)", new Offsets(new Vector3(0f, 0.2f, -0.13f), new Vector3(270f, 0f, 0f)) },
            { "anchor_E", new Offsets(new Vector3(0f, 0.2f, -0.13f), new Vector3(270f, 0f, 0f)) },
            { "anchor_M", new Offsets(new Vector3(0f, 0.2f, -0.13f), new Vector3(270f, 0f, 0f)) },
            { "anchor_A", new Offsets(new Vector3(0f, 0.2f, -0.13f), new Vector3(270f, 0f, 0f)) },
            { "512 anemometer(Clone)", new Offsets(new Vector3(0.002f, 0.25f, -0.12f)) },
        };

        public static bool AddAttachedOffset(string itemName, Vector3 positionOffset, Vector3 rotationOffset)
        {
            if (AttachedItems.ContainsKey(itemName))
            {
                LogWarning($"Attached item offset for {itemName} already exists.");
                return false;
            }
            else
            {
                AttachedItems.Add(itemName, new Offsets(positionOffset, rotationOffset));
                return true;
            }
        }

        public static bool AddHangingOffset(string itemName, Vector3 positionOffset, Vector3 rotationOffset, bool lockX, bool lockZ)
        {
            if (HangingItems.ContainsKey(itemName))
            {
                LogWarning($"Hanging item offset for {itemName} already exists.");
                return false;
            }
            else
            {
                HangingItems.Add(itemName, new Offsets(positionOffset, rotationOffset, lockX, lockZ));
                return true;
            }
        }
    }

    internal static class OffsetsExtensions
    {
        public static bool TryGetOffset(this Dictionary<string, Offsets> dict, Anchor item, out Offsets offset)
            => dict.TryGetValue(item.name, out offset);

        public static bool TryGetOffset(this Dictionary<string, Offsets> dict, HangableItem item, out Offsets offset)
            => dict.TryGetValue(item.name, out offset);

        public static bool TryGetOffset(this Dictionary<string, Offsets> dict, ShipItem item, out Offsets offset)
        {
            var key = dict.ContainsKey(item.transform.name) ? item.transform.name : item.name;
            return dict.TryGetValue(key, out offset);
        }            

        public static Offsets GetOffset(this Dictionary<string, Offsets> dict, string itemName)            
            => dict.TryGetValue(itemName, out Offsets offset) ? offset : null;

        public static bool IsHangable(this Dictionary<string, Offsets> dict, string itemName)
            => dict.ContainsKey(itemName);
    }
}
