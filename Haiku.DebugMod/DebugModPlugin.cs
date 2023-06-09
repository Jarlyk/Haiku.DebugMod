using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using BepInEx;
using BepInEx.Configuration;
using Haiku.DebugMod.Warp;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Haiku.DebugMod
{
    [BepInPlugin("haiku.debugmod", "Haiku Debug Mod", "1.1.0.0")]
    [BepInDependency("haiku.mapi", "1.0.1.0")]
    public sealed class DebugModPlugin : BaseUnityPlugin
    {
        public void Awake()
        {
            // Use BaseUnityPlugin.Config so that ConfigManager works
            Settings.InitSettings(Config);

            HitboxRendering.Init();
            RepairStationWarp.InitHooks();
            QoL.InitHooks();
            AbilityToggling.InitHooks();

            On.PCSaveManager.Load += PCSaveManager_Load;
            On.PCSaveManager.Save += PCSaveManager_Save;

            gameObject.AddComponent<HitboxRendering>();
            gameObject.AddComponent<DebugUI>();
            gameObject.AddComponent<Hooks>();
        }

        private void PCSaveManager_Save(On.PCSaveManager.orig_Save orig, PCSaveManager self, string filepath)
        {
            orig(self, filepath);
            RepairStationWarp.SaveToFile(self.es3SaveFile);
            self.es3SaveFile.Sync();
        }

        private void PCSaveManager_Load(On.PCSaveManager.orig_Load orig, PCSaveManager self, string filepath)
        {
            orig(self, filepath);
            RepairStationWarp.LoadFromFile(self.es3SaveFile);
        }

        private void LogArray(string fieldName)
        {
            var instance = GameManager.instance;
            var builder = new StringBuilder();
            Type itemType = null;
            var items = (Array)typeof(GameManager).GetField(fieldName, BindingFlags.Instance | BindingFlags.Public).GetValue(instance);
            builder.AppendLine($"{fieldName}.Length == {items.Length}");
            if (items.Length > 0) {
                itemType = items.GetValue(0).GetType();
                var size = Marshal.SizeOf(itemType);
                builder.AppendLine($"sizeof({itemType.Name}) = {size}");
                var titleField = itemType.GetField("title", BindingFlags.Public | BindingFlags.Instance);

                if (titleField != null) {

                    for (int i = 0; i < items.Length; i++) {
                        var item = items.GetValue(i);
                        var title = titleField.GetValue(item) as string;
                
                        builder.AppendLine($"{fieldName}[{i}].title == {title}");
                    }
                }
            }
            Debug.LogWarning(builder.ToString());
        }
    }
}

