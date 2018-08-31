using System.Collections.Generic;
using UnityEngine;

public class LandLoader
{
    private Transform viewer;
    private Vector2 viewerPos;

    private float visibleDistance;
    private int visibleChunk;

    private int chunkSize;
    private Dictionary<Vector2, LandChunk> chunkDictionary;
    private List<LandChunk> activeChunks;

    private Transform chunksParent;

    public LandLoader(Transform viewer, float visibleDistance, int chunkSize, Transform chunksParent)
    {
        this.viewer = viewer;
        this.visibleDistance = visibleDistance;
        this.chunkSize = chunkSize;
        this.chunksParent = chunksParent;

        viewerPos = new Vector2(viewer.position.x, viewer.position.z);
        visibleChunk = Mathf.RoundToInt(visibleDistance / chunkSize);
        chunkDictionary = new Dictionary<Vector2, LandChunk>();
        activeChunks = new List<LandChunk>();
    }

    public void Update()
    {
        viewerPos = new Vector2(viewer.position.x, viewer.position.z);

        for (int i = 0; i < activeChunks.Count; i++)
        {
            activeChunks[i].gameObject.SetActive(false);
        }
        activeChunks.Clear();

        Vector2 viewerChunkCorrd = new Vector2(
            Mathf.RoundToInt(viewer.position.x / chunkSize),
            Mathf.RoundToInt(viewer.position.z / chunkSize));

        for (int y = (int)viewerChunkCorrd.y - visibleChunk; y <= viewerChunkCorrd.y + visibleChunk; y++)
        {
            for (int x = (int)viewerChunkCorrd.x - visibleChunk; x <= viewerChunkCorrd.x + visibleChunk; x++)
            {
                Vector2 currentChunkCoord = new Vector2(x, y);
                Vector2 currentChunkPosition = new Vector2(
                    currentChunkCoord.x * chunkSize,
                    currentChunkCoord.y * chunkSize);

                LandChunk currentChunk;
                if (chunkDictionary.ContainsKey(currentChunkCoord))
                {
                    currentChunk = chunkDictionary[currentChunkCoord];
                }
                else
                {
                    currentChunk = new LandChunk(currentChunkPosition, chunkSize, chunksParent);
                    chunkDictionary.Add(currentChunkCoord, currentChunk);
                }

                UpdateChunk(currentChunk);
                if (currentChunk.gameObject.activeSelf)
                {
                    activeChunks.Add(currentChunk);
                }
            }
        }
    }

    private void UpdateChunk(LandChunk chunk)
    {
        float viewerDistance = Mathf.Sqrt(chunk.bounds.SqrDistance(viewerPos));
        chunk.gameObject.SetActive(viewerDistance <= visibleDistance);
    }
}
