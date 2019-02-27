using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NFace : MonoBehaviour {
    public Tile tile;

    public int Level;
    public int Size;

    private Tile root;

	void Start () {
        root = Instantiate(tile, transform).Init(this, 1);
	}
	
	void Update () {
		
	}
}
