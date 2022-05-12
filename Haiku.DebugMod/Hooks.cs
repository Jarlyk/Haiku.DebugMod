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
            if (Input.GetKeyDown(KeyCode.F2)) {
                MiniCheats.Invuln = !MiniCheats.Invuln;
            }
            if (Input.GetKeyDown(KeyCode.F3)) {
                MiniCheats.IgnoreHeat = !MiniCheats.IgnoreHeat;
            }

            if (Input.GetKeyDown(KeyCode.F4)) {
                HitboxRendering.ShowHitboxes = !HitboxRendering.ShowHitboxes;
            }

            if (Input.GetKeyDown(KeyCode.F5)) {
                MiniDebugUI.ShowStats = !MiniDebugUI.ShowStats;
            }

            if (Input.GetKeyDown(KeyCode.F6))
            {
                SaveStates.SaveStatesManager.SaveState();
            }

            if (Input.GetKeyDown(KeyCode.F7))
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
