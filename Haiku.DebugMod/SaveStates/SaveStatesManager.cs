using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Haiku.DebugMod.SaveStates {
    class SaveStatesManager {
        public static readonly string debugPath = Application.persistentDataPath + "/Debug";

        public static void SaveState() {
            SaveData.Save(debugPath + "/SaveState/saveData.haiku");
        }

        public static void LoadState() {
            SaveData.Load(debugPath + "/SaveState/saveData.haiku");
            if (!SaveData.localSaveData.TryGetValue("sceneToLoad", out int scene)) return;
            GameManager.instance.StartCoroutine(LoadScene(scene));
        }

        private static IEnumerator LoadScene(int sceneIndex) {
            // Code from Haiku
            PlayerScript.instance.DisableMovementFor(0.35f);
            CameraBehavior.instance.TransitionState(true);
            GameManager.instance.UpdateUpgradeAbilityBooleans();
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
            while (!operation.isDone) {
                Mathf.Clamp01(operation.progress / 0.9f);
                yield return null;
            }
            CameraBehavior.instance.TransitionState(false);

            // Adjustments for SaveStates
            if (SaveData.localSaveData["savedHealth"] - PlayerHealth.instance.currentHealth != 0) {
                PlayerHealth.instance.AddHealth(SaveData.localSaveData["savedHealth"] - PlayerHealth.instance.currentHealth);
            }
            if (SaveData.lastPosition != new Vector2())
                PlayerScript.instance.transform.position = SaveData.lastPosition;
            yield break;
        }
    }
}
