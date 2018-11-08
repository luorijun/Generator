using System.Collections.Generic;
using UnityEngine;

public class WorldLoader : MonoBehaviour
{
    // 视点/玩家，可见距离，生成数据
    public Transform viewer;
    public float visibleDistance;
    public GenerateSetting setting;

    // 视点位置，直线可见快数
    private Vector2 viewerPos;
    private int visibleChunks;

    // 所有地块集合，活动地块集合
    private Dictionary<Vector2, GameObject> chunkDictionary;
    private List<GameObject> activedChunks;

    void Start()
    {
        // 初始化
        viewerPos = new Vector3(viewer.position.x, viewer.position.z);
        visibleChunks = Mathf.RoundToInt(visibleDistance / setting.size);

        chunkDictionary = new Dictionary<Vector2, GameObject>();
        activedChunks = new List<GameObject>();
    }

    void Update()
    {
        // 更新位置
        viewerPos = new Vector2(viewer.position.x, viewer.position.z);

        // 清空可见地块
        foreach (var item in activedChunks)
            item.gameObject.SetActive(false);
        activedChunks.Clear();

        // 视点所在地块
        Vector2 viewerChunkCoord = new Vector2(
            Mathf.RoundToInt(viewerPos.x / setting.size),
            Mathf.RoundToInt(viewerPos.y / setting.size));
        // 以视点所在地块为中心更新地块
        for (int y = (int)viewerChunkCoord.y - visibleChunks; y <= viewerChunkCoord.y + visibleChunks; y++)
        {
            for (int x = (int)viewerChunkCoord.x - visibleChunks; x <= viewerChunkCoord.x + visibleChunks; x++)
            {
                Vector2 currentChunkCoord = new Vector2(x, y);
                GameObject currentChunk;

                if (!chunkDictionary.ContainsKey(currentChunkCoord)) {
                    currentChunk = Instantiate(ResourcesManager.chunk).
                        GetComponent<Chunk>().Init(setting, currentChunkCoord, viewer, visibleDistance, true);

                    chunkDictionary.Add(currentChunkCoord, currentChunk);
                }
                else currentChunk = chunkDictionary[currentChunkCoord];

                currentChunk.GetComponent<Chunk>().ActiveUpdate();
                if (currentChunk.gameObject.activeSelf)
                    activedChunks.Add(currentChunk);
            }
        }
    }

}
