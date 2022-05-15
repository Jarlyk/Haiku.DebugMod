using System.Linq;
using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Haiku.DebugMod {
    public sealed class MiniDebugUI : MonoBehaviour {
        public static bool ShowStats;
        private static string[] fileNames = new string[10];

        private void Update()
        {
            // Time.deltaTime doesn't work..??
            Hooks.timer += 0.02f;
        }

        public static void findFileNames()
        {
            fileNames = SaveStates.SaveData.loadFileName(Settings.debugPath + $"/SaveState/{SaveStates.SaveStatesManager.currentPage}/fileNameList.haiku");
            if (fileNames == null) return;
        }

        private void OnGUI()
        {
            GUI.Label(new Rect(5, 5, 100, 30), $"Debug");

            #region Cheats
            if (MiniCheats.IgnoreHeat)
            {
                GUI.Label(new Rect(105, 5, 50, 30), "NoHeat");
            }

            if (MiniCheats.Invuln)
            {
                GUI.Label(new Rect(155, 5, 50, 30), "Invuln");
            }

            if (ShowStats)
            {
                var gm = GameManager.instance;
                if (!gm) return;

                var tileCount = gm.mapTiles.Count(t => t.explored);
                var chipCount = gm.chip.Count(c => c.collected);
                var bossCount = gm.bosses.Count(b => b.defeated);
                var cellCount = gm.powerCells.Count(c => c.collected);
                var disruptCount = gm.disruptors.Count(d => d.destroyed);
                var slotCount = gm.chipSlot.Count(s => s.collected);
                var stationCount = gm.trainStations.Count(s => s.unlockedStation);

                int abilityCount = 0;
                if (gm.canBomb)
                {
                    abilityCount++;
                }
                if (gm.canRoll)
                {
                    abilityCount++;
                }
                if (gm.canWallJump)
                {
                    abilityCount++;
                }
                if (gm.canDoubleJump)
                {
                    abilityCount++;
                }
                if (gm.canGrapple)
                {
                    abilityCount++;
                }
                if (gm.canTeleport)
                {
                    abilityCount++;
                }
                if (gm.waterRes)
                {
                    abilityCount++;
                }
                if (gm.fireRes)
                {
                    abilityCount++;
                }
                if (gm.lightBulb)
                {
                    abilityCount++;
                }

                var completePercent = 85f * (tileCount + chipCount + cellCount + disruptCount + slotCount + stationCount + abilityCount + gm.maxHealth + gm.coolingPoints) /
                                      (gm.mapTiles.Length + gm.chip.Length + gm.powerCells.Length + gm.disruptors.Length +
                                       gm.chipSlot.Length + gm.trainStations.Length + 9 + 8 + 3);
                completePercent += bossCount;

                int y0 = 300;
                GUI.Label(new Rect(5, y0, 200, 20), $"Map Tiles {tileCount}/{gm.mapTiles.Length}");
                GUI.Label(new Rect(5, y0 + 20, 200, 20), $"Disruptors {disruptCount}/{gm.disruptors.Length}");
                GUI.Label(new Rect(5, y0 + 40, 200, 20), $"Chips {chipCount}/{gm.chip.Length}");
                GUI.Label(new Rect(5, y0 + 60, 200, 20), $"Chip Slots {slotCount}/{gm.chipSlot.Length}");
                GUI.Label(new Rect(5, y0 + 80, 200, 20), $"Power Cells {cellCount}/{gm.powerCells.Length}");
                GUI.Label(new Rect(5, y0 + 100, 200, 20), $"Bosses {bossCount}/{gm.bosses.Length}");
                GUI.Label(new Rect(5, y0 + 120, 200, 20), $"Stations {stationCount}/{gm.trainStations.Length}");
                GUI.Label(new Rect(5, y0 + 140, 200, 20), $"Coolant {gm.coolingPoints}/3");
                GUI.Label(new Rect(5, y0 + 160, 200, 20), $"Health {gm.maxHealth}/8");
                GUI.Label(new Rect(5, y0 + 180, 200, 20), $"Abilities {abilityCount}/9");
                GUI.Label(new Rect(5, y0 + 200, 200, 20), $"Completion {completePercent:0.00}%");


                var player = PlayerScript.instance;
                if (player)
                {
                    GUI.Label(new Rect(5, y0 + 240, 200, 20), $"Invuln {player.isInvunerableTimer:0.00}s");
                }

                var activeScene = SceneManager.GetActiveScene();
                if (activeScene.IsValid())
                {
                    GUI.Label(new Rect(5, y0 + 260, 200, 20), $"Scene# {activeScene.buildIndex} : {activeScene.name}");
                }
                GUI.Label(new Rect(5, y0 + 280,200,20), $"Player Position: {PlayerScript.instance.transform.position.x} : {PlayerScript.instance.transform.position.y}");

            }

            if (Event.current.type.Equals(EventType.Repaint))
            {
                HitboxRendering.Render();
            }
            #endregion

            #region SaveStateUI
            GUIStyle saveStateStyle = GUIStyle.none;
            saveStateStyle.fontSize = 30;
            saveStateStyle.normal.textColor = Color.white;
            if (SaveStates.SaveStatesManager.isSaving)
            {
                if (SaveStates.SaveStatesManager.saveSlot == -1)
                {
                    GUI.Label(new Rect(Screen.width - 350, Screen.height - 100, 100, 50), "Saved to Quick Slot", saveStateStyle);
                }
                else
                {
                    GUI.Label(new Rect(Screen.width - 350, Screen.height - 100, 100, 50), string.Format("Saved to Slot {0}", SaveStates.SaveStatesManager.saveSlot), saveStateStyle);
                }
            }
            if (SaveStates.SaveStatesManager.isLoading)
            {
                if (SaveStates.SaveStatesManager.saveSlot == -1)
                {
                    GUI.Label(new Rect(Screen.width - 350, Screen.height - 100, 100, 50), "Loaded Quick Slot", saveStateStyle);
                }
                else
                {
                    GUI.Label(new Rect(Screen.width - 350, Screen.height - 100, 100, 50), string.Format("Loaded Slot {0}", SaveStates.SaveStatesManager.loadSlot), saveStateStyle);
                }
            }
            if (SaveStates.SaveStatesManager.showFiles)
            {
                GUI.Label(new Rect(5, 600, 200, 20), "Current Page: " + SaveStates.SaveStatesManager.currentPage.ToString());
                for (int i = 1; i < fileNames.Length; i++) 
                {
                    GUI.Label(new Rect(5, 620 + i * 20, 200, 20), $"{i}: " + fileNames[i]);
                }
                GUI.Label(new Rect(5, 620 + fileNames.Length * 20, 200, 20), $"{0}: " + fileNames[0]);
            }
            #endregion
        }
    }
}
