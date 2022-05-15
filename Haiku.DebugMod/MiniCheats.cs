namespace Haiku.DebugMod {
    public static class MiniCheats {
        public static bool Invuln;
        public static bool IgnoreHeat;
        public static bool QuickMapWarp = false;

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

        public static void giveAllMaps()
        {
            for (int i = 0; i < GameManager.instance.mapTiles.Length; i++)
            {
                GameManager.instance.mapTiles[i].explored = true;
            }
            for (int j = 0; j < GameManager.instance.disruptors.Length; j++)
            {
                GameManager.instance.disruptors[j].destroyed = true;
            }
        }
    }
}
