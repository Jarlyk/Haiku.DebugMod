using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Haiku.DebugMod.SaveStates {
    class SaveData {
        private static ES3File _es3SaveFile;
        internal static Vector2 LastPosition;
        internal static int SceneToLoad;

        private static readonly string[] _fileNameData = new string[10];


        private static Dictionary<string, float> _enemyDeathTimes = new();

        public static void RegisterDeath(int enemyId, int sceneId, float deathTime)
        {
            _enemyDeathTimes[$"Enemy_{enemyId}{sceneId}"] = deathTime;
        }

        public static void ClearEnemyDeathTimes()
        {
            _enemyDeathTimes.Clear();
        }

        public static void InitSaveStates()
        {
            // Create non meaningful Save File Names to Display
            for (int i = 0; i < 10; i++)
            {
                string path = Settings.debugPath + $"/SaveState/{i}/fileNameList.haiku";
                if (File.Exists(path)) continue;
                _es3SaveFile = new ES3File(path);
                for (int j = 0; j < _fileNameData.Length; j++)
                {
                    _es3SaveFile.Save<string>($"fileName{j}", "No Save Data");
                }
                _es3SaveFile.Sync();
            }
        }

        public static void SaveFileNames(string filePath, int slot)
        {
            // Save the name of a SaveState to the right slot
            _es3SaveFile = new ES3File(filePath);
            if (Settings.nameNextSave.Value.Equals("Insert Name"))
            {
                _es3SaveFile.Save<string>($"fileName{slot}", _fileNameData[slot]);
            }
            else
            {
                _es3SaveFile.Save<string>($"fileName{slot}", Settings.nameNextSave.Value);
            }
            _es3SaveFile.Sync();
        }

        public static string[] LoadFileName(string filePath)
        {
            // Load all SaveState names from File for ShowStates UI
            if (!File.Exists(filePath))
            {
                Debug.LogWarning($"Couldn't find File at{filePath}");
                return CreateEmptyFileNames();
            }
            _es3SaveFile = new ES3File(filePath);
            string[] result = new string[_fileNameData.Length];
            for (int i = 0; i < _fileNameData.Length; i++)
            {
                result[i] = _es3SaveFile.Load<string>($"fileName{i}");
                _fileNameData[i] = result[i];
            }
            _es3SaveFile.Sync();
            return result;
        }

        private static string[] CreateEmptyFileNames()
        {
            // Fail save if settings were deleted
            string[] result = new string[_fileNameData.Length];
            for (int i = 0; i < _fileNameData.Length; i++)
            {
                result[i] = "No Save Data";
            }
            return result;
        }

        internal static void Save(string filePath, int slot = -1) {
            // Save everything to an ES3 File
            _es3SaveFile = new ES3File(filePath);
            var activeScene = SceneManager.GetActiveScene();
            string name = "No info";
            if (activeScene.IsValid())
            {
                name = $"Scene {activeScene.buildIndex} {activeScene.name}";
            }
            if (slot != -1)
            {
                _fileNameData[slot] = name;
            }
            _es3SaveFile.Save<float>("timePlayed", GameManager.instance.timePlayed);
            _es3SaveFile.Save<Dictionary<string, float>>(nameof(_enemyDeathTimes), _enemyDeathTimes);

            _es3SaveFile.Save<int>("savePointSceneIndex", GameManager.instance.savePointSceneIndex);
            _es3SaveFile.Save<int>("maxHealth", GameManager.instance.maxHealth);
            _es3SaveFile.Save<int>("coolingPoints", GameManager.instance.coolingPoints);
            _es3SaveFile.Save<int>("healthShards", GameManager.instance.healthShards);
            _es3SaveFile.Save<int>("trainRoom", GameManager.instance.trainRoom);
            _es3SaveFile.Save<float>("timePlayed", GameManager.instance.timePlayed);
            _es3SaveFile.Save<int>("spareParts", GameManager.instance.spareParts);
            _es3SaveFile.Save<int>("bankSpareParts", GameManager.instance.bankSpareParts);
            _es3SaveFile.Save<int>("attackDamage", GameManager.instance.attackDamage);
            _es3SaveFile.Save<bool>("canHeal", GameManager.instance.canHeal);
            _es3SaveFile.Save<bool>("canBomb", GameManager.instance.canBomb);
            _es3SaveFile.Save<bool>("canRoll", GameManager.instance.canRoll);
            _es3SaveFile.Save<bool>("canTeleport", GameManager.instance.canTeleport);
            _es3SaveFile.Save<bool>("canDoubleJump", GameManager.instance.canDoubleJump);
            _es3SaveFile.Save<bool>("canWallJump", GameManager.instance.canWallJump);
            _es3SaveFile.Save<bool>("canGrapple", GameManager.instance.canGrapple);
            _es3SaveFile.Save<bool>("waterRes", GameManager.instance.waterRes);
            _es3SaveFile.Save<bool>("fireRes", GameManager.instance.fireRes);
            _es3SaveFile.Save<bool>("lightBulb", GameManager.instance.lightBulb);
            _es3SaveFile.Save<bool>("g_Magnetism", GameManager.instance.g_Magnetism);
            _es3SaveFile.Save<bool>("g_RollSpeed", GameManager.instance.g_RollSpeed);
            _es3SaveFile.Save<bool>("g_MapHelper", GameManager.instance.g_MapHelper);
            _es3SaveFile.Save<bool>("g_MoreMoney", GameManager.instance.g_MoreMoney);
            _es3SaveFile.Save<bool>("g_AutoRoll", GameManager.instance.g_AutoRoll);
            _es3SaveFile.Save<bool>("g_ExtraFireRes", GameManager.instance.g_ExtraFireRes);
            _es3SaveFile.Save<bool>("r_CritChance", GameManager.instance.r_CritChance);
            _es3SaveFile.Save<bool>("r_QuickAttack", GameManager.instance.r_QuickAttack);
            _es3SaveFile.Save<bool>("r_ElectricTrail", GameManager.instance.r_ElectricTrail);
            _es3SaveFile.Save<bool>("r_AttackRange", GameManager.instance.r_AttackRange);
            _es3SaveFile.Save<bool>("r_BulbProjectile", GameManager.instance.r_BulbProjectile);
            _es3SaveFile.Save<bool>("r_ShockWave", GameManager.instance.r_ShockWave);
            _es3SaveFile.Save<bool>("r_ShockProjectile", GameManager.instance.r_ShockProjectile);
            _es3SaveFile.Save<bool>("r_NoKnockback", GameManager.instance.r_NoKnockback);
            _es3SaveFile.Save<bool>("r_IncreasedBombDamage", GameManager.instance.r_IncreasedBombDamage);
            _es3SaveFile.Save<bool>("r_LessBombHeatCost", GameManager.instance.r_LessBombHeatCost);
            _es3SaveFile.Save<bool>("r_ElectricOrbs1", GameManager.instance.r_ElectricOrbs1);
            _es3SaveFile.Save<bool>("r_SharpDash", GameManager.instance.r_SharpDash);
            _es3SaveFile.Save<bool>("b_IncreaseHealth1", GameManager.instance.b_IncreaseHealth1);
            _es3SaveFile.Save<bool>("b_MoneyMagnet", GameManager.instance.b_MoneyMagnet);
            _es3SaveFile.Save<bool>("b_AutoBomb", GameManager.instance.b_AutoBomb);
            _es3SaveFile.Save<bool>("b_LongerInvincibility", GameManager.instance.b_LongerInvincibility);
            _es3SaveFile.Save<bool>("b_ReduceMoneyDropped", GameManager.instance.b_ReduceMoneyDropped);
            _es3SaveFile.Save<bool>("b_RollSaw", GameManager.instance.b_RollSaw);
            _es3SaveFile.Save<bool>("b_BlinkDistance", GameManager.instance.b_BlinkDistance);
            _es3SaveFile.Save<bool>("b_FastHeal", GameManager.instance.b_FastHeal);
            _es3SaveFile.Save<bool>("g_FastCooldown", GameManager.instance.b_FastCooldown);
            for (int i = 0; i < GameManager.instance.chip.Length; i++) {
                _es3SaveFile.Save<bool>("chipCollected" + i, GameManager.instance.chip[i].collected);
                _es3SaveFile.Save<bool>("chipEquipped" + i, GameManager.instance.chip[i].equipped);
            }
            for (int j = 3; j < GameManager.instance.chipSlot.Length; j++) {
                _es3SaveFile.Save<bool>("chipSlotCollect" + j, GameManager.instance.chipSlot[j].collected);
            }
            for (int k = 0; k < GameManager.instance.chipSlot.Length; k++) {
                _es3SaveFile.Save<string>("chipSlotChipEquipped" + k, GameManager.instance.chipSlot[k].chipEquippedIdentifier);
            }
            for (int l = 0; l < GameManager.instance.bosses.Length; l++) {
                _es3SaveFile.Save<bool>("bosses" + l, GameManager.instance.bosses[l].defeated);
            }
            _es3SaveFile.Save<bool>("introPlayed", GameManager.instance.introPlayed);
            _es3SaveFile.Save<bool>("verseSceneAfterVirusPlayed", GameManager.instance.verseSceneAfterVirusPlayed);
            _es3SaveFile.Save<int>("savedChildren", GameManager.instance.savedChildren);
            _es3SaveFile.Save<bool>("passedEngineDoor", GameManager.instance.passedEngineDoor);
            _es3SaveFile.Save<bool>("openedVault", GameManager.instance.openedVault);
            _es3SaveFile.Save<bool>("trainUnlocked", GameManager.instance.trainUnlocked);
            for (int m = 0; m < GameManager.instance.trainStations.Length; m++) {
                _es3SaveFile.Save<bool>("trainStation" + m, GameManager.instance.trainStations[m].unlockedStation);
            }
            for (int n = 0; n < GameManager.instance.nPC.Length; n++) {
                _es3SaveFile.Save<bool>("nPC" + n, GameManager.instance.nPC[n].interactedWith);
            }
            _es3SaveFile.Save<bool>("quaternQuestComplete", GameManager.instance.quaternQuestComplete);
            _es3SaveFile.Save<bool>("melodyQuestComplete", GameManager.instance.melodyQuestComplete);
            _es3SaveFile.Save<bool>("welderQuestComplete", GameManager.instance.welderQuestComplete);
            _es3SaveFile.Save<bool>("bohQuestComplete", GameManager.instance.bohQuestComplete);
            _es3SaveFile.Save<float>("bohQuestCompleteTime", GameManager.instance.bohQuestCompleteTime);
            _es3SaveFile.Save<bool>("luneFirstEncounter", GameManager.instance.luneFirstEncounter);
            for (int num = 0; num < GameManager.instance.haikus.Length; num++) {
                _es3SaveFile.Save<bool>("haikus" + num, GameManager.instance.haikus[num].readHaikuPoem);
            }
            for (int num2 = 0; num2 < GameManager.instance.powerCells.Length; num2++) {
                _es3SaveFile.Save<bool>("powercells" + num2, GameManager.instance.powerCells[num2].collected);
            }
            _es3SaveFile.Save<bool>("firstEncounterWithQuatern", GameManager.instance.firstEncounterWithQuatern);
            _es3SaveFile.Save<int>("lastPowercellCount", GameManager.instance.lastPowercellCount);
            for (int num3 = 0; num3 < GameManager.instance.areas.Length; num3++) {
                _es3SaveFile.Save<bool>("areas" + num3, GameManager.instance.areas[num3].visited);
            }
            _es3SaveFile.Save<int>("currentArea", GameManager.instance.currentArea);
            for (int num4 = 0; num4 < GameManager.instance.doors.Length; num4++) {
                _es3SaveFile.Save<bool>("doors" + num4, GameManager.instance.doors[num4].opened);
            }
            for (int num5 = 0; num5 < GameManager.instance.bombableTiles.Length; num5++) {
                _es3SaveFile.Save<bool>("bombableTiles" + num5, GameManager.instance.bombableTiles[num5].bombed);
            }
            for (int num6 = 0; num6 < GameManager.instance.moneyPiles.Length; num6++) {
                _es3SaveFile.Save<bool>("moneyPiles" + num6, GameManager.instance.moneyPiles[num6].collected);
            }
            for (int num7 = 0; num7 < GameManager.instance.moneyMagnets.Length; num7++) {
                _es3SaveFile.Save<float>("moneyMagnetsTimeStamp" + num7, GameManager.instance.moneyMagnets[num7].timeStamp);
                _es3SaveFile.Save<int>("moneyMagnetsScene" + num7, GameManager.instance.moneyMagnets[num7].scene);
                _es3SaveFile.Save<float>("moneyMagnetsWorldPosX" + num7, GameManager.instance.moneyMagnets[num7].worldPosition.x);
                _es3SaveFile.Save<float>("moneyMagnetsWorldPosY" + num7, GameManager.instance.moneyMagnets[num7].worldPosition.y);
                _es3SaveFile.Save<int>("moneyMagnetsMoneyHeld" + num7, GameManager.instance.moneyMagnets[num7].moneyHeld);
            }
            for (int num8 = 0; num8 < GameManager.instance.worldObjects.Length; num8++) {
                _es3SaveFile.Save<bool>("worldObjects" + num8, GameManager.instance.worldObjects[num8].collected);
            }
            for (int num9 = 0; num9 < GameManager.instance.itemSlots.Length; num9++) {
                _es3SaveFile.Save<bool>("itemSlot" + num9, GameManager.instance.itemSlots[num9].isFull);
                _es3SaveFile.Save<int>("itemIDinSlot" + num9, GameManager.instance.itemSlots[num9].itemID);
                _es3SaveFile.Save<int>("itemQuantityinSlot" + num9, GameManager.instance.itemSlots[num9].quantity);
            }
            for (int num10 = 0; num10 < GameManager.instance.mapTiles.Length; num10++) {
                _es3SaveFile.Save<bool>("mapTiles" + num10, GameManager.instance.mapTiles[num10].explored);
            }
            for (int num11 = 0; num11 < GameManager.instance.disruptors.Length; num11++) {
                _es3SaveFile.Save<bool>("disruptors" + num11, GameManager.instance.disruptors[num11].destroyed);
                _es3SaveFile.Save<bool>("disruptorsAnimationViewed" + num11, GameManager.instance.disruptors[num11].animationViewed);
            }
            for (int num12 = 0; num12 < GameManager.instance.mapHelpers.Length; num12++) {
                _es3SaveFile.Save<bool>("mapHelpersVisited" + num12, GameManager.instance.mapHelpers[num12].visited);
                _es3SaveFile.Save<bool>("mapHelpersCompleted" + num12, GameManager.instance.mapHelpers[num12].completed);
            }
            _es3SaveFile.Save<bool>("playedMapTutorial", GameManager.instance.playedMapTutorial);
            _es3SaveFile.Save<bool>("showPowercells", GameManager.instance.showPowercells);
            _es3SaveFile.Save<bool>("showHealthStations", GameManager.instance.showHealthStations);
            _es3SaveFile.Save<bool>("showBankStations", GameManager.instance.showBankStations);
            _es3SaveFile.Save<bool>("showVendors", GameManager.instance.showVendors);
            _es3SaveFile.Save<bool>("showTrainStations", GameManager.instance.showTrainStations);

            _es3SaveFile.Save<int>("sceneToLoad", SceneManager.GetActiveScene().buildIndex);
            _es3SaveFile.Save<int>("savedHealth", PlayerHealth.instance.currentHealth);
            _es3SaveFile.Save<Vector2>("lastPosition", PlayerScript.instance.lastPositionOnPlatform);
            _es3SaveFile.Save<float>("heatStatus", ManaManager.instance.currentRotation);
            _es3SaveFile.Save<bool>("canChangeChips", PlayerScript.instance.canChangeChips);
            _es3SaveFile.Save<float>("isInvunerableTimer", PlayerScript.instance.isInvunerableTimer);
            _es3SaveFile.Sync();
        }

        internal static void Load(string filePath) {
            // Load everything from an ES3 Savefile
            _es3SaveFile = new ES3File(filePath);
            GameManager.instance.savePointSceneIndex = _es3SaveFile.Load<int>("savePointSceneIndex", 10);
            GameManager.instance.maxHealth = _es3SaveFile.Load<int>("maxHealth", 4);
            GameManager.instance.coolingPoints = _es3SaveFile.Load<int>("coolingPoints", 0);
            GameManager.instance.healthShards = _es3SaveFile.Load<int>("healthShards", 0);
            GameManager.instance.trainRoom = _es3SaveFile.Load<int>("trainRoom", 0);
            GameManager.instance.timePlayed = _es3SaveFile.Load<float>("timePlayed", 0f);
            GameManager.instance.spareParts = _es3SaveFile.Load<int>("spareParts", 0);
            GameManager.instance.bankSpareParts = _es3SaveFile.Load<int>("bankSpareParts", 0);
            GameManager.instance.attackDamage = _es3SaveFile.Load<int>("attackDamage", 3);
            GameManager.instance.canHeal = _es3SaveFile.Load<bool>("canHeal", false);
            GameManager.instance.canBomb = _es3SaveFile.Load<bool>("canBomb", false);
            GameManager.instance.canRoll = _es3SaveFile.Load<bool>("canRoll", false);
            GameManager.instance.canTeleport = _es3SaveFile.Load<bool>("canTeleport", false);
            GameManager.instance.canDoubleJump = _es3SaveFile.Load<bool>("canDoubleJump", false);
            GameManager.instance.canWallJump = _es3SaveFile.Load<bool>("canWallJump", false);
            GameManager.instance.canGrapple = _es3SaveFile.Load<bool>("canGrapple", false);
            GameManager.instance.waterRes = _es3SaveFile.Load<bool>("waterRes", false);
            GameManager.instance.fireRes = _es3SaveFile.Load<bool>("fireRes", false);
            GameManager.instance.lightBulb = _es3SaveFile.Load<bool>("lightBulb", false);
            GameManager.instance.g_Magnetism = _es3SaveFile.Load<bool>("g_Magnetism", false);
            GameManager.instance.g_RollSpeed = _es3SaveFile.Load<bool>("g_RollSpeed", false);
            GameManager.instance.g_MapHelper = _es3SaveFile.Load<bool>("g_MapHelper", false);
            GameManager.instance.g_MoreMoney = _es3SaveFile.Load<bool>("g_MoreMoney", false);
            GameManager.instance.g_AutoRoll = _es3SaveFile.Load<bool>("g_AutoRoll", false);
            GameManager.instance.g_ExtraFireRes = _es3SaveFile.Load<bool>("g_ExtraFireRes", false);
            GameManager.instance.r_CritChance = _es3SaveFile.Load<bool>("r_CritChance", false);
            GameManager.instance.r_QuickAttack = _es3SaveFile.Load<bool>("r_QuickAttack", false);
            GameManager.instance.r_ElectricTrail = _es3SaveFile.Load<bool>("r_ElectricTrail", false);
            GameManager.instance.r_AttackRange = _es3SaveFile.Load<bool>("r_AttackRange", false);
            GameManager.instance.r_BulbProjectile = _es3SaveFile.Load<bool>("r_BulbProjectile", false);
            GameManager.instance.r_ShockWave = _es3SaveFile.Load<bool>("r_ShockWave", false);
            GameManager.instance.r_ShockProjectile = _es3SaveFile.Load<bool>("r_ShockProjectile", false);
            GameManager.instance.r_NoKnockback = _es3SaveFile.Load<bool>("r_NoKnockback", false);
            GameManager.instance.r_IncreasedBombDamage = _es3SaveFile.Load<bool>("r_IncreasedBombDamage", false);
            GameManager.instance.r_LessBombHeatCost = _es3SaveFile.Load<bool>("r_LessBombHeatCost", false);
            GameManager.instance.r_ElectricOrbs1 = _es3SaveFile.Load<bool>("r_ElectricOrbs1", false);
            GameManager.instance.r_SharpDash = _es3SaveFile.Load<bool>("r_SharpDash", false);
            GameManager.instance.b_IncreaseHealth1 = _es3SaveFile.Load<bool>("b_IncreaseHealth1", false);
            GameManager.instance.b_MoneyMagnet = _es3SaveFile.Load<bool>("b_MoneyMagnet", false);
            GameManager.instance.b_AutoBomb = _es3SaveFile.Load<bool>("b_AutoBomb", false);
            GameManager.instance.b_LongerInvincibility = _es3SaveFile.Load<bool>("b_LongerInvincibility", false);
            GameManager.instance.b_ReduceMoneyDropped = _es3SaveFile.Load<bool>("b_ReduceMoneyDropped", false);
            GameManager.instance.b_RollSaw = _es3SaveFile.Load<bool>("b_RollSaw", false);
            GameManager.instance.b_BlinkDistance = _es3SaveFile.Load<bool>("b_BlinkDistance", false);
            GameManager.instance.b_FastHeal = _es3SaveFile.Load<bool>("b_FastHeal", false);
            GameManager.instance.b_FastCooldown = _es3SaveFile.Load<bool>("g_FastCooldown", false);
            for (int i = 0; i < GameManager.instance.chip.Length; i++) {
                GameManager.instance.chip[i].collected = _es3SaveFile.Load<bool>("chipCollected" + i, false);
                GameManager.instance.chip[i].equipped = _es3SaveFile.Load<bool>("chipEquipped" + i, false);
            }
            for (int j = 3; j < GameManager.instance.chipSlot.Length; j++) {
                GameManager.instance.chipSlot[j].collected = _es3SaveFile.Load<bool>("chipSlotCollect" + j, false);
            }
            for (int k = 0; k < GameManager.instance.chipSlot.Length; k++) {
                GameManager.instance.chipSlot[k].chipEquippedIdentifier = _es3SaveFile.Load<string>("chipSlotChipEquipped" + k, "");
            }
            for (int l = 0; l < GameManager.instance.bosses.Length; l++) {
                GameManager.instance.bosses[l].defeated = _es3SaveFile.Load<bool>("bosses" + l, false);
            }
            GameManager.instance.introPlayed = _es3SaveFile.Load<bool>("introPlayed", false);
            GameManager.instance.verseSceneAfterVirusPlayed = _es3SaveFile.Load<bool>("verseSceneAfterVirusPlayed", false);
            GameManager.instance.savedChildren = _es3SaveFile.Load<int>("savedChildren", 0);
            GameManager.instance.passedEngineDoor = _es3SaveFile.Load<bool>("passedEngineDoor", false);
            GameManager.instance.openedVault = _es3SaveFile.Load<bool>("openedVault", false);
            GameManager.instance.trainUnlocked = _es3SaveFile.Load<bool>("trainUnlocked", false);
            for (int m = 0; m < GameManager.instance.trainStations.Length; m++) {
                GameManager.instance.trainStations[m].unlockedStation = _es3SaveFile.Load<bool>("trainStation" + m, false);
            }
            for (int n = 0; n < GameManager.instance.nPC.Length; n++) {
                GameManager.instance.nPC[n].interactedWith = _es3SaveFile.Load<bool>("nPC" + n, false);
            }
            GameManager.instance.quaternQuestComplete = _es3SaveFile.Load<bool>("quaternQuestComplete", false);
            GameManager.instance.melodyQuestComplete = _es3SaveFile.Load<bool>("melodyQuestComplete", false);
            GameManager.instance.welderQuestComplete = _es3SaveFile.Load<bool>("welderQuestComplete", false);
            GameManager.instance.bohQuestComplete = _es3SaveFile.Load<bool>("bohQuestComplete", false);
            GameManager.instance.bohQuestCompleteTime = _es3SaveFile.Load<float>("bohQuestCompleteTime", 0f);
            GameManager.instance.luneFirstEncounter = _es3SaveFile.Load<bool>("luneFirstEncounter", false);
            for (int num = 0; num < GameManager.instance.haikus.Length; num++) {
                GameManager.instance.haikus[num].readHaikuPoem = _es3SaveFile.Load<bool>("haikus" + num, false);
            }
            for (int num2 = 0; num2 < GameManager.instance.powerCells.Length; num2++) {
                GameManager.instance.powerCells[num2].collected = _es3SaveFile.Load<bool>("powercells" + num2, false);
            }
            GameManager.instance.firstEncounterWithQuatern = _es3SaveFile.Load<bool>("firstEncounterWithQuatern", false);
            GameManager.instance.lastPowercellCount = _es3SaveFile.Load<int>("lastPowercellCount", -1);
            for (int num3 = 0; num3 < GameManager.instance.areas.Length; num3++) {
                GameManager.instance.areas[num3].visited = _es3SaveFile.Load<bool>("areas" + num3, false);
            }
            GameManager.instance.currentArea = _es3SaveFile.Load<int>("currentArea", 0);
            for (int num4 = 0; num4 < GameManager.instance.doors.Length; num4++) {
                GameManager.instance.doors[num4].opened = _es3SaveFile.Load<bool>("doors" + num4, false);
            }
            for (int num5 = 0; num5 < GameManager.instance.bombableTiles.Length; num5++) {
                GameManager.instance.bombableTiles[num5].bombed = _es3SaveFile.Load<bool>("bombableTiles" + num5, false);
            }
            for (int num6 = 0; num6 < GameManager.instance.moneyPiles.Length; num6++) {
                GameManager.instance.moneyPiles[num6].collected = _es3SaveFile.Load<bool>("moneyPiles" + num6, false);
            }
            for (int num7 = 0; num7 < GameManager.instance.moneyMagnets.Length; num7++) {
                GameManager.instance.moneyMagnets[num7].timeStamp = _es3SaveFile.Load<float>("moneyMagnetsTimeStamp" + num7, 0f);
                GameManager.instance.moneyMagnets[num7].scene = _es3SaveFile.Load<int>("moneyMagnetsScene" + num7, 0);
                GameManager.instance.moneyMagnets[num7].worldPosition.x = _es3SaveFile.Load<float>("moneyMagnetsWorldPosX" + num7, 0f);
                GameManager.instance.moneyMagnets[num7].worldPosition.y = _es3SaveFile.Load<float>("moneyMagnetsWorldPosY" + num7, 0f);
                GameManager.instance.moneyMagnets[num7].moneyHeld = _es3SaveFile.Load<int>("moneyMagnetsMoneyHeld" + num7, 0);
            }
            for (int num8 = 0; num8 < GameManager.instance.worldObjects.Length; num8++) {
                GameManager.instance.worldObjects[num8].collected = _es3SaveFile.Load<bool>("worldObjects" + num8, false);
            }
            for (int num9 = 0; num9 < GameManager.instance.itemSlots.Length; num9++) {
                GameManager.instance.itemSlots[num9].isFull = _es3SaveFile.Load<bool>("itemSlot" + num9, false);
                GameManager.instance.itemSlots[num9].itemID = _es3SaveFile.Load<int>("itemIDinSlot" + num9, 0);
                GameManager.instance.itemSlots[num9].quantity = _es3SaveFile.Load<int>("itemQuantityinSlot" + num9, 0);
            }
            for (int num10 = 0; num10 < GameManager.instance.mapTiles.Length; num10++) {
                GameManager.instance.mapTiles[num10].explored = _es3SaveFile.Load<bool>("mapTiles" + num10, false);
            }
            for (int num11 = 0; num11 < GameManager.instance.disruptors.Length; num11++) {
                GameManager.instance.disruptors[num11].destroyed = _es3SaveFile.Load<bool>("disruptors" + num11, false);
                GameManager.instance.disruptors[num11].animationViewed = _es3SaveFile.Load<bool>("disruptorsAnimationViewed" + num11, false);
            }
            for (int num12 = 0; num12 < GameManager.instance.mapHelpers.Length; num12++) {
                GameManager.instance.mapHelpers[num12].visited = _es3SaveFile.Load<bool>("mapHelpersVisited" + num12, false);
                GameManager.instance.mapHelpers[num12].completed = _es3SaveFile.Load<bool>("mapHelpersCompleted" + num12, false);
            }
            GameManager.instance.playedMapTutorial = _es3SaveFile.Load<bool>("playedMapTutorial", false);
            GameManager.instance.showPowercells = _es3SaveFile.Load<bool>("showPowercells", false);
            GameManager.instance.showHealthStations = _es3SaveFile.Load<bool>("showHealthStations", false);
            GameManager.instance.showBankStations = _es3SaveFile.Load<bool>("showBankStations", false);
            GameManager.instance.showVendors = _es3SaveFile.Load<bool>("showVendors", false);
            GameManager.instance.showTrainStations = _es3SaveFile.Load<bool>("showTrainStations", false);
            SceneToLoad = _es3SaveFile.Load<int>("sceneToLoad", -1);
            int savedHealth = _es3SaveFile.Load<int>("savedHealth", -1);
            if (savedHealth - PlayerHealth.instance.currentHealth != 0)
            {
                PlayerHealth.instance.AddHealth(savedHealth - PlayerHealth.instance.currentHealth);
            }
            LastPosition = _es3SaveFile.Load<Vector2>("lastPosition", new Vector2());
            ManaManager.instance.currentRotation = _es3SaveFile.Load<float>("heatStatus", 405f);
            PlayerScript.instance.canChangeChips = _es3SaveFile.Load<bool>("canChangeChips", false);
            PlayerScript.instance.isInvunerableTimer = _es3SaveFile.Load<float>("isInvunerableTimer", 0f);

            GameManager.instance.timePlayed = _es3SaveFile.Load<float>("timePlayed", GameManager.instance.timePlayed);

            PlayerPrefs.DeleteAll();
            _enemyDeathTimes = _es3SaveFile.Load<Dictionary<string, float>>(nameof(_enemyDeathTimes), new());
            foreach (KeyValuePair<string, float> kvp in _enemyDeathTimes)
            {
                PlayerPrefs.SetFloat(kvp.Key, kvp.Value);
            }
        }
    }
}
