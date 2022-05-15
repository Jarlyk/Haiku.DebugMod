using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace Haiku.DebugMod
{
    internal static class MapWarp
    {
        public static GameObject[] MapRooms = new GameObject[0];
        private static GameObject MapWarpSelectOverlay = null;
        public static void MoveSelectObject(GameObject canvas)
        {
            PointerEventData ped = new PointerEventData(null);
            ped.position = Input.mousePosition;
            List<RaycastResult> results = new();
            canvas.GetComponent<GraphicRaycaster>().Raycast(ped, results);
            GameObject MaskMapTile = null;
            foreach (RaycastResult result in results)
            {
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
                }
                else
                {
                    imgOfSelectedMapTile = MapWarpSelectOverlay.GetComponent<Image>();
                }
                MapWarpSelectOverlay.transform.position = MaskMapTile.transform.position;
                Debug.Log("Position of select" + MaskMapTile.transform.position);
                RectTransform MaskMapTileRect = MaskMapTile.GetComponent<RectTransform>();
                imgOfSelectedMapTile.rectTransform.sizeDelta = new Vector2(MaskMapTileRect.rect.width, MaskMapTileRect.rect.height);
            }
            Hooks.timer = 0f;
        }

        public static void LoadRoom()
        {
            try
            {
                GameObject temp = findClosestRoom(Input.mousePosition);
                if (temp != null)
                {
                    GameManager.instance.StartCoroutine(SaveStates.SaveStatesManager.LoadScene(temp.name));
                }
                else
                {
                    Debug.LogWarning("Room was too far away");
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error while attempting to find a room, error is: \n" + e);
            }
        }

        private static GameObject findClosestRoom(Vector2 mousePos)
        {
            // Goes through all rooms and finds the nearest to the Mouse Position within 300f** range
            if (MapRooms.Length == 0) return null;
            GameObject closestRoom = null;
            float smallestDistance = 300f * 300f;
            foreach (GameObject room in MapRooms)
            {
                float distance = calcDistSquared(mousePos,room.transform.position);
                if (distance < smallestDistance)
                {
                    closestRoom = room;
                    smallestDistance = distance;
                }
            }
            Debug.Log(closestRoom + " : " + closestRoom.transform.position);
            return closestRoom;
        }

        private static float calcDistSquared(Vector2 pos1, Vector2 pos2)
        {
            return (pos2.x - pos1.x) * (pos2.x - pos1.x) + (pos2.y - pos1.y) * (pos2.y - pos1.y);
        }
    }
}
