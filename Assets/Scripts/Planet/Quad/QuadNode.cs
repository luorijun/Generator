using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Terrain
{
    [System.Serializable]
    public class QuadNode
    {
        public NodeType Type;


        [SerializeField]
        private int depth;
        [SerializeField]
        private int size;

        private QuadTree tree;
        private QuadNode parent;

        #region Vertex
        public QuadNodeVertex VertexTopLeft;
        public QuadNodeVertex VertexTop;
        public QuadNodeVertex VertexTopRight;
        public QuadNodeVertex VertexLeft;
        public QuadNodeVertex VertexCenter;
        public QuadNodeVertex VertexRight;
        public QuadNodeVertex VertexBottomLeft;
        public QuadNodeVertex VertexBottom;
        public QuadNodeVertex VertexBottomRight;
        #endregion

        private bool hasChild = false;
        #region Child
        public QuadNode ChildTopLeft;
        public QuadNode ChildTopRight;
        public QuadNode ChildBottomLeft;
        public QuadNode ChildBottomRight;
        #endregion

        #region Neihbor
        public QuadNode NeighborTop;
        public QuadNode NeighborRight;
        public QuadNode NeighborBottom;
        public QuadNode NeighborLeft;
        #endregion

        public QuadNode(NodeType type, int depth, QuadTree tree, QuadNode parent = null)
        {
            Type = type;

            if (type != NodeType.FullNode)
                this.parent = parent;
            this.tree = tree;

            this.depth = depth;

            if (depth == 1)
                size = tree.MaxSize;
            else
                size = tree.MaxSize / (int)Mathf.Pow(2, depth - 1) + 1;

            AddVertices();
            if (depth < tree.MaxDepth)
                AddChildren();
        }

        private void AddVertices()
        {
            switch (Type)
            {
                case NodeType.TopLeft:
                    VertexTopLeft = parent.VertexTopLeft;
                    VertexTopRight = parent.VertexTop;
                    VertexBottomLeft = parent.VertexLeft;
                    VertexBottomRight = parent.VertexCenter;

                    VertexTopLeft.Activated = true;
                    VertexTopRight.Activated = true;
                    VertexBottomLeft.Activated = true;
                    VertexBottomRight.Activated = true;
                    break;

                case NodeType.TopRight:
                    VertexTopLeft = parent.VertexTop;
                    VertexTopRight = parent.VertexTopRight;
                    VertexBottomLeft = parent.VertexCenter;
                    VertexBottomRight = parent.VertexRight;

                    VertexTopLeft.Activated = true;
                    VertexTopRight.Activated = true;
                    VertexBottomLeft.Activated = true;
                    VertexBottomRight.Activated = true;
                    break;

                case NodeType.BottomLeft:
                    VertexTopLeft = parent.VertexLeft;
                    VertexTopRight = parent.VertexCenter;
                    VertexBottomLeft = parent.VertexBottomLeft;
                    VertexBottomRight = parent.VertexBottom;

                    VertexTopLeft.Activated = true;
                    VertexTopRight.Activated = true;
                    VertexBottomLeft.Activated = true;
                    VertexBottomRight.Activated = true;
                    break;

                case NodeType.BottomRight:
                    VertexTopLeft = parent.VertexCenter;
                    VertexTopRight = parent.VertexRight;
                    VertexBottomLeft = parent.VertexBottom;
                    VertexBottomRight = parent.VertexBottomRight;

                    VertexTopLeft.Activated = true;
                    VertexTopRight.Activated = true;
                    VertexBottomLeft.Activated = true;
                    VertexBottomRight.Activated = true;
                    break;

                default:
                    VertexTopLeft = new QuadNodeVertex
                    {
                        Index = 0,
                        Activated = true
                    };

                    VertexTopRight = new QuadNodeVertex
                    {
                        Index = tree.MaxSize - 1,
                        Activated = true
                    };

                    VertexBottomLeft = new QuadNodeVertex
                    {
                        Index = (tree.MaxSize - 1) * tree.MaxSize,
                        Activated = true
                    };

                    VertexBottomRight = new QuadNodeVertex
                    {
                        Index = tree.MaxSize * tree.MaxSize - 1,
                        Activated = true
                    };
                    break;
            }

            VertexLeft = new QuadNodeVertex
            {
                Index = VertexTopLeft.Index + tree.MaxSize * (size / 2),
                Activated = false
            };

            VertexTop = new QuadNodeVertex
            {
                Index = VertexTopLeft.Index + (size / 2),
                Activated = false
            };

            VertexCenter = new QuadNodeVertex
            {
                Index = VertexLeft.Index + (size / 2),
                Activated = false
            };

            VertexBottom = new QuadNodeVertex
            {
                Index = VertexBottomLeft.Index + (size / 2),
                Activated = false
            };

            VertexRight = new QuadNodeVertex
            {
                Index = VertexLeft.Index + size - 1,
                Activated = false
            };
        }
        private void AddChildren()
        {
            if (depth<tree.MaxDepth)
            {
                ChildTopLeft = new QuadNode(NodeType.TopLeft, depth + 1, tree, this);
                ChildTopRight = new QuadNode(NodeType.TopRight, depth + 1, tree, this);
                ChildBottomLeft = new QuadNode(NodeType.BottomLeft, depth + 1, tree, this);
                ChildBottomRight = new QuadNode(NodeType.BottomRight, depth + 1, tree, this);

                hasChild = true;
            }
        }

        private bool Sub()
        {
            return tree.Dis < (Vector3.Distance(Camera.main.transform.position, tree.vertices[VertexCenter.Index]) / size);
        }

        public List<int> Update()
        {
            var triagnles = new List<int>();

            if (tree.Dis>(Vector3.Distance(Camera.main.transform.position, tree.vertices[VertexCenter.Index])/size) && hasChild)
            {
                triagnles.AddRange(ChildTopLeft.Update());
                triagnles.AddRange(ChildTopRight.Update());
                triagnles.AddRange(ChildBottomLeft.Update());
                triagnles.AddRange(ChildBottomRight.Update());
                return triagnles;
            }

            triagnles.Add(VertexCenter.Index);
            triagnles.Add(VertexTopLeft.Index);

            if (VertexLeft.Activated)
            {
                triagnles.Add(VertexLeft.Index);
                triagnles.Add(VertexCenter.Index);
                triagnles.Add(VertexLeft.Index);
            }

            triagnles.Add(VertexBottomLeft.Index);
            triagnles.Add(VertexCenter.Index);
            triagnles.Add(VertexBottomLeft.Index);

            if (VertexBottom.Activated)
            {
                triagnles.Add(VertexBottom.Index);
                triagnles.Add(VertexCenter.Index);
                triagnles.Add(VertexBottom.Index);
            }
            triagnles.Add(VertexBottomRight.Index);
            triagnles.Add(VertexCenter.Index);
            triagnles.Add(VertexBottomRight.Index);

            if (VertexRight.Activated)
            {
                triagnles.Add(VertexRight.Index);
                triagnles.Add(VertexCenter.Index);
                triagnles.Add(VertexRight.Index);
            }

            triagnles.Add(VertexTopRight.Index);
            triagnles.Add(VertexCenter.Index);
            triagnles.Add(VertexTopRight.Index);

            if (VertexTop.Activated)
            {
                triagnles.Add(VertexTop.Index);
                triagnles.Add(VertexCenter.Index);
                triagnles.Add(VertexTop.Index);
            }

            triagnles.Add(VertexTopLeft.Index);

            return triagnles;
        }

        public void LogVertices()
        {
            Debug.Log(VertexTopLeft.Index);
            Debug.Log(VertexTop.Index);
            Debug.Log(VertexTopRight.Index);
            Debug.Log(VertexLeft.Index);
            Debug.Log(VertexCenter.Index);
            Debug.Log(VertexRight.Index);
            Debug.Log(VertexBottomLeft.Index);
            Debug.Log(VertexBottom.Index);
            Debug.Log(VertexBottomRight.Index);
        }
    }
}
