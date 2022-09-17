using System.Linq;
using System.IO;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using Modding;
using UnityEngine.UI;

namespace Haiku.DebugMod {
    public sealed class DebugUI : MonoBehaviour {
        public static bool ShowStats = false;
        private static string[] fileNames = new string[10];
        GameObject DebugCanvas;
        GameObject CheatsPanel;
        GameObject ShowStatsGameObject;
        GameObject ShowStatesGameObject;
        GameObject DisplayLoadingSavingGameObject;
        Text CheatsText;
        Text ShowStatsText;
        Text ShowStatesText;
        Text DisplayLoadingSavingText;

        void Start()
        {
            DebugCanvas = CanvasUtil.CreateCanvas(1);
            DebugCanvas.name = "DebugCanvas";
            DebugCanvas.transform.SetParent(gameObject.transform);

            GameObject DebugPanel = CanvasUtil.CreateBasePanel(DebugCanvas, 
                new CanvasUtil.RectData(new Vector2(0, 0), new Vector2(10, -4), new Vector2(0, 0), new Vector2(1, 2)));
            DebugPanel.name = "DebugPanel";

            #region Cheats
            CheatsPanel = CanvasUtil.CreateTextPanel(DebugPanel, "NoHeat", 5, TextAnchor.MiddleLeft, 
                new CanvasUtil.RectData(new Vector2(400, 10), new Vector2(0, 0)),CanvasUtil.GameFont);
            CheatsPanel.name = "CheatsPanel";
            CheatsText = CheatsPanel.GetComponent<Text>();
            #endregion

            ShowStatsGameObject = CanvasUtil.CreateTextPanel(DebugPanel, "", 4, TextAnchor.MiddleLeft,
                new CanvasUtil.RectData(new Vector2(400, 300), new Vector2(0, -90)), CanvasUtil.GameFont);
            ShowStatsGameObject.name = "ShowStatsGameObject";
            ShowStatsGameObject.SetActive(false);
            ShowStatsText = ShowStatsGameObject.GetComponent<Text>();

            #region SaveStates
            ShowStatesGameObject = CanvasUtil.CreateTextPanel(DebugPanel, "", 4, TextAnchor.MiddleLeft,
                new CanvasUtil.RectData(new Vector2(400, 300), new Vector2(0, -175)), CanvasUtil.GameFont);
            ShowStatesGameObject.name = "ShowStatesGameObject";
            ShowStatesGameObject.SetActive(false);
            ShowStatesText = ShowStatesGameObject.GetComponent<Text>();
            
            DisplayLoadingSavingGameObject = CanvasUtil.CreateTextPanel(DebugPanel, "", 10, TextAnchor.MiddleLeft,
                new CanvasUtil.RectData(new Vector2(400, 300), new Vector2(280, -200)), CanvasUtil.GameFont);
            DisplayLoadingSavingGameObject.name = "DisplayLoadingSavingGameObject";
            DisplayLoadingSavingGameObject.SetActive(false);
            DisplayLoadingSavingText = DisplayLoadingSavingGameObject.GetComponent<Text>();
            #endregion
        }

        public static void findFileNames()
        {
            fileNames = SaveStates.SaveData.LoadFileName(Settings.debugPath + $"/SaveState/{SaveStates.SaveStatesManager.currentPage}/fileNameList.haiku");
            if (fileNames == null) return;
        }

        void Update()
        {
            Hooks.timer += 0.02f;
            var cameraBehavior = CameraBehavior.instance;

            #region Cheats
            string activeCheats = "";
            if (MiniCheats.IgnoreHeat)
            {
                activeCheats += "No Heat ";
            }
            if (MiniCheats.Invuln)
            {
                activeCheats += "Invuln ";
            }
            if (MiniCheats.CameraFollow && cameraBehavior)
            {
                activeCheats += $"CamFollow: {cameraBehavior.cameraObject.orthographicSize} ";
            }
            CheatsText.text = activeCheats;

            ShowStatsGameObject.SetActive(ShowStats);

            if (ShowStats)
            {
                DoShowStats();
            }

            #endregion

            #region SaveStateUI
            bool displayLoadingSaving = SaveStates.SaveStatesManager.isSaving || SaveStates.SaveStatesManager.isLoading;
            DisplayLoadingSavingGameObject.SetActive(displayLoadingSaving);
            if (SaveStates.SaveStatesManager.isSaving)
            {
                if (SaveStates.SaveStatesManager.saveSlot == -1)
                {
                    DisplayLoadingSavingText.text = "Saved to Quick Slot";
                }
                else
                {
                    DisplayLoadingSavingText.text = $"Saved to Slot {SaveStates.SaveStatesManager.saveSlot}";
                }
            }
            if (SaveStates.SaveStatesManager.isLoading)
            {
                if (SaveStates.SaveStatesManager.saveSlot == -1)
                {
                    DisplayLoadingSavingText.text = "Loaded Quick Slot";
                }
                else
                {
                    DisplayLoadingSavingText.text = $"Loaded Slot {SaveStates.SaveStatesManager.loadSlot}";
                }
            }

            ShowStatesGameObject.SetActive(SaveStates.SaveStatesManager.showFiles);
            if (SaveStates.SaveStatesManager.showFiles)
            {
                string states = "Current Page: " + SaveStates.SaveStatesManager.currentPage.ToString();
                for (int i = 1; i < fileNames.Length; i++) 
                {
                    states += $"\n{i}: {fileNames[i]}";
                }
                states += $"\n0: {fileNames[0]}";
                ShowStatesText.text = states;
            }
            #endregion
        }

        private void DoShowStats()
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

            var completePercent = 85f*(tileCount + chipCount + cellCount + disruptCount + slotCount + stationCount +
                                       abilityCount + gm.maxHealth + gm.coolingPoints)/
                                  (gm.mapTiles.Length + gm.chip.Length + gm.powerCells.Length + gm.disruptors.Length +
                                   gm.chipSlot.Length + gm.trainStations.Length + 9 + 8 + 3);
            completePercent += bossCount;

            var builder = new StringBuilder();
            if (Settings.ShowCompletionDetails.Value)
            {
                builder.Append($"Map Tiles {tileCount}/{gm.mapTiles.Length}" + "\n" +
                               $"Disruptors {disruptCount}/{gm.disruptors.Length}" + "\n" +
                               $"Chips {chipCount}/{gm.chip.Length}" + "\n" +
                               $"Chip Slots {slotCount}/{gm.chipSlot.Length}" + "\n" +
                               $"Power Cells {cellCount}/{gm.powerCells.Length}" + "\n" +
                               $"Bosses {bossCount}/{gm.bosses.Length}" + "\n" +
                               $"Stations {stationCount}/{gm.trainStations.Length}" + "\n" +
                               $"Coolant {gm.coolingPoints}/3" + "\n" +
                               $"Health {gm.maxHealth}/8" + "\n" +
                               $"Abilities {abilityCount}/9");
            }

            builder.Append($"\nCompletion {completePercent:0.00}%");
            var player = PlayerScript.instance;
            if (player)
            {
                builder.Append($"\nInvuln {player.isInvunerableTimer:0.00}s");
            }

            var activeScene = SceneManager.GetActiveScene();
            if (activeScene.IsValid())
            {
                builder.Append($"\nScene# {activeScene.buildIndex} : {activeScene.name}");
            }

            if (player)
            {
                builder.Append($"\nPlayer Position: {player.transform.position.x} : {player.transform.position.y}");
            }

            if (Settings.ShowBossInfo.Value)
            {
                int sceneId = SceneManager.GetActiveScene().buildIndex;
                if (sceneId == 201)
                {
                    var boss = FindObjectOfType<BunkerSentient>();
                    if (boss)
                    {
                        builder.Append($"\nNeutron State: {boss.state}");
                        builder.Append($"\nNeutron HP: {boss.currentHealth} / {boss.phaseTwoHealth}");
                        var phaseText = (boss.phase2 ? "2" : "1");
                        builder.Append($"\nNeutron Phase {phaseText}");
                        if (boss.bounceHits > 0)
                        {
                            builder.Append($"\nNeutron Bounces: {boss.bounceHits}/{boss.maxBounceHits}");
                        }
                    }
                }
                else if (sceneId == 19)
                {
                    var boss = FindObjectOfType<SwingingGarbageMagnet>();
                    if (boss)
                    {
                        builder.Append("\nGarbage Magnet State: ");
                        if (boss.phaseTwo) builder.Append("phase2 ");
                        if (boss.magnetDive) builder.Append("magnetDive ");
                        if (boss.isSwinging) builder.Append("isSwinging ");
                        if (boss.isMovingTowards) builder.Append("isMovingTowards ");
                        if (boss.dead) builder.Append("dead");
                        builder.Append($"\nGM HP: {boss.currentHealth} / {boss.health}");
                        builder.Append($"\nGM Timer: {boss.timer}");
                    }
                }
                else if (sceneId == 84)
                {
                    var boss = FindObjectOfType<ElectricSentientRework>();
                    if (boss)
                    {
                        builder.Append("\nElectron State: ");
                        if (boss.phaseTwo) builder.Append("phase2 ");
                        if (boss.canMove) builder.Append("canMove ");
                        if (boss.followPlayer) builder.Append("followPlayer");
                        builder.Append($"\nElectron HP: {boss.currentHealth} / {boss.health}");
                    }
                }
                else if (sceneId == 144)
                {
                    var boss = FindObjectOfType<FightManager>();
                    builder.Append($"\nProton hp: {boss.fHealth}");
                    builder.Append($"\nNeutron hp: {boss.bHealth}");
                    builder.Append($"\nElectron hp: {boss.eHealth}");
                }
            }

            ShowStatsText.GetComponent<Text>().text = builder.ToString();
        }
    }
}
