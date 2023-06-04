using BepInEx;
using BepInEx.Configuration;
using Modding;
using System.Collections.Generic;
using UnityEngine;

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
        public static ConfigEntry<KeyboardShortcut> CameraFollow;
        public static ConfigEntry<KeyboardShortcut> CameraIncZoom;
        public static ConfigEntry<KeyboardShortcut> CameraDecZoom;
        // public static ConfigEntry<KeyboardShortcut> CameraResetZoom;
        public static ConfigEntry<bool> ShowCompletionDetails;
        public static ConfigEntry<bool> ShowBossInfo;
        public static ConfigEntry<bool> UnlimitedWarp;
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

        public static void InitSettings(ConfigFile config)
        {
            #region Cheats
            CameraFollow = config.Bind("Cheats", "CameraFollow", new KeyboardShortcut(KeyCode.H), ConfigManagerUtil.setPosition(8));
            CameraIncZoom = config.Bind("Cheats", "CameraIncreaseZoom", new KeyboardShortcut(KeyCode.PageUp), ConfigManagerUtil.setPosition(6));
            CameraDecZoom = config.Bind("Cheats", "CameraDecreaseZoom", new KeyboardShortcut(KeyCode.PageDown), ConfigManagerUtil.setPosition(5));
            // CameraResetZoom = config.Bind("Cheats", "CameraResetZoom", new KeyboardShortcut(KeyCode.G), ConfigManagerUtil.setPosition(7));

            Invuln = config.Bind("Cheats", "ToggleInvuln", new KeyboardShortcut(KeyCode.F2), ConfigManagerUtil.setPosition(4));
            IgnoreHeat = config.Bind("Cheats", "IgnoreHeat", new KeyboardShortcut(KeyCode.F3), ConfigManagerUtil.setPosition(3));
            ShowHitboxes = config.Bind("Cheats", "ShowHitboxes", new KeyboardShortcut(KeyCode.F4), ConfigManagerUtil.setPosition(2));
            ShowStats = config.Bind("Cheats", "ShowStats", new KeyboardShortcut(KeyCode.F5), ConfigManagerUtil.setPosition(1));
            ShowCompletionDetails = config.Bind("Cheats", "ShowCompletionDetails", true);
            ShowBossInfo = config.Bind("Cheats", "ShowBossInfo", true);
            ConfigManagerUtil.createButton(config, MiniCheats.GiveAllMaps, "Cheats", "GiveMaps", "Give all Maps");
            ConfigManagerUtil.createButton(config, MiniCheats.GiveAllChips, "Cheats", "GiveChips", "Give all Chips and Chip Slots");
            ConfigManagerUtil.createButton(config, MiniCheats.IncCoolingPoints, "Cheats", "IncrementCoolant", "Increment the amount of coolant by 1 (wraps around at max)");
            UnlimitedWarp = config.Bind("Cheats", "UnlimitedWarp", false,
                                        "Allow warping to save stations that have not yet been visited");
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
