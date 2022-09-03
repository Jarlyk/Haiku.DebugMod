using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Haiku.DebugMod {
    public class HitboxRendering : MonoBehaviour {
        public static bool ShowHitboxes;

        private static Texture2D texBox;
        private static Texture2D texCircle;
        private static Texture2D texSolid;
        private static TilemapCollider2D lastTmc;
        private static CompositeCollider2D lastCc;
        private static Mesh tmcMesh;
        private static Material tileColliderMaterial;

        public static void Init() {
            texBox = TextureUtils.LoadEmbedded("square.png", 64, 64);
            texCircle = TextureUtils.LoadEmbedded("circle.png", 64, 64);
            texSolid = new Texture2D(1, 1);
            texSolid.SetPixel(0, 0, Color.white);
        }

        private void OnGUI()
        {
            if (Event.current is { type: EventType.Repaint })
            {
                Render();
            }
        }

        public static void Render() {
            if (!ShowHitboxes) return;

            var colliders = Object.FindObjectsOfType<Collider2D>();
            foreach (var collider in colliders) {
                if (!collider.isActiveAndEnabled) continue;

                if (collider.gameObject.tag == "Player") {
                    GUI.color = collider.GetComponents<Rigidbody2D>().Any() ? Color.green : Color.magenta;
                } else if (collider.GetComponents<Component>().Any(c => c is IDamageable)){
                    GUI.color = collider.GetComponents<Rigidbody2D>().Any() ? new Color(1, 0.4f, 0, 1) : Color.red;
                }
                else if (collider.isTrigger) {
                    GUI.color = Color.yellow;
                }
                else {
                    GUI.color = Color.white;
                }

                DrawCollider(collider);
            }
        }

        private static void DrawCollider(Collider2D collider) {
            if (collider is CompositeCollider2D cc) DrawCollider(cc);
            if (collider is TilemapCollider2D tmc) DrawCollider(tmc);
            if (collider is CircleCollider2D circle) DrawCollider(circle);
            if (collider is BoxCollider2D box) DrawCollider(box);
            if (collider is CapsuleCollider2D capsule) DrawCollider(capsule);
            if (collider is PolygonCollider2D polygon) DrawBox(polygon.bounds);
            DrawBox(collider.bounds);
        }

        private static void DrawCollider(CompositeCollider2D cc) {
            if (!tileColliderMaterial) {
                var existingSprite = Object.FindObjectOfType<SpriteRenderer>();
                if (!existingSprite) return;

                tileColliderMaterial = new Material(Shader.Find("Hidden/Internal-Colored"));
                tileColliderMaterial.color = new Color(0.3f, 0.3f, 1, 0.3f);
                tileColliderMaterial.mainTexture = texSolid;

                // Turn off backface culling, depth writes, depth test.
                tileColliderMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
                tileColliderMaterial.SetInt("_ZWrite", 0);
                tileColliderMaterial.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);               
                
                // Debug.LogWarning($"Created tileColliderMaterial with shader: {tileColliderMaterial.shader.name}");
            }

            bool newMesh = false;
            if (cc != lastCc) {
                if (tmcMesh) {
                    Object.Destroy(tmcMesh);
                }
                tmcMesh = cc.CreateMesh(false, false);
                lastCc = cc;
                if (tmcMesh) {
                    newMesh = true;
                    // Debug.LogWarning($"Created mesh for CompositeCollider2D with {tmcMesh.vertexCount} vertices");

                    //Flip y vertices for use later in rendering in pixel coordinates
                    var vertices = new Vector3[tmcMesh.vertexCount];
                    for (int i = 0; i < tmcMesh.vertexCount; i++) {
                        vertices[i] = new Vector3(tmcMesh.vertices[i].x, -1*tmcMesh.vertices[i].y, tmcMesh.vertices[i].z);
                    }
                    tmcMesh.SetVertices(vertices);
                    tmcMesh.MarkModified();
                }
            }

            if (tmcMesh) {
                var cameraPos = Camera.current.transform.position;
                var p0 = Camera.current.WorldToScreenPoint(Vector3.zero);
                var p1 = Camera.current.WorldToScreenPoint(new Vector3(1, 1, 0));
                var scaleX = p1.x - p0.x;
                var scaleY = p1.y - p0.y;
                var height = Camera.current.pixelHeight;
                var w0 = Camera.current.ScreenToWorldPoint(new Vector3(0, 0, 0));
                if (newMesh) {
                    // Debug.LogWarning($"Using mesh scaling of {scaleX},{scaleY} based on camera sp {p0.x},{p0.y} - {p1.x},{p1.y}");
                    // Debug.LogWarning($"Camera position {cameraPos}");
                    // Debug.LogWarning($"World position for screen origin: {w0}");
                }

                var toOrigin = Matrix4x4.Translate(new Vector3(-w0.x, w0.y, -w0.z));
                var scale = Matrix4x4.Scale(new Vector3(scaleX, scaleY, 1));
                var toScreen = Matrix4x4.Translate(new Vector3(cameraPos.x, cameraPos.y + height, 0));
                var meshTransform = toScreen*scale*toOrigin;
                tileColliderMaterial.SetPass(0);
                Graphics.DrawMeshNow(tmcMesh, meshTransform);
            }
        }

        private static void DrawCollider(TilemapCollider2D tmc) {
            //Nothing to render for tmc, as it uses composite collider instead
            return;

            #pragma warning disable CS0162 // Unreachable code detected
            if (!tileColliderMaterial) {
                var existingSprite = Object.FindObjectOfType<SpriteRenderer>();
                if (!existingSprite) return;

                tileColliderMaterial = new Material(existingSprite.material.shader);
                tileColliderMaterial.color = new Color(0, 1, 0, 0.4f);
                tileColliderMaterial.mainTexture = texSolid;
                // Debug.LogWarning($"Created tileColliderMaterial with shader: {existingSprite.material.shader.name}");
                if (tmc.composite) {
                    // Debug.LogWarning("Located composite collider attached to Tilemap");
                }
            }
            #pragma warning restore CS0162 // Unreachable code detected

            if (tmc != lastTmc) {
                if (tmcMesh) {
                    Object.Destroy(tmcMesh);
                }
                tmcMesh = tmc.CreateMesh(true, false);
                lastTmc = tmc;
            }

            if (tmcMesh) {
                Graphics.DrawMesh(tmcMesh, Vector3.zero, Quaternion.identity, tileColliderMaterial, 1);
            }
        }

        private static void DrawCollider(CircleCollider2D circle) {
            DrawCircle(circle.bounds);
        }

        private static void DrawCircle(Bounds bounds) {
            var s0 = Camera.current.WorldToScreenPoint(bounds.min);
            var s2 = Camera.current.WorldToScreenPoint(bounds.max);
            Graphics.DrawTexture(new Rect(s0.x, Camera.current.pixelHeight - s2.y, s2.x - s0.x, s2.y - s0.y), texCircle, new Rect(0, 0, 1, 1), 0, 0, 0, 0, GUI.color);
        }

        private static void DrawCollider(BoxCollider2D box) {
            DrawBox(box.bounds);
        }

        private static void DrawCollider(CapsuleCollider2D capsule) {
            if (capsule.direction == CapsuleDirection2D.Horizontal) {
                var box = new Bounds(capsule.bounds.center, new Vector3(capsule.size.x - 2*capsule.size.y, capsule.size.y));
                DrawBox(box);
                var c0 = new Bounds(capsule.bounds.center - new Vector3(capsule.size.x/2 - capsule.size.y, 0),
                                    new Vector3(capsule.size.y, capsule.size.y));
                var c1 = new Bounds(capsule.bounds.center + new Vector3(capsule.size.x/2 - capsule.size.y, 0),
                                    new Vector3(capsule.size.y, capsule.size.y));
                DrawCircle(c0);
                DrawCircle(c1);
            }
            else if (capsule.direction == CapsuleDirection2D.Vertical) {
                var box = new Bounds(capsule.bounds.center, new Vector3(capsule.size.x, capsule.size.y - 2*capsule.size.x));
                DrawBox(box);
                var c0 = new Bounds(capsule.bounds.center - new Vector3(0, capsule.size.y/2 - capsule.size.x),
                                    new Vector3(capsule.size.x, capsule.size.x));
                var c1 = new Bounds(capsule.bounds.center + new Vector3(0, capsule.size.y/2 - capsule.size.x),
                                    new Vector3(capsule.size.x, capsule.size.x));
                DrawCircle(c0);
                DrawCircle(c1);
            }
        }

        public static void DrawBox(Bounds bounds) {
            var s0 = Camera.current.WorldToScreenPoint(bounds.min);
            var s2 = Camera.current.WorldToScreenPoint(bounds.max);
            Graphics.DrawTexture(new Rect(s0.x, Camera.current.pixelHeight - s2.y, s2.x - s0.x, s2.y - s0.y), texBox, new Rect(0, 0, 1, 1), 3, 3, 3, 3, GUI.color);
        }

        private static void DrawCollider(PolygonCollider2D polygon) {
            DrawBox(polygon.bounds);
        }
    }
}
