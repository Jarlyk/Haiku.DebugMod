using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Haiku.DebugMod.SaveStates {
    class SaveStatesManager {
        public static int currentPage = 0;

        public static bool isSaving;
        public static int saveSlot;

        public static bool isLoading;
        public static int loadSlot;

        public static bool showFiles = false;

        public static void previousPage()
        {
            currentPage = (currentPage - 1 + 10) % 10;
        }

        public static void nextPage()
        {
            currentPage = (currentPage + 1 + 10) % 10;
        }

        public static void SaveState(int slot = -1) {
            // Quick Save
            if (slot == -1)
            {
                SaveData.Save(Settings.debugPath + "/SaveState/saveData.haiku");
            } else
            {
                SaveData.Save(Settings.debugPath + $"/SaveState/{currentPage}/saveData{slot}.haiku",slot);
                SaveData.saveFileNames(Settings.debugPath + $"/SaveState/{currentPage}/fileNameList.haiku",slot);
            }
            saveSlot = slot;
            GameManager.instance.StartCoroutine(savingUI());
            MiniDebugUI.findFileNames();
        }

        public static void LoadState(int slot = -1) {
            // Quick Load
            if (slot == -1)
            {
                SaveData.Load(Settings.debugPath + "/SaveState/saveData.haiku");
            } else
            {
                loadSlot = slot;
                SaveData.Load(Settings.debugPath + $"/SaveState/{currentPage}/saveData{slot}.haiku");
            }
            if (!SaveData.localSaveData.TryGetValue("sceneToLoad", out int scene)) return;
            GameManager.instance.StartCoroutine(LoadScene(scene));
            loadSlot = slot;
            GameManager.instance.StartCoroutine(loadingUI());
        }

        private static IEnumerator savingUI()
        {
            isLoading = false;
            isSaving = true;
            yield return new WaitForSeconds(1f);
            isSaving = false;
        }

        private static IEnumerator loadingUI()
        {
            isSaving = false;
            isLoading = true;
            yield return new WaitForSeconds(1f);
            isLoading = false;
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
