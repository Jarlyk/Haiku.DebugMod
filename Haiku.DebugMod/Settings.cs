using UnityEngine;
using BepInEx;
using BepInEx.Configuration;
using System.Collections.Generic;

namespace Haiku.DebugMod
{
    class Settings
    {
        // Path for local files
        public static readonly string debugPath = Paths.ConfigPath + "/Debug";

        #region ConfigEntries
        #region Cheats
        public static ConfigEntry<KeyboardShortcut> Invuln;
        public static ConfigEntry<KeyboardShortcut> IgnoreHeat;
        public static ConfigEntry<KeyboardShortcut> ShowHitboxes;
        public static ConfigEntry<KeyboardShortcut> ShowStats;
        #endregion

        #region SaveStates
        public static ConfigEntry<KeyboardShortcut> MemorySaveState;
        public static ConfigEntry<KeyboardShortcut> MemoryLoadState;


        public static Dictionary<int,ConfigEntry<KeyboardShortcut>> SaveStateSlots;

        public static Dictionary<int, ConfigEntry<KeyboardShortcut>> LoadStateSlots;

        public static ConfigEntry<KeyboardShortcut> PageNext;
        public static ConfigEntry<KeyboardShortcut> PagePrevious;

        public static ConfigEntry<KeyboardShortcut> showStates;

        #endregion
        #endregion

        public static void initSettings(ConfigFile config)
        {
            Invuln = config.Bind("Cheats", "ToggleInvuln", new KeyboardShortcut(KeyCode.F2));
            IgnoreHeat = config.Bind("Cheats", "IgnoreHeat", new KeyboardShortcut(KeyCode.F3));
            ShowHitboxes = config.Bind("Cheats", "ShowHitboxes", new KeyboardShortcut(KeyCode.F4));
            ShowStats = config.Bind("Cheats", "ShowStats", new KeyboardShortcut(KeyCode.F5));

            MemorySaveState = config.Bind("SaveStates", "SaveState", new KeyboardShortcut(KeyCode.F6));
            MemoryLoadState = config.Bind("SaveStates", "LoadState", new KeyboardShortcut(KeyCode.F7));

            SaveStateSlots = new Dictionary<int, ConfigEntry<KeyboardShortcut>>();
            for (int i = 0; i < 10; i++)
            {
                KeyCode defaultModifier = System.Enum.TryParse<KeyCode>(string.Format("Alpha{0}", i),out KeyCode parsedResult) ? parsedResult : KeyCode.None;
                SaveStateSlots.Add(i, config.Bind("SaveStates.SaveStateSlots", string.Format("SaveState{0}",i), new KeyboardShortcut(KeyCode.F8, defaultModifier)));
            }

            LoadStateSlots = new Dictionary<int, ConfigEntry<KeyboardShortcut>>();
            for (int i = 0; i < 10; i++)
            {
                KeyCode defaultModifier = System.Enum.TryParse<KeyCode>(string.Format("Alpha{0}", i), out KeyCode parsedResult) ? parsedResult : KeyCode.None;
                LoadStateSlots.Add(i, config.Bind("SaveStates.LoadStateSlots", string.Format("LoadState{0}", i), new KeyboardShortcut(KeyCode.F9, defaultModifier)));
            }

            PageNext = config.Bind("SaveStates", "NextPage", new KeyboardShortcut(KeyCode.F11));
            PagePrevious = config.Bind("SaveStates", "PreviousPage", new KeyboardShortcut(KeyCode.F10));
            showStates = config.Bind("SaveStates", "ShowStates", new KeyboardShortcut(KeyCode.F11));
            config.Save();
        }
    }
}
