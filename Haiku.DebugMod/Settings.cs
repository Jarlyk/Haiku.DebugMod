using UnityEngine;
using BepInEx;
using BepInEx.Configuration;

namespace Haiku.DebugMod
{
    class Settings
    {
        public static readonly string debugPath = Paths.ConfigPath + "/Debug";

        public static ConfigEntry<KeyboardShortcut> Invuln;
        public static ConfigEntry<KeyboardShortcut> IgnoreHeat;
        public static ConfigEntry<KeyboardShortcut> ShowHitboxes;
        public static ConfigEntry<KeyboardShortcut> ShowStats;

        public static ConfigEntry<KeyboardShortcut> SaveState;
        public static ConfigEntry<KeyboardShortcut> LoadState;

        public static void initSettings(ConfigFile config)
        {
            Invuln = config.Bind("Cheats", "ToggleInvuln", new KeyboardShortcut(KeyCode.F2));
            IgnoreHeat = config.Bind("Cheats", "IgnoreHeat", new KeyboardShortcut(KeyCode.F3));
            ShowHitboxes = config.Bind("Cheats", "ShowHitboxes", new KeyboardShortcut(KeyCode.F4));
            ShowStats = config.Bind("Cheats", "ShowStats", new KeyboardShortcut(KeyCode.F5));
            SaveState = config.Bind("SaveStates", "SaveState", new KeyboardShortcut(KeyCode.F6));
            LoadState = config.Bind("SaveStates", "LoadState", new KeyboardShortcut(KeyCode.F7));
            config.Save();
        }
    }
}
