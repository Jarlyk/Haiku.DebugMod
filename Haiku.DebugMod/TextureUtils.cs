using System.Reflection;
using UnityEngine;

namespace Haiku.DebugMod {
    public static class TextureUtils {

        public static Texture2D LoadEmbedded(string filename, int width, int height) {
            var key = $"Haiku.DebugMod.Images.{filename}";
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(key)) {
                var data = new byte[(int)stream.Length];
                stream.Read(data, 0, data.Length);
                var tex = new Texture2D(width, height, TextureFormat.BGRA32, false);
                tex.LoadImage(data);
                return tex;
            }
        }
    }
}
