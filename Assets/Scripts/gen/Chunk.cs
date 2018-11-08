using System.Threading;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Chunk : MonoBehaviour
{
    private Transform viewer;

    private GenerateSetting setting;
    private Bounds bounds;
    private float[,] heightData;

    private float maxDistance;

    public bool sign = false;

    public GameObject Init(GenerateSetting setting, Vector2 coord, Transform viewer, float maxDistance, bool randomSeed)
    {
        name = coord.ToString();

        Vector2 pos = coord * (setting.size - 1);
        Vector3 position = new Vector3(pos.x, 0, pos.y);

        transform.position = position;

        if (randomSeed)
            setting.seed = Random.Range(-100000, 100000);
        this.setting = setting;
        bounds = new Bounds(position, new Vector3(1, 0, 1) * setting.size);
        this.viewer = viewer;
        this.maxDistance = maxDistance;

        return gameObject;
    }

    void Start()
    {
        // 请求地形数据
        new Thread(() => {
            heightData = ChunkGenerator.CalculateHeightData(setting);
            sign = true;
        }).Start();
    }

    void Update()
    {
        
    }

    public void ActiveUpdate()
    {
        // 生成地形
        if (sign)
        {
            ChunkGenerator.Get(heightData, setting, gameObject);
            sign = false;
        }

        float distance = Mathf.Sqrt(bounds.SqrDistance(viewer.position));
        gameObject.SetActive(distance <= maxDistance);
    }
}
