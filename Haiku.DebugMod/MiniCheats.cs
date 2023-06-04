namespace Haiku.DebugMod {
    public static class MiniCheats {
        public static bool Invuln = false;
        public static bool IgnoreHeat = false;
        public static bool QuickMapWarp = false;
        public static bool CameraFollow = false;
        public static float CameraZoom = 7f;
        public static float InitCameraZoom = 7f;

        public static bool IsInvuln() {
            return Invuln;
        }

        public static bool IsIgnoreHeat() {
            return IgnoreHeat;
        }
        public static void toggleQuickMap()
        {
            QuickMapWarp = !QuickMapWarp;
        }

        public static void GiveAllMaps()
        {
            for (int i = 0; i < GameManager.instance.mapTiles.Length; i++)
            {
                GameManager.instance.mapTiles[i].explored = true;
            }
            for (int j = 0; j < GameManager.instance.disruptors.Length; j++)
            {
                GameManager.instance.disruptors[j].destroyed = true;
            }

            //Turn on all map markers
            GameManager.instance.showPowercells = true;
            GameManager.instance.showHealthStations = true;
            GameManager.instance.showBankStations = true;
            GameManager.instance.showVendors = true;
            GameManager.instance.showTrainStations = true;
        }

        public static void GiveAllChips()
        {
            for (int i = 0; i < GameManager.instance.chip.Length; i++)
            {
                GameManager.instance.chip[i].collected = true;
            }
            for (int i = 0; i < GameManager.instance.chipSlot.Length; i++)
            {
                GameManager.instance.chipSlot[i].collected = true;
            }
        }

        public static void IncCoolingPoints()
        {
            GameManager.instance.coolingPoints = (GameManager.instance.coolingPoints + 1) % 4;
        }
    }
}
