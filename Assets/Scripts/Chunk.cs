using Generator.Utility;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Generator {

    public class Chunk: MonoBehaviour {
        #region 静态字段
        public static float Resolution = 32;

        private static readonly Vector3 TopLeft = new Vector3(-1, 0, 1);
        private static readonly Vector3 TopRight = new Vector3(1, 0, 1);
        private static readonly Vector3 BottomLeft = new Vector3(-1, 0, -1);
        private static readonly Vector3 BottomRight = new Vector3(1, 0, -1);
        #endregion

        public Vector3[] vert;

        #region 字段
        public Vector3 center; //
        public Vector3 centerNor;

        public float size; //
        public int depth; //

        public bool needSub;

        public bool hasChild; //
        public bool fullChild;
        public Chunk[] children; //
        private Chunk parent; //

        private Face face;

        public MeshFilter filter; //
        public new MeshRenderer renderer; //
        public new MeshCollider collider;
        #endregion

        public Chunk Init(float size, int depth, Vector3 offset, Chunk parent, Face face) {
            name = "depth: " + depth;
            
            center = face.matrix.MultiplyPoint3x4(offset);
            if (parent != null) center += parent.center;

            centerNor = center.normalized * face.radius;
            centerNor *= (float)(face.perlin.GetValue(center * face.frequency) * face.amplitude + 1);

            this.size = size;
            this.depth = depth;

            hasChild = depth < face.maxDepth;
            fullChild = false;
            children = new Chunk[4];

            this.face = face;

            filter = GetComponent<MeshFilter>();
            renderer = GetComponent<MeshRenderer>();
            collider = GetComponent<MeshCollider>();

            filter.mesh = CreateMesh();
            vert = filter.mesh.vertices;
            renderer.material = face.material;
            collider.sharedMesh = filter.mesh;
            return this;
        }

        public void UpdateState(Transform viewer, float subdivision) {
            // 计算位置
            var ratio = Vector3.Distance(viewer.position, centerNor + transform.position) / size;

            // 需要细分
            if (ratio < subdivision) {
                needSub = true;
                if (!hasChild) {
                    renderer.enabled = true;
                    collider.enabled = true;
                }
                else {
                    renderer.enabled = false;
                    collider.enabled = false;
                    if (!fullChild) CreateChildren();
                    foreach (var child in children) {
                        child.UpdateState(viewer, subdivision);
                    }
                }
            }
            // 无须细分
            else {
                needSub = false;
                renderer.enabled = true;
                collider.enabled = true;
                if (hasChild && fullChild) RemoveChildren();
            }
        }

        private void CreateChildren() {
            fullChild = true;
            AddChild(0, TopLeft);
            AddChild(1, TopRight);
            AddChild(2, BottomLeft);
            AddChild(3, BottomRight);
        }
        private void AddChild(int index, Vector3 offset) {
            children[index] = New(
                    size / 2, depth + 1, size / 4 * offset, this, face, transform);
        }
        private void RemoveChildren() {
            fullChild = false;
            foreach (var child in children) {
                Destroy(child.gameObject);
            }
        }

        public Mesh CreateMesh() =>
            MeshFactory.CreatePlane((int)Resolution, (x, z) => {
                var coord = new Vector3 {
                    x = x / (Resolution - 1) - .5f,
                    y = 0,
                    z = z / (Resolution - 1) - .5f,
                } * size;

                var coordNor = (face.matrix.MultiplyPoint3x4(coord) + center).normalized * face.radius;
                if (face.isNoise) {
                    coordNor *= (float)face.perlin.GetValue(coordNor * face.frequency) * face.amplitude + 1;
                }
                return coordNor;
            });

        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(centerNor + transform.position, size/16);
        }

        public static Chunk New(float size, int depth, Vector3 offset, Chunk parent, Face face, Transform parentTransform) {
            var chunk = new GameObject("chunk",
                typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))
                .AddComponent<Chunk>();
            chunk.transform.position = parentTransform.position;
            chunk.transform.SetParent(parentTransform);
            chunk.name = "depth";
            return chunk.Init(size, depth, offset, parent, face);
        }
    }
}
