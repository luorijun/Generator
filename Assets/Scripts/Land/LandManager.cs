using UnityEngine;

public class LandManager : MonoBehaviour
{
    public GameObject land;
    public GameObject viewer;

    public enum DrawMode { HeightMap, ColorMap }
    public DrawMode drawMode;

    public bool autoUpdate;

    public ChunkSettings landSetting;

    private LandLoader loader;

    void Start()
    {
        loader = new LandLoader(viewer.transform, 450, landSetting.size, transform);
    }

    void Update()
    {
        loader.Update();
    }

    void OnValidate()
    {
        landSetting.size = landSetting.size - landSetting.size % 16 + 1;
    }

    public void Generate()
    {
        //LandGenerator.Get(landSetting, ref land, (TextureMode)drawMode);
    }
}
