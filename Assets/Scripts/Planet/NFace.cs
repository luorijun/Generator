using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NFace: MonoBehaviour {
    public Tile tile;

    public int Level;
    public int Size;
    public float ViewDistance;

    private Tile root;

    void Start() {
        root = Instantiate(tile, transform).Init(this, 1);
    }

    void Update() {
        root.UpdateState();
    }
}
