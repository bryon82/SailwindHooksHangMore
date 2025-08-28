using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;

namespace HooksHangMore
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    [BepInDependency(IDLE_FISHING_GUID, BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency(COOKED_INFO_GUID, BepInDependency.DependencyFlags.SoftDependency)]
    public class HHM_Plugin : BaseUnityPlugin
    {
        public const string PLUGIN_GUID = "com.raddude82.hookshangmore";
        public const string PLUGIN_NAME = "HooksHangMore";
        public const string PLUGIN_VERSION = "1.0.5";

        public const string IDLE_FISHING_GUID = "com.isa_idlefishing.patch";
        public const string COOKED_INFO_GUID = "pr0skynesis.cookedinfo";

        internal static HHM_Plugin Instance { get; private set; }
        private static ManualLogSource _logger;

        internal static void LogDebug(string message) => _logger.LogDebug(message);
        internal static void LogInfo(string message) => _logger.LogInfo(message);
        internal static void LogWarning(string message) => _logger.LogWarning(message);
        internal static void LogError(string message) => _logger.LogError(message);

        internal static bool IdleFishingFound { get; private set; } = false;
        internal static Dictionary<ShipItem, ShipItemHolder> AttachedItems { get; set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            _logger = Logger;

            AttachedItems = new Dictionary<ShipItem, ShipItemHolder>();

            foreach (var plugin in Chainloader.PluginInfos)
            {
                var metadata = plugin.Value.Metadata;
                if (metadata.GUID.Equals(IDLE_FISHING_GUID))
                {
                    LogInfo($"{IDLE_FISHING_GUID} found");
                    IdleFishingFound = true;
                }
                if (metadata.GUID.Equals(COOKED_INFO_GUID))
                {
                    LogInfo($"{COOKED_INFO_GUID} found");
                    FishHangDryPatches.GetNonBurnableDescription();
                }
            }

            Configs.InitializeConfigs();

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PLUGIN_GUID);
        }
    }
}
