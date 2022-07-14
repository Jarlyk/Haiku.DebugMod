using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Haiku.DebugMod.Warp
{
    public sealed class RepairStationWarpButton : MonoBehaviour, ISelectHandler
    {
        public RepairStationWarp owner;
        public int sceneId;
        public bool isHaikuWake;

        private void Start()
        {
            var button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);
        }
        public void OnSelect(BaseEventData eventData)
        {
            owner.UpdateCursor(this);
        }

        private void OnClick()
        {
            //We pretend the game was just loaded so that we load in from the save point
            GameManager.instance.gameLoaded = true;
            if (isHaikuWake)
            {
                Debug.Log($"Station Warp: Haiku Wake");
                GameManager.instance.introPlayed = false;
                SceneManager.LoadScene(10);
            }
            else
            {
                CameraBehavior.instance.TransitionState(true);
                Debug.Log($"Station Warp: Scene {sceneId}");
                SceneManager.LoadScene(sceneId);
            }

            CameraBehavior.instance.ResumeHideUI();
        }
    }
}
