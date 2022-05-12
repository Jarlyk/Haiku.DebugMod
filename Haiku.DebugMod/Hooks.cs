using System;
using System.Reflection;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using UnityEngine;

namespace Haiku.DebugMod {
    public static class Hooks {
        private const BindingFlags AllBindings = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        public static void Init()
        {
            IL.PlayerHealth.TakeDamage += UpdateTakeDamage;
            IL.PlayerHealth.StunAndTakeDamage += UpdateTakeDamage;
            IL.ManaManager.AddHeat += UpdateAddHeat;
        }

        public static void Update() {
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

            if (Settings.SaveState.Value.IsDown())
            {
                SaveStates.SaveStatesManager.SaveState();
            }

            if (Settings.LoadState.Value.IsDown())
            {
                SaveStates.SaveStatesManager.LoadState();
            }

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
