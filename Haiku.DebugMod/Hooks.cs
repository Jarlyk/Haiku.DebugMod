using System;
using System.Reflection;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Haiku.DebugMod {
    public static class Hooks {
        private const BindingFlags AllBindings = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
        private static GameObject[] MapRooms;
        public static Vector2 validStartPosition = new Vector2(10f,10f);
        private static bool isQuickMapOpened = false;
        private static GameObject MapWarpSelectOverlay = null;

        public static void Init()
        {
            #region Cheats
            IL.PlayerHealth.TakeDamage += UpdateTakeDamage;
            IL.PlayerHealth.StunAndTakeDamage += UpdateTakeDamage;
            IL.ManaManager.AddHeat += UpdateAddHeat;
            #endregion
            #region Quick Map Warp
            On.PlayerLocation.OnEnable += QuickMapEnabled;
            On.CameraBehavior.Update += cameraUpdate;
            On.CanvasAspectScaler.Update += MainCanvasUpdate;
            On.LoadNewLevel.Awake += TransitionToNextRoom;
            On.MapTile.CheckMyTile += MapTileCheck;
            #endregion
            #region Save enemies in Scene
            On.EnemyHealth.Start += EnemySpawned;
            On.LoadNewLevel.OnTriggerEnter2D += loadingNewScene;
            #endregion
            SaveStates.SaveData.initSaveStates();
        }

        private static bool MapTileCheck(On.MapTile.orig_CheckMyTile orig, MapTile self)
        {
            bool result = false;
            if (self.TryGetComponent<Image>(out Image img))
            {
                img.raycastTarget = true;
                if (GameManager.instance.mapTiles[self.tileID].explored)
                {
                    img.color = new Color(1f, 1f, 1f, 0f);
                    result = true;
                }
            }
            return result;
        }



        #region Hooks
        #region Cheats
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

        private static void UpdateAddHeat(ILContext il)
        {
            var c = new ILCursor(il);

            var skip = c.DefineLabel();
            c.EmitDelegate<Func<bool>>(MiniCheats.IsIgnoreHeat);
            c.Emit(OpCodes.Brfalse, skip);
            c.Emit(OpCodes.Ret);
            c.MarkLabel(skip);
        }
        #endregion
        #region Quick Map Warp
        private static void QuickMapEnabled(On.PlayerLocation.orig_OnEnable orig, PlayerLocation self)
        {
            // Enable Mouse after Quick Map is opened and save all Room GameObject
            orig(self);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            if (MapRooms != null) return;
            MapRooms = self.rooms;
        }

        private static void cameraUpdate(On.CameraBehavior.orig_Update orig, CameraBehavior self)
        {
            // Keep Cursor visible even if you click out of the window
            orig(self);
            if (self.mapUI.activeSelf)
            {
                isQuickMapOpened = true;
                if (Cursor.lockState != CursorLockMode.None && Cursor.visible != true)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
            }
            else
            {
                isQuickMapOpened = false;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        private static void MainCanvasUpdate(On.CanvasAspectScaler.orig_Update orig, CanvasAspectScaler self)
        {
            // Get the closest room whenever you click on the Map
            if (!isQuickMapOpened || !MiniCheats.QuickMapWarp) return;

            if (Input.GetMouseButtonDown(0))
            {
                PointerEventData ped = new PointerEventData(null);
                ped.position = Input.mousePosition;
                List<RaycastResult> results = new();
                self.gameObject.GetComponent<GraphicRaycaster>().Raycast(ped, results);
                GameObject MaskMapTile = null;
                foreach (RaycastResult result in results)
                {
                    Debug.Log(result.gameObject.name);
                    if (result.gameObject.name.StartsWith("Mask Map Tile"))
                    {
                        MaskMapTile = result.gameObject;
                    }
                }
                if (MaskMapTile != null)
                {
                    Image imgOfSelectedMapTile;
                    if (MapWarpSelectOverlay == null)
                    {
                        MapWarpSelectOverlay = new GameObject();
                        MapWarpSelectOverlay.transform.SetParent(MaskMapTile.transform.parent.transform);
                        MapWarpSelectOverlay.transform.localScale = new Vector2(1f, 1f);

                        imgOfSelectedMapTile = MapWarpSelectOverlay.AddComponent<Image>();
                        imgOfSelectedMapTile.color = new Color(imgOfSelectedMapTile.color.r, imgOfSelectedMapTile.color.g, imgOfSelectedMapTile.color.b, 0.4f);
                        Debug.Log("GameObect created" + MapWarpSelectOverlay + " parent: " + MapWarpSelectOverlay.transform.parent);
                    } else
                    {
                        imgOfSelectedMapTile = MapWarpSelectOverlay.GetComponent<Image>();
                    }
                    MapWarpSelectOverlay.transform.position = MaskMapTile.transform.position;
                    RectTransform MaskMapTileRect = MaskMapTile.GetComponent<RectTransform>();
                    imgOfSelectedMapTile.rectTransform.sizeDelta = new Vector2(MaskMapTileRect.rect.width, MaskMapTileRect.rect.height);
                }
                try
                {
                    GameObject temp = findClosestRoom(Input.mousePosition);
                    if (temp != null)
                    {
                        GameManager.instance.StartCoroutine(SaveStates.SaveStatesManager.LoadScene(temp.name));
                    }
                    else
                    {
                        Debug.LogError("Couldn't match Scene By Name: " + temp.name);
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
            }
            orig(self);
        }

        private static GameObject findClosestRoom(Vector2 mousePos)
        {
            // Goes through all rooms and finds the nearest to the Mouse Position within 300f range
            if (MapRooms == null) return null;
            GameObject closestRoom = null;
            float smallestDistance = 300f;
            foreach (GameObject room in MapRooms)
            {
                float distance = Vector2.Distance(mousePos, room.transform.position);
                if (distance < smallestDistance)
                {
                    closestRoom = room;
                    smallestDistance = distance;
                }
            }
            Debug.Log("s distance: " + smallestDistance);
            return closestRoom;
        }

        private static void TransitionToNextRoom(On.LoadNewLevel.orig_Awake orig, LoadNewLevel self)
        {
            // Get a "random" transition position we can Warp to
            validStartPosition = self.startPoint.position;
            orig(self);
        }
        #endregion

        #region Save enemies in Scene
        private static void EnemySpawned(On.EnemyHealth.orig_Start orig, EnemyHealth self)
        {
            // Grab the ID of every Enemy group in a Scene
            orig(self);
            SaveStates.SaveData.AddID(self.ID);
        }
        private static void loadingNewScene(On.LoadNewLevel.orig_OnTriggerEnter2D orig, LoadNewLevel self, object collision)
        {
            // When loading a Scene, clear the Hashset of Enemy IDs so that it only contains a list per Scene
            Collider2D collisionVar = (Collider2D)collision;
            if (collisionVar.gameObject.tag == "Player")
            {
                SaveStates.SaveData.clearEnemyIDHashset();
            }
            orig(self, collision);
        }
        #endregion
        #endregion

        public static void Update()
        {
            #region Cheats
            if (Settings.Invuln.Value.IsDown())
            {
                MiniCheats.Invuln = !MiniCheats.Invuln;
            }
            if (Settings.IgnoreHeat.Value.IsDown())
            {
                MiniCheats.IgnoreHeat = !MiniCheats.IgnoreHeat;
            }

            if (Settings.ShowHitboxes.Value.IsDown())
            {
                HitboxRendering.ShowHitboxes = !HitboxRendering.ShowHitboxes;
            }

            if (Settings.ShowStats.Value.IsDown())
            {
                MiniDebugUI.ShowStats = !MiniDebugUI.ShowStats;
            }
            #endregion

            #region SaveStates
            if (Settings.MemorySaveState.Value.IsDown())
            {
                SaveStates.SaveStatesManager.SaveState();
            }
            if (Settings.MemoryLoadState.Value.IsDown())
            {
                SaveStates.SaveStatesManager.LoadState();
            }

            foreach (KeyValuePair<int, ConfigEntry<KeyboardShortcut>> SaveStateKeys in Settings.SaveStateSlots)
            {
                if (SaveStateKeys.Value.Value.IsDown())
                {
                    SaveStates.SaveStatesManager.SaveState(SaveStateKeys.Key);
                }
            }
            foreach (KeyValuePair<int, ConfigEntry<KeyboardShortcut>> LoadStateKeys in Settings.LoadStateSlots)
            {
                if (LoadStateKeys.Value.Value.IsDown())
                {
                    SaveStates.SaveStatesManager.LoadState(LoadStateKeys.Key);
                }
            }

            if (Settings.showStates.Value.IsDown())
            {
                if (!SaveStates.SaveStatesManager.showFiles)
                {
                    MiniDebugUI.findFileNames();
                    SaveStates.SaveStatesManager.showFiles = !SaveStates.SaveStatesManager.showFiles;
                }
                else
                {
                    SaveStates.SaveStatesManager.showFiles = !SaveStates.SaveStatesManager.showFiles;
                }
            }

            if (Settings.PageNext.Value.IsDown())
            {
                SaveStates.SaveStatesManager.nextPage();
                if (SaveStates.SaveStatesManager.showFiles) MiniDebugUI.findFileNames();
            }
            if (Settings.PagePrevious.Value.IsDown())
            {
                SaveStates.SaveStatesManager.previousPage();
                if (SaveStates.SaveStatesManager.showFiles) MiniDebugUI.findFileNames();
            }
            #endregion

            //TODO: Zoom
            //CameraFollow
            //Savepoint warping
        }
    }
}
