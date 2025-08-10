using BepInEx.Configuration;

namespace HooksHangMore
{
    internal class Configs
    {
        internal static ConfigEntry<bool> flipKnifeRotation;

        internal static void InitializeConfigs()
        {
            var config = HHM_Plugin.Instance.Config;

            flipKnifeRotation = config.Bind(
                "Settings",
                "Knife hangs down",
                false,
                "Knife hangs down in the lamp hook instead of pointing up.");
        }
    }
}
