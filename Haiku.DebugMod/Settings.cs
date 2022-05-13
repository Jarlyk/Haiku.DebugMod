using UnityEngine;
using BepInEx;
using BepInEx.Configuration;
using System.Collections.Generic;
using System.Diagnostics;

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

        public static ConfigEntry<string> nameNextSave;

        public static ConfigEntry<string> url;

        #endregion
        #endregion

        public static void initSettings(ConfigFile config)
        {
            Invuln = config.Bind("Cheats", "ToggleInvuln", new KeyboardShortcut(KeyCode.F2), setPosition(4));
            IgnoreHeat = config.Bind("Cheats", "IgnoreHeat", new KeyboardShortcut(KeyCode.F3), setPosition(3));
            ShowHitboxes = config.Bind("Cheats", "ShowHitboxes", new KeyboardShortcut(KeyCode.F4), setPosition(2));
            ShowStats = config.Bind("Cheats", "ShowStats", new KeyboardShortcut(KeyCode.F5), setPosition(1));

            MemorySaveState = config.Bind("SaveStates", "SaveState", new KeyboardShortcut(KeyCode.F6), setPosition(6));
            MemoryLoadState = config.Bind("SaveStates", "LoadState", new KeyboardShortcut(KeyCode.F7), setPosition(5));

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
            PageNext = config.Bind("SaveStates", "NextPage", new KeyboardShortcut(KeyCode.F11), setPosition(2));
            PagePrevious = config.Bind("SaveStates", "PreviousPage", new KeyboardShortcut(KeyCode.F10), setPosition(3));
            showStates = config.Bind("SaveStates", "ShowStates", new KeyboardShortcut(KeyCode.F11), setPosition(4));
            nameNextSave = config.Bind(new ConfigDefinition("SaveStates","Name Next Save"), "Insert Name", setPosition(1));

            config.Bind("Website", "", "https://github.com/Jarlyk/Haiku.DebugMod", new ConfigDescription("Open Website", null, 
                new ConfigurationManagerAttributes { CustomDrawer = OpenWebsiteDrawer, ReadOnly = true, HideDefaultButton = true}));

            config.Save();
        }


        private static void OpenWebsiteDrawer(ConfigEntryBase entry)
        {
            if (GUILayout.Button(new GUIContent("URL", "https://github.com/Jarlyk/Haiku.DebugMod"), GUI.skin.button, GUILayout.ExpandWidth(false))) {
                Process.Start("https://github.com/Jarlyk/Haiku.DebugMod");
            }
        }

        private static ConfigDescription setPosition(int pos)
        {
            // Set the position relative to its category using ConfigurationManagerAttributes. Higher number is higher on the list
            return new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = pos });
        }
    }
}
