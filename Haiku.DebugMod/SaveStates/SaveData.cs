using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Haiku.DebugMod.SaveStates {
    class SaveData {
        private static ES3File es3SaveFile;

        internal static Dictionary<string, int> localSaveData;
        internal static Vector2 lastPosition;

        internal static void Save(string filePath) {
            es3SaveFile = new ES3File(filePath);
            es3SaveFile.Save<int>("savePointSceneIndex", GameManager.instance.savePointSceneIndex);
            es3SaveFile.Save<int>("maxHealth", GameManager.instance.maxHealth);
            es3SaveFile.Save<int>("coolingPoints", GameManager.instance.coolingPoints);
            es3SaveFile.Save<int>("healthShards", GameManager.instance.healthShards);
            es3SaveFile.Save<int>("trainRoom", GameManager.instance.trainRoom);
            es3SaveFile.Save<float>("timePlayed", GameManager.instance.timePlayed);
            es3SaveFile.Save<int>("spareParts", GameManager.instance.spareParts);
            es3SaveFile.Save<int>("bankSpareParts", GameManager.instance.bankSpareParts);
            es3SaveFile.Save<int>("attackDamage", GameManager.instance.attackDamage);
            es3SaveFile.Save<bool>("canHeal", GameManager.instance.canHeal);
            es3SaveFile.Save<bool>("canBomb", GameManager.instance.canBomb);
            es3SaveFile.Save<bool>("canRoll", GameManager.instance.canRoll);
            es3SaveFile.Save<bool>("canTeleport", GameManager.instance.canTeleport);
            es3SaveFile.Save<bool>("canDoubleJump", GameManager.instance.canDoubleJump);
            es3SaveFile.Save<bool>("canWallJump", GameManager.instance.canWallJump);
            es3SaveFile.Save<bool>("canGrapple", GameManager.instance.canGrapple);
            es3SaveFile.Save<bool>("waterRes", GameManager.instance.waterRes);
            es3SaveFile.Save<bool>("fireRes", GameManager.instance.fireRes);
            es3SaveFile.Save<bool>("lightBulb", GameManager.instance.lightBulb);
            es3SaveFile.Save<bool>("g_Magnetism", GameManager.instance.g_Magnetism);
            es3SaveFile.Save<bool>("g_RollSpeed", GameManager.instance.g_RollSpeed);
            es3SaveFile.Save<bool>("g_MapHelper", GameManager.instance.g_MapHelper);
            es3SaveFile.Save<bool>("g_MoreMoney", GameManager.instance.g_MoreMoney);
            es3SaveFile.Save<bool>("g_AutoRoll", GameManager.instance.g_AutoRoll);
            es3SaveFile.Save<bool>("g_ExtraFireRes", GameManager.instance.g_ExtraFireRes);
            es3SaveFile.Save<bool>("r_CritChance", GameManager.instance.r_CritChance);
            es3SaveFile.Save<bool>("r_QuickAttack", GameManager.instance.r_QuickAttack);
            es3SaveFile.Save<bool>("r_ElectricTrail", GameManager.instance.r_ElectricTrail);
            es3SaveFile.Save<bool>("r_AttackRange", GameManager.instance.r_AttackRange);
            es3SaveFile.Save<bool>("r_BulbProjectile", GameManager.instance.r_BulbProjectile);
            es3SaveFile.Save<bool>("r_ShockWave", GameManager.instance.r_ShockWave);
            es3SaveFile.Save<bool>("r_ShockProjectile", GameManager.instance.r_ShockProjectile);
            es3SaveFile.Save<bool>("r_NoKnockback", GameManager.instance.r_NoKnockback);
            es3SaveFile.Save<bool>("r_IncreasedBombDamage", GameManager.instance.r_IncreasedBombDamage);
            es3SaveFile.Save<bool>("r_LessBombHeatCost", GameManager.instance.r_LessBombHeatCost);
            es3SaveFile.Save<bool>("r_ElectricOrbs1", GameManager.instance.r_ElectricOrbs1);
            es3SaveFile.Save<bool>("r_SharpDash", GameManager.instance.r_SharpDash);
            es3SaveFile.Save<bool>("b_IncreaseHealth1", GameManager.instance.b_IncreaseHealth1);
            es3SaveFile.Save<bool>("b_MoneyMagnet", GameManager.instance.b_MoneyMagnet);
            es3SaveFile.Save<bool>("b_AutoBomb", GameManager.instance.b_AutoBomb);
            es3SaveFile.Save<bool>("b_LongerInvincibility", GameManager.instance.b_LongerInvincibility);
            es3SaveFile.Save<bool>("b_ReduceMoneyDropped", GameManager.instance.b_ReduceMoneyDropped);
            es3SaveFile.Save<bool>("b_RollSaw", GameManager.instance.b_RollSaw);
            es3SaveFile.Save<bool>("b_BlinkDistance", GameManager.instance.b_BlinkDistance);
            es3SaveFile.Save<bool>("b_FastHeal", GameManager.instance.b_FastHeal);
            es3SaveFile.Save<bool>("g_FastCooldown", GameManager.instance.b_FastCooldown);
            for (int i = 0; i < GameManager.instance.chip.Length; i++) {
                es3SaveFile.Save<bool>("chipCollected" + i, GameManager.instance.chip[i].collected);
                es3SaveFile.Save<bool>("chipEquipped" + i, GameManager.instance.chip[i].equipped);
            }
            for (int j = 3; j < GameManager.instance.chipSlot.Length; j++) {
                es3SaveFile.Save<bool>("chipSlotCollect" + j, GameManager.instance.chipSlot[j].collected);
            }
            for (int k = 0; k < GameManager.instance.chipSlot.Length; k++) {
                es3SaveFile.Save<string>("chipSlotChipEquipped" + k, GameManager.instance.chipSlot[k].chipEquippedIdentifier);
            }
            for (int l = 0; l < GameManager.instance.bosses.Length; l++) {
                es3SaveFile.Save<bool>("bosses" + l, GameManager.instance.bosses[l].defeated);
            }
            es3SaveFile.Save<bool>("introPlayed", GameManager.instance.introPlayed);
            es3SaveFile.Save<bool>("verseSceneAfterVirusPlayed", GameManager.instance.verseSceneAfterVirusPlayed);
            es3SaveFile.Save<int>("savedChildren", GameManager.instance.savedChildren);
            es3SaveFile.Save<bool>("passedEngineDoor", GameManager.instance.passedEngineDoor);
            es3SaveFile.Save<bool>("openedVault", GameManager.instance.openedVault);
            es3SaveFile.Save<bool>("trainUnlocked", GameManager.instance.trainUnlocked);
            for (int m = 0; m < GameManager.instance.trainStations.Length; m++) {
                es3SaveFile.Save<bool>("trainStation" + m, GameManager.instance.trainStations[m].unlockedStation);
            }
            for (int n = 0; n < GameManager.instance.nPC.Length; n++) {
                es3SaveFile.Save<bool>("nPC" + n, GameManager.instance.nPC[n].interactedWith);
            }
            es3SaveFile.Save<bool>("quaternQuestComplete", GameManager.instance.quaternQuestComplete);
            es3SaveFile.Save<bool>("melodyQuestComplete", GameManager.instance.melodyQuestComplete);
            es3SaveFile.Save<bool>("welderQuestComplete", GameManager.instance.welderQuestComplete);
            es3SaveFile.Save<bool>("bohQuestComplete", GameManager.instance.bohQuestComplete);
            es3SaveFile.Save<float>("bohQuestCompleteTime", GameManager.instance.bohQuestCompleteTime);
            es3SaveFile.Save<bool>("luneFirstEncounter", GameManager.instance.luneFirstEncounter);
            for (int num = 0; num < GameManager.instance.haikus.Length; num++) {
                es3SaveFile.Save<bool>("haikus" + num, GameManager.instance.haikus[num].readHaikuPoem);
            }
            for (int num2 = 0; num2 < GameManager.instance.powerCells.Length; num2++) {
                es3SaveFile.Save<bool>("powercells" + num2, GameManager.instance.powerCells[num2].collected);
            }
            es3SaveFile.Save<bool>("firstEncounterWithQuatern", GameManager.instance.firstEncounterWithQuatern);
            es3SaveFile.Save<int>("lastPowercellCount", GameManager.instance.lastPowercellCount);
            for (int num3 = 0; num3 < GameManager.instance.areas.Length; num3++) {
                es3SaveFile.Save<bool>("areas" + num3, GameManager.instance.areas[num3].visited);
            }
            es3SaveFile.Save<int>("currentArea", GameManager.instance.currentArea);
            for (int num4 = 0; num4 < GameManager.instance.doors.Length; num4++) {
                es3SaveFile.Save<bool>("doors" + num4, GameManager.instance.doors[num4].opened);
            }
            for (int num5 = 0; num5 < GameManager.instance.bombableTiles.Length; num5++) {
                es3SaveFile.Save<bool>("bombableTiles" + num5, GameManager.instance.bombableTiles[num5].bombed);
            }
            for (int num6 = 0; num6 < GameManager.instance.moneyPiles.Length; num6++) {
                es3SaveFile.Save<bool>("moneyPiles" + num6, GameManager.instance.moneyPiles[num6].collected);
            }
            for (int num7 = 0; num7 < GameManager.instance.moneyMagnets.Length; num7++) {
                es3SaveFile.Save<float>("moneyMagnetsTimeStamp" + num7, GameManager.instance.moneyMagnets[num7].timeStamp);
                es3SaveFile.Save<int>("moneyMagnetsScene" + num7, GameManager.instance.moneyMagnets[num7].scene);
                es3SaveFile.Save<float>("moneyMagnetsWorldPosX" + num7, GameManager.instance.moneyMagnets[num7].worldPosition.x);
                es3SaveFile.Save<float>("moneyMagnetsWorldPosY" + num7, GameManager.instance.moneyMagnets[num7].worldPosition.y);
                es3SaveFile.Save<int>("moneyMagnetsMoneyHeld" + num7, GameManager.instance.moneyMagnets[num7].moneyHeld);
            }
            for (int num8 = 0; num8 < GameManager.instance.worldObjects.Length; num8++) {
                es3SaveFile.Save<bool>("worldObjects" + num8, GameManager.instance.worldObjects[num8].collected);
            }
            for (int num9 = 0; num9 < GameManager.instance.itemSlots.Length; num9++) {
                es3SaveFile.Save<bool>("itemSlot" + num9, GameManager.instance.itemSlots[num9].isFull);
                es3SaveFile.Save<int>("itemIDinSlot" + num9, GameManager.instance.itemSlots[num9].itemID);
                es3SaveFile.Save<int>("itemQuantityinSlot" + num9, GameManager.instance.itemSlots[num9].quantity);
            }
            for (int num10 = 0; num10 < GameManager.instance.mapTiles.Length; num10++) {
                es3SaveFile.Save<bool>("mapTiles" + num10, GameManager.instance.mapTiles[num10].explored);
            }
            for (int num11 = 0; num11 < GameManager.instance.disruptors.Length; num11++) {
                es3SaveFile.Save<bool>("disruptors" + num11, GameManager.instance.disruptors[num11].destroyed);
                es3SaveFile.Save<bool>("disruptorsAnimationViewed" + num11, GameManager.instance.disruptors[num11].animationViewed);
            }
            for (int num12 = 0; num12 < GameManager.instance.mapHelpers.Length; num12++) {
                es3SaveFile.Save<bool>("mapHelpersVisited" + num12, GameManager.instance.mapHelpers[num12].visited);
                es3SaveFile.Save<bool>("mapHelpersCompleted" + num12, GameManager.instance.mapHelpers[num12].completed);
            }
            es3SaveFile.Save<bool>("playedMapTutorial", GameManager.instance.playedMapTutorial);
            es3SaveFile.Save<bool>("showPowercells", GameManager.instance.showPowercells);
            es3SaveFile.Save<bool>("showHealthStations", GameManager.instance.showHealthStations);
            es3SaveFile.Save<bool>("showBankStations", GameManager.instance.showBankStations);
            es3SaveFile.Save<bool>("showVendors", GameManager.instance.showVendors);
            es3SaveFile.Save<bool>("showTrainStations", GameManager.instance.showTrainStations);
            es3SaveFile.Save<int>("sceneToLoad", SceneManager.GetActiveScene().buildIndex);
            es3SaveFile.Save<int>("savedHealth", PlayerHealth.instance.currentHealth);
            es3SaveFile.Save<Vector2>("lastPosition", PlayerScript.instance.lastPositionOnPlatform);
            es3SaveFile.Sync();
        }

        internal static void Load(string filePath) {
            es3SaveFile = new ES3File(filePath);
            localSaveData = new Dictionary<string, int>();
            GameManager.instance.savePointSceneIndex = es3SaveFile.Load<int>("savePointSceneIndex", 10);
            GameManager.instance.maxHealth = es3SaveFile.Load<int>("maxHealth", 4);
            GameManager.instance.coolingPoints = es3SaveFile.Load<int>("coolingPoints", 0);
            GameManager.instance.healthShards = es3SaveFile.Load<int>("healthShards", 0);
            GameManager.instance.trainRoom = es3SaveFile.Load<int>("trainRoom", 0);
            GameManager.instance.timePlayed = es3SaveFile.Load<float>("timePlayed", 0f);
            GameManager.instance.spareParts = es3SaveFile.Load<int>("spareParts", 0);
            GameManager.instance.bankSpareParts = es3SaveFile.Load<int>("bankSpareParts", 0);
            GameManager.instance.attackDamage = es3SaveFile.Load<int>("attackDamage", 3);
            GameManager.instance.canHeal = es3SaveFile.Load<bool>("canHeal", false);
            GameManager.instance.canBomb = es3SaveFile.Load<bool>("canBomb", false);
            GameManager.instance.canRoll = es3SaveFile.Load<bool>("canRoll", false);
            GameManager.instance.canTeleport = es3SaveFile.Load<bool>("canTeleport", false);
            GameManager.instance.canDoubleJump = es3SaveFile.Load<bool>("canDoubleJump", false);
            GameManager.instance.canWallJump = es3SaveFile.Load<bool>("canWallJump", false);
            GameManager.instance.canGrapple = es3SaveFile.Load<bool>("canGrapple", false);
            GameManager.instance.waterRes = es3SaveFile.Load<bool>("waterRes", false);
            GameManager.instance.fireRes = es3SaveFile.Load<bool>("fireRes", false);
            GameManager.instance.lightBulb = es3SaveFile.Load<bool>("lightBulb", false);
            GameManager.instance.g_Magnetism = es3SaveFile.Load<bool>("g_Magnetism", false);
            GameManager.instance.g_RollSpeed = es3SaveFile.Load<bool>("g_RollSpeed", false);
            GameManager.instance.g_MapHelper = es3SaveFile.Load<bool>("g_MapHelper", false);
            GameManager.instance.g_MoreMoney = es3SaveFile.Load<bool>("g_MoreMoney", false);
            GameManager.instance.g_AutoRoll = es3SaveFile.Load<bool>("g_AutoRoll", false);
            GameManager.instance.g_ExtraFireRes = es3SaveFile.Load<bool>("g_ExtraFireRes", false);
            GameManager.instance.r_CritChance = es3SaveFile.Load<bool>("r_CritChance", false);
            GameManager.instance.r_QuickAttack = es3SaveFile.Load<bool>("r_QuickAttack", false);
            GameManager.instance.r_ElectricTrail = es3SaveFile.Load<bool>("r_ElectricTrail", false);
            GameManager.instance.r_AttackRange = es3SaveFile.Load<bool>("r_AttackRange", false);
            GameManager.instance.r_BulbProjectile = es3SaveFile.Load<bool>("r_BulbProjectile", false);
            GameManager.instance.r_ShockWave = es3SaveFile.Load<bool>("r_ShockWave", false);
            GameManager.instance.r_ShockProjectile = es3SaveFile.Load<bool>("r_ShockProjectile", false);
            GameManager.instance.r_NoKnockback = es3SaveFile.Load<bool>("r_NoKnockback", false);
            GameManager.instance.r_IncreasedBombDamage = es3SaveFile.Load<bool>("r_IncreasedBombDamage", false);
            GameManager.instance.r_LessBombHeatCost = es3SaveFile.Load<bool>("r_LessBombHeatCost", false);
            GameManager.instance.r_ElectricOrbs1 = es3SaveFile.Load<bool>("r_ElectricOrbs1", false);
            GameManager.instance.r_SharpDash = es3SaveFile.Load<bool>("r_SharpDash", false);
            GameManager.instance.b_IncreaseHealth1 = es3SaveFile.Load<bool>("b_IncreaseHealth1", false);
            GameManager.instance.b_MoneyMagnet = es3SaveFile.Load<bool>("b_MoneyMagnet", false);
            GameManager.instance.b_AutoBomb = es3SaveFile.Load<bool>("b_AutoBomb", false);
            GameManager.instance.b_LongerInvincibility = es3SaveFile.Load<bool>("b_LongerInvincibility", false);
            GameManager.instance.b_ReduceMoneyDropped = es3SaveFile.Load<bool>("b_ReduceMoneyDropped", false);
            GameManager.instance.b_RollSaw = es3SaveFile.Load<bool>("b_RollSaw", false);
            GameManager.instance.b_BlinkDistance = es3SaveFile.Load<bool>("b_BlinkDistance", false);
            GameManager.instance.b_FastHeal = es3SaveFile.Load<bool>("b_FastHeal", false);
            GameManager.instance.b_FastCooldown = es3SaveFile.Load<bool>("g_FastCooldown", false);
            for (int i = 0; i < GameManager.instance.chip.Length; i++) {
                GameManager.instance.chip[i].collected = es3SaveFile.Load<bool>("chipCollected" + i, false);
                GameManager.instance.chip[i].equipped = es3SaveFile.Load<bool>("chipEquipped" + i, false);
            }
            for (int j = 3; j < GameManager.instance.chipSlot.Length; j++) {
                GameManager.instance.chipSlot[j].collected = es3SaveFile.Load<bool>("chipSlotCollect" + j, false);
            }
            for (int k = 0; k < GameManager.instance.chipSlot.Length; k++) {
                GameManager.instance.chipSlot[k].chipEquippedIdentifier = es3SaveFile.Load<string>("chipSlotChipEquipped" + k, "");
            }
            for (int l = 0; l < GameManager.instance.bosses.Length; l++) {
                GameManager.instance.bosses[l].defeated = es3SaveFile.Load<bool>("bosses" + l, false);
            }
            GameManager.instance.introPlayed = es3SaveFile.Load<bool>("introPlayed", false);
            GameManager.instance.verseSceneAfterVirusPlayed = es3SaveFile.Load<bool>("verseSceneAfterVirusPlayed", false);
            GameManager.instance.savedChildren = es3SaveFile.Load<int>("savedChildren", 0);
            GameManager.instance.passedEngineDoor = es3SaveFile.Load<bool>("passedEngineDoor", false);
            GameManager.instance.openedVault = es3SaveFile.Load<bool>("openedVault", false);
            GameManager.instance.trainUnlocked = es3SaveFile.Load<bool>("trainUnlocked", false);
            for (int m = 0; m < GameManager.instance.trainStations.Length; m++) {
                GameManager.instance.trainStations[m].unlockedStation = es3SaveFile.Load<bool>("trainStation" + m, false);
            }
            for (int n = 0; n < GameManager.instance.nPC.Length; n++) {
                GameManager.instance.nPC[n].interactedWith = es3SaveFile.Load<bool>("nPC" + n, false);
            }
            GameManager.instance.quaternQuestComplete = es3SaveFile.Load<bool>("quaternQuestComplete", false);
            GameManager.instance.melodyQuestComplete = es3SaveFile.Load<bool>("melodyQuestComplete", false);
            GameManager.instance.welderQuestComplete = es3SaveFile.Load<bool>("welderQuestComplete", false);
            GameManager.instance.bohQuestComplete = es3SaveFile.Load<bool>("bohQuestComplete", false);
            GameManager.instance.bohQuestCompleteTime = es3SaveFile.Load<float>("bohQuestCompleteTime", 0f);
            GameManager.instance.luneFirstEncounter = es3SaveFile.Load<bool>("luneFirstEncounter", false);
            for (int num = 0; num < GameManager.instance.haikus.Length; num++) {
                GameManager.instance.haikus[num].readHaikuPoem = es3SaveFile.Load<bool>("haikus" + num, false);
            }
            for (int num2 = 0; num2 < GameManager.instance.powerCells.Length; num2++) {
                GameManager.instance.powerCells[num2].collected = es3SaveFile.Load<bool>("powercells" + num2, false);
            }
            GameManager.instance.firstEncounterWithQuatern = es3SaveFile.Load<bool>("firstEncounterWithQuatern", false);
            GameManager.instance.lastPowercellCount = es3SaveFile.Load<int>("lastPowercellCount", -1);
            for (int num3 = 0; num3 < GameManager.instance.areas.Length; num3++) {
                GameManager.instance.areas[num3].visited = es3SaveFile.Load<bool>("areas" + num3, false);
            }
            GameManager.instance.currentArea = es3SaveFile.Load<int>("currentArea", 0);
            for (int num4 = 0; num4 < GameManager.instance.doors.Length; num4++) {
                GameManager.instance.doors[num4].opened = es3SaveFile.Load<bool>("doors" + num4, false);
            }
            for (int num5 = 0; num5 < GameManager.instance.bombableTiles.Length; num5++) {
                GameManager.instance.bombableTiles[num5].bombed = es3SaveFile.Load<bool>("bombableTiles" + num5, false);
            }
            for (int num6 = 0; num6 < GameManager.instance.moneyPiles.Length; num6++) {
                GameManager.instance.moneyPiles[num6].collected = es3SaveFile.Load<bool>("moneyPiles" + num6, false);
            }
            for (int num7 = 0; num7 < GameManager.instance.moneyMagnets.Length; num7++) {
                GameManager.instance.moneyMagnets[num7].timeStamp = es3SaveFile.Load<float>("moneyMagnetsTimeStamp" + num7, 0f);
                GameManager.instance.moneyMagnets[num7].scene = es3SaveFile.Load<int>("moneyMagnetsScene" + num7, 0);
                GameManager.instance.moneyMagnets[num7].worldPosition.x = es3SaveFile.Load<float>("moneyMagnetsWorldPosX" + num7, 0f);
                GameManager.instance.moneyMagnets[num7].worldPosition.y = es3SaveFile.Load<float>("moneyMagnetsWorldPosY" + num7, 0f);
                GameManager.instance.moneyMagnets[num7].moneyHeld = es3SaveFile.Load<int>("moneyMagnetsMoneyHeld" + num7, 0);
            }
            for (int num8 = 0; num8 < GameManager.instance.worldObjects.Length; num8++) {
                GameManager.instance.worldObjects[num8].collected = es3SaveFile.Load<bool>("worldObjects" + num8, false);
            }
            for (int num9 = 0; num9 < GameManager.instance.itemSlots.Length; num9++) {
                GameManager.instance.itemSlots[num9].isFull = es3SaveFile.Load<bool>("itemSlot" + num9, false);
                GameManager.instance.itemSlots[num9].itemID = es3SaveFile.Load<int>("itemIDinSlot" + num9, 0);
                GameManager.instance.itemSlots[num9].quantity = es3SaveFile.Load<int>("itemQuantityinSlot" + num9, 0);
            }
            for (int num10 = 0; num10 < GameManager.instance.mapTiles.Length; num10++) {
                GameManager.instance.mapTiles[num10].explored = es3SaveFile.Load<bool>("mapTiles" + num10, false);
            }
            for (int num11 = 0; num11 < GameManager.instance.disruptors.Length; num11++) {
                GameManager.instance.disruptors[num11].destroyed = es3SaveFile.Load<bool>("disruptors" + num11, false);
                GameManager.instance.disruptors[num11].animationViewed = es3SaveFile.Load<bool>("disruptorsAnimationViewed" + num11, false);
            }
            for (int num12 = 0; num12 < GameManager.instance.mapHelpers.Length; num12++) {
                GameManager.instance.mapHelpers[num12].visited = es3SaveFile.Load<bool>("mapHelpersVisited" + num12, false);
                GameManager.instance.mapHelpers[num12].completed = es3SaveFile.Load<bool>("mapHelpersCompleted" + num12, false);
            }
            GameManager.instance.playedMapTutorial = es3SaveFile.Load<bool>("playedMapTutorial", false);
            GameManager.instance.showPowercells = es3SaveFile.Load<bool>("showPowercells", false);
            GameManager.instance.showHealthStations = es3SaveFile.Load<bool>("showHealthStations", false);
            GameManager.instance.showBankStations = es3SaveFile.Load<bool>("showBankStations", false);
            GameManager.instance.showVendors = es3SaveFile.Load<bool>("showVendors", false);
            GameManager.instance.showTrainStations = es3SaveFile.Load<bool>("showTrainStations", false);
            localSaveData.Add("sceneToLoad", es3SaveFile.Load<int>("sceneToLoad", -1));
            localSaveData.Add("savedHealth", es3SaveFile.Load<int>("savedHealth", -1));
            lastPosition = es3SaveFile.Load<Vector2>("lastPosition", new Vector2());
        }
    }
}
