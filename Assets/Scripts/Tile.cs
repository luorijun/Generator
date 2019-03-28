using Generator.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Generator {

    public class Tile: MonoBehaviour {
        public MeshFilter Filter;
        public MeshRenderer Renderer;
        public Face Face;

        public const int RESOLUTION = 32;

        public float Size;
        public float SizePerCell;

        public Tile Init(float size, Face face) {
            Face = face;
            Size = size;
            SizePerCell = Size / (RESOLUTION - 1);

            Filter.mesh = MeshFactory.CreatePlane(RESOLUTION,
                (x, z) => {
                    var xCoord = (x * SizePerCell - size / 2) * Face.XAxis;
                    var zCoord = (z * SizePerCell - size / 2) * Face.ZAxis;
                    var coord = (xCoord + zCoord
                        + transform.position)
                        .normalized * Face.Planet.Radius
                        - transform.position;
                    return coord;
                });
            return this;
        }
    }
}