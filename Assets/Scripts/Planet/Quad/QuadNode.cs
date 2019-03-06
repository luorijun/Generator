using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Generator.DataStructure {

    public class QuadNode<T> {
        public T Value { get; set; }

        public int Depth;

        // children
        private bool HasChild;

        private QuadNode<T> TopLeftChild;
        private QuadNode<T> TopRightChild;
        private QuadNode<T> BottomLeftChild;
        private QuadNode<T> BottomRightChild;

        public QuadNode(Func<int, T> Init, int Depth, QuadTree<T> Tree) {
            Value = Init(Depth);
            this.Depth = Depth;

            if (Depth < Tree.Level) {
                TopLeftChild = new QuadNode<T>(Init, Depth + 1, Tree);
                TopRightChild = new QuadNode<T>(Init, Depth + 1, Tree);
                BottomLeftChild = new QuadNode<T>(Init, Depth + 1, Tree);
                BottomRightChild = new QuadNode<T>(Init, Depth + 1, Tree);

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
