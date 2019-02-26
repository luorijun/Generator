using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetManager : MonoBehaviour {

    public int resolution;

    private Face[] faces = new Face[6];

    void Start () {
        faces[0] = new Face(transform, Vector3.up);
        faces[1] = new Face(transform, Vector3.down);
        faces[2] = new Face(transform, Vector3.left);
        faces[3] = new Face(transform, Vector3.right);
        faces[4] = new Face(transform, Vector3.forward);
        faces[5] = new Face(transform, Vector3.back);

        UpdatePlanet();
    }
	
	void Update () {
		
	}

    private void OnValidate()
    {
        UpdatePlanet();
    }

    private void UpdatePlanet()
    {
        resolution = Mathf.Max(3, resolution);
        resolution = Mathf.Min(resolution, 241);

        for (int i = 0; i < faces.Length; i++)
        {
            if (faces[i]!=null)
                faces[i].CreateMesh(resolution);
        }
    }
}
