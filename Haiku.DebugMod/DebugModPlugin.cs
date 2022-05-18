using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using BepInEx;
using BepInEx.Configuration;
using UnityEngine;

namespace Haiku.DebugMod
{
    [BepInPlugin("haiku.debugmod", "Haiku Debug Mod", "1.0.1.1")]
    [BepInDependency("haiku.mapi", "1.0")]
    public sealed class DebugModPlugin : BaseUnityPlugin
    {
        public void Awake()
        {
            // Use BaseUnityPlugin.Config so that ConfigManager works
            Settings.initSettings(Config);

            On.GameManager.Start += GameManagerStart;
            On.GameManager.Update += GameManagerUpdate;
        }

        private void GameManagerStart(On.GameManager.orig_Start orig, GameManager instance)
        {
            orig(instance);
            Hooks.Init();
            HitboxRendering.Init();

            //LogArray("powerCells");
            //LogArray("mapTiles");
            //LogArray("disruptors");
            //LogArray("chip");
            //LogArray("chipSlot");
            //LogArray("trainStations");

            instance.gameObject.AddComponent<DebugUI>();
        }

        private void GameManagerUpdate(On.GameManager.orig_Update orig, GameManager instance)
        {
            orig(instance);
            Hooks.Update();
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

