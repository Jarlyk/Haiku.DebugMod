using System;
using System.Reflection;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using BepInEx;
using BepInEx.Configuration;
using UnityEngine;

namespace Haiku.DebugMod {
    public static class Hooks {
        private const BindingFlags AllBindings = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        public static void Init()
        {
            IL.PlayerHealth.TakeDamage += UpdateTakeDamage;
            IL.PlayerHealth.StunAndTakeDamage += UpdateTakeDamage;
            IL.ManaManager.AddHeat += UpdateAddHeat;

            SaveStates.SaveData.initSaveStates();
        }

        public static void Update() {

            #region Cheats
            if (Settings.Invuln.Value.IsDown()) {
                MiniCheats.Invuln = !MiniCheats.Invuln;
            }
            if (Settings.IgnoreHeat.Value.IsDown()) {
                MiniCheats.IgnoreHeat = !MiniCheats.IgnoreHeat;
            }

            if (Settings.ShowHitboxes.Value.IsDown()) {
                HitboxRendering.ShowHitboxes = !HitboxRendering.ShowHitboxes;
            }

            if (Settings.ShowStats.Value.IsDown()) {
                MiniDebugUI.ShowStats = !MiniDebugUI.ShowStats;
            }
            #endregion

            #region SaveStates
            if (Settings.MemorySaveState.Value.IsDown())
            {
                SaveStates.SaveStatesManager.SaveState();
            }
            if (Settings.MemoryLoadState.Value.IsDown())
            {
                SaveStates.SaveStatesManager.LoadState();
            }

            foreach (KeyValuePair<int,ConfigEntry<KeyboardShortcut>> valuePair in Settings.SaveStateSlots)
            {
                if (valuePair.Value.Value.IsDown())
                {
                    SaveStates.SaveStatesManager.SaveState(valuePair.Key);
                }
            }

            foreach (KeyValuePair<int, ConfigEntry<KeyboardShortcut>> valuePair in Settings.LoadStateSlots)
            {
                if (valuePair.Value.Value.IsDown())
                {
                    SaveStates.SaveStatesManager.LoadState(valuePair.Key);
                }
            }

            if (Settings.showStates.Value.IsDown())
            {
                if (!SaveStates.SaveStatesManager.showFiles)
                {
                    MiniDebugUI.findFileNames();
                    SaveStates.SaveStatesManager.showFiles = !SaveStates.SaveStatesManager.showFiles;
                }
                else
                {
                    SaveStates.SaveStatesManager.showFiles = !SaveStates.SaveStatesManager.showFiles;
                }
            }

            if (Settings.PageNext.Value.IsDown()) {
                SaveStates.SaveStatesManager.nextPage();
                if (SaveStates.SaveStatesManager.showFiles) MiniDebugUI.findFileNames();
            }
            if (Settings.PagePrevious.Value.IsDown())
            {
                SaveStates.SaveStatesManager.previousPage();
                if (SaveStates.SaveStatesManager.showFiles) MiniDebugUI.findFileNames();
            }
            #endregion

            //TODO: Zoom
            //CameraFollow
            //Savepoint warping
        }

        private static void UpdateTakeDamage(ILContext il) {
            var c = new ILCursor(il);

            var currentHealth = typeof(PlayerHealth).GetField("currentHealth", AllBindings);
            c.GotoNext(x => x.MatchLdarg(0),
                       x => x.MatchLdarg(0),
                       x => x.MatchLdfld(currentHealth),
                       x => x.MatchLdarg(1),
                       x => x.MatchSub());
            var skip = c.DefineLabel();
            c.EmitDelegate<Func<bool>>(MiniCheats.IsInvuln);
            c.Emit(OpCodes.Brtrue, skip);
            c.GotoNext(MoveType.After, x => x.MatchStfld(currentHealth));
            c.MarkLabel(skip);
        }

        private static void UpdateAddHeat(ILContext il) {
            var c = new ILCursor(il);

            var skip = c.DefineLabel();
            c.EmitDelegate<Func<bool>>(MiniCheats.IsIgnoreHeat);
            c.Emit(OpCodes.Brfalse, skip);
            c.Emit(OpCodes.Ret);
            c.MarkLabel(skip);
        }
    }
}
