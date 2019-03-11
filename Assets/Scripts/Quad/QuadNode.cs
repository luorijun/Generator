using System;

namespace Generator.DataStructure {

    public class QuadNode<T> {
        public T Value { get; set; }

        public int Depth;

        public enum NodeMode { Full, TopLeft, TopRight, BottomLeft, BottomRight }
        public NodeMode Mode;

        public QuadNode<T> Parent;
        // children
        private readonly bool HasChild;

        private QuadNode<T> TopLeftChild;
        private QuadNode<T> TopRightChild;
        private QuadNode<T> BottomLeftChild;
        private QuadNode<T> BottomRightChild;

        public QuadNode(Func<QuadNode<T>, T> Init, int depth, int level, NodeMode mode, QuadNode<T> parent = null) {
            Depth = depth;
            Parent = parent;
            Mode = mode;

            Value = Init(this);
            if (Depth < level) {
                TopLeftChild = new QuadNode<T>(Init, Depth + 1, level, NodeMode.TopLeft, this);
                TopRightChild = new QuadNode<T>(Init, Depth + 1, level, NodeMode.TopRight, this);
                BottomLeftChild = new QuadNode<T>(Init, Depth + 1, level, NodeMode.BottomLeft, this);
                BottomRightChild = new QuadNode<T>(Init, Depth + 1, level, NodeMode.BottomRight, this);

                HasChild = true;
            }
            else HasChild = false;
        }

        public void Operate(Action<QuadNode<T>> otherwise) {
            otherwise(this);
            if (HasChild) {
                TopLeftChild.Operate(otherwise);
                TopRightChild.Operate(otherwise);
                BottomLeftChild.Operate(otherwise);
                BottomRightChild.Operate(otherwise);
            }
        }
        public void Operate(
            Func<QuadNode<T>, bool> condition,
            Action<QuadNode<T>> conclusion,
            Action<QuadNode<T>> otherwise) {

            if (condition(this)) {
                conclusion(this);
                if (HasChild) {
                    TopLeftChild.Operate(otherwise);
                    TopRightChild.Operate(otherwise);
                    BottomLeftChild.Operate(otherwise);
                    BottomRightChild.Operate(otherwise);
                }
            }
            else {
                if (HasChild) {
                    otherwise(this);
                    TopLeftChild.Operate(condition, conclusion, otherwise);
                    TopRightChild.Operate(condition, conclusion, otherwise);
                    BottomLeftChild.Operate(condition, conclusion, otherwise);
                    BottomRightChild.Operate(condition, conclusion, otherwise);
                }
                else {
                    conclusion(this);
                }
            }
        }
    }
}
