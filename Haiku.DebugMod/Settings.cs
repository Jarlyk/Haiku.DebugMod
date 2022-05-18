using UnityEngine;
using BepInEx;
using BepInEx.Configuration;
using System.Collections.Generic;
using Modding;

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


        public static Dictionary<int, ConfigEntry<KeyboardShortcut>> SaveStateSlots;

        public static Dictionary<int, ConfigEntry<KeyboardShortcut>> LoadStateSlots;

        public static ConfigEntry<KeyboardShortcut> PageNext;
        public static ConfigEntry<KeyboardShortcut> PagePrevious;

        public static ConfigEntry<KeyboardShortcut> showStates;

        public static ConfigEntry<string> nameNextSave;
        #endregion
        #endregion

        public static void initSettings(ConfigFile config)
        {
            #region Cheats
            Invuln = config.Bind("Cheats", "ToggleInvuln", new KeyboardShortcut(KeyCode.F2), ConfigManagerUtil.setPosition(4));
            IgnoreHeat = config.Bind("Cheats", "IgnoreHeat", new KeyboardShortcut(KeyCode.F3), ConfigManagerUtil.setPosition(3));
            ShowHitboxes = config.Bind("Cheats", "ShowHitboxes", new KeyboardShortcut(KeyCode.F4), ConfigManagerUtil.setPosition(2));
            ShowStats = config.Bind("Cheats", "ShowStats", new KeyboardShortcut(KeyCode.F5), ConfigManagerUtil.setPosition(1));
            ConfigManagerUtil.createButton(config, MiniCheats.giveAllMaps, "Cheats", "GiveMaps", "Give all Maps");
            #endregion

            #region SaveStates
            MemorySaveState = config.Bind("SaveStates", "SaveState", new KeyboardShortcut(KeyCode.F6), ConfigManagerUtil.setPosition(6));
            MemoryLoadState = config.Bind("SaveStates", "LoadState", new KeyboardShortcut(KeyCode.F7), ConfigManagerUtil.setPosition(5));

            SaveStateSlots = new Dictionary<int, ConfigEntry<KeyboardShortcut>>();
            for (int i = 0; i < 10; i++)
            {
                KeyCode defaultModifier = System.Enum.TryParse<KeyCode>(string.Format("Alpha{0}", i), out KeyCode parsedResult) ? parsedResult : KeyCode.None;
                SaveStateSlots.Add(i, config.Bind("SaveStates.SaveStateSlots", string.Format("SaveState{0}", i), new KeyboardShortcut(KeyCode.F8, defaultModifier)));
            }

            LoadStateSlots = new Dictionary<int, ConfigEntry<KeyboardShortcut>>();
            for (int i = 0; i < 10; i++)
            {
                KeyCode defaultModifier = System.Enum.TryParse<KeyCode>(string.Format("Alpha{0}", i), out KeyCode parsedResult) ? parsedResult : KeyCode.None;
                LoadStateSlots.Add(i, config.Bind("SaveStates.LoadStateSlots", string.Format("LoadState{0}", i), new KeyboardShortcut(KeyCode.F9, defaultModifier)));
            }
            PageNext = config.Bind("SaveStates", "NextPage", new KeyboardShortcut(KeyCode.F11), ConfigManagerUtil.setPosition(2));
            PagePrevious = config.Bind("SaveStates", "PreviousPage", new KeyboardShortcut(KeyCode.F10), ConfigManagerUtil.setPosition(3));
            showStates = config.Bind("SaveStates", "ShowStates", new KeyboardShortcut(KeyCode.F11), ConfigManagerUtil.setPosition(4));
            nameNextSave = config.Bind(new ConfigDefinition("SaveStates", "Name Next Save"), "Insert Name", ConfigManagerUtil.setPosition(1));
            #endregion
            ConfigManagerUtil.createWebsiteButton(config, "https://github.com/Jarlyk/Haiku.DebugMod");
            ConfigManagerUtil.createButton(config, MiniCheats.toggleQuickMap, "Cheats", "QuickMapWarp", "");
            config.Save();
        }
    }
}
