using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Generator.DataStructure {

    public class QuadTree<T> {
        private QuadNode<T> Root;

        public int Level;

        public QuadTree(int Level, Func<QuadNode<T>, T> Init) {
            this.Level = Level;
            Root = new QuadNode<T>(Init, 1, Level, QuadNode<T>.NodeMode.Full);
        }

        public void Traversing(
            Func<QuadNode<T>, bool> condition,
            Action<QuadNode<T>> conclusion,
            Action<QuadNode<T>> otherwise) {

            Root.Operate(condition, conclusion, otherwise);
        }
    }
}
