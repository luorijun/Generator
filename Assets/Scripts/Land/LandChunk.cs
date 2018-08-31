using UnityEngine;

public class LandChunk
{
    public GameObject gameObject;
    public Bounds bounds;

    public LandChunk(Vector2 position, int size, Transform parent)
    {
        gameObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
        gameObject.transform.position = new Vector3(position.x, 0, position.y);
        gameObject.transform.localScale = new Vector3(size / 10f, 1, size / 10f);

        bounds = new Bounds(position, Vector2.one * size);

        gameObject.transform.parent = parent;
    }
}
