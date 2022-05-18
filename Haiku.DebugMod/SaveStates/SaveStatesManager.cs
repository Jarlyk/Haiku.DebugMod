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
                // Save everything to the current Page and slot selected, and then the Name currently selected in Settings.nameNextSave, otherwise SceneIndex and Name.
                SaveData.Save(Settings.debugPath + $"/SaveState/{currentPage}/saveData{slot}.haiku",slot);
                SaveData.saveFileNames(Settings.debugPath + $"/SaveState/{currentPage}/fileNameList.haiku",slot);
            }
            // Simple Saving UI
            saveSlot = slot;
            GameManager.instance.StartCoroutine(savingUI());
            // Update File Names
            DebugUI.findFileNames();
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
            if (SaveData.sceneToLoad == -1) return;
            GameManager.instance.StartCoroutine(LoadScene(SaveData.sceneToLoad, true));
            loadSlot = slot;
            GameManager.instance.StartCoroutine(loadingUI());
        }

        private static IEnumerator savingUI()
        {
            // This is not the cleanest solution, implementing a robust Timer is needed..
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

        public static IEnumerator LoadScene(int sceneIndex, bool SaveState) {
            // Code from Haiku
            PlayerScript.instance.DisableMovementFor(0.35f);
            CameraBehavior.instance.TransitionState(true);
            GameManager.instance.UpdateUpgradeAbilityBooleans();
            PlayerScript.instance.transform.position = new Vector3(1000f, 100f);
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
            while (!operation.isDone) {
                Mathf.Clamp01(operation.progress / 0.9f);
                yield return null;
            }
            CameraBehavior.instance.ResetCameraPositionToPlayer();
            yield return 5;
            yield return new WaitForSeconds(0.2f);
            CameraBehavior.instance.TransitionState(false);

            // Adjustment for SaveStates
            if (!SaveState) yield break;
            if (SaveData.lastPosition != new Vector2())
                PlayerScript.instance.transform.position = SaveData.lastPosition;
            yield break;
        }

        public static IEnumerator LoadScene(string sceneName)
        {
            // Code from Haiku
            PlayerScript.instance.DisableMovementFor(0.35f);
            CameraBehavior.instance.TransitionState(true);
            GameManager.instance.UpdateUpgradeAbilityBooleans();
            PlayerScript.instance.transform.position = new Vector3(1000f, 100f);
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            while (!operation.isDone)
            {
                Mathf.Clamp01(operation.progress / 0.9f);
                yield return null;
            }
            CameraBehavior.instance.ResetCameraPositionToPlayer();
            yield return 5;
            yield return new WaitForSeconds(0.2f);
            CameraBehavior.instance.TransitionState(false);

            // Changes for scene loading
            PlayerScript.instance.rb.velocity = Vector2.zero;
            PlayerScript.instance.transform.position = Hooks.validStartPosition;
        }
    }
}
