using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int width = 10;
    public int height = 10;
    public GameObject wallPrefab;
    public GameObject boxPrefab;
    public GameObject targetPrefab;
    public GameObject playerPrefab;
    public GameObject floorPrefab;

    private HashSet<Vector2Int> usedPositions = new HashSet<Vector2Int>();
    private int wallCount = 10;
    private void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        usedPositions.Clear();
        ClearMap();

        
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Instantiate(floorPrefab, new Vector3(x, y, 0), Quaternion.identity);
            }
        }

        
        CreateBoundaryWalls();

        
        int targetCount = PlayerPrefs.GetInt("BoxCount", 1); 
        List<Vector2Int> targets = new List<Vector2Int>();
        for (int i = 0; i < targetCount; i++)
        {
            Vector2Int targetPos = PlaceRandom(targetPrefab, false);
            targets.Add(targetPos);
        }

        
        foreach (var target in targets)
        {
            Vector2Int boxPos = FindValidBoxPositionNearTarget(target);
            PlaceAt(boxPrefab, boxPos);
        }

        
        PlaceRandom(playerPrefab, false);
        GenerateInternalWalls(wallCount);
    }

    void GenerateInternalWalls(int count)
    {
        for (int i = 0; i < count; i++)
        {
            PlaceRandom(wallPrefab, true);
        }
    }


    void CreateBoundaryWalls()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                {
                    Instantiate(wallPrefab, new Vector3(x, y, 0), Quaternion.identity);
                    usedPositions.Add(new Vector2Int(x, y));
                }
            }
        }
    }


    Vector2Int PlaceRandom(GameObject prefab, bool ignoreEdges)
    {
        int x, y, attempts = 0;
        do
        {
            x = Random.Range(1, width - 1);
            y = Random.Range(1, height - 1);
            attempts++;
            if (attempts > 100) break;
        } while ((ignoreEdges && (x == 0 || y == 0 || x == width - 1 || y == height - 1)) ||
                 usedPositions.Contains(new Vector2Int(x, y)));

        if (attempts <= 100)
        {
            Instantiate(prefab, new Vector3(x, y, 0), Quaternion.identity);
            usedPositions.Add(new Vector2Int(x, y));
            return new Vector2Int(x, y);
        }

        return new Vector2Int(-1, -1); 
    }


    Vector2Int FindValidBoxPositionNearTarget(Vector2Int target)
    {
        
        List<Vector2Int> possiblePositions = new List<Vector2Int>();
        for (int dx = -2; dx <= 2; dx++)
        {
            for (int dy = -2; dy <= 2; dy++)
            {
                Vector2Int pos = new Vector2Int(target.x + dx, target.y + dy);
                if (!usedPositions.Contains(pos) && IsInsideBounds(pos) && (dx != 0 || dy != 0))
                {
                    possiblePositions.Add(pos);
                }
            }
        }

        if (possiblePositions.Count > 0)
        {
            Vector2Int selected = possiblePositions[Random.Range(0, possiblePositions.Count)];
            usedPositions.Add(selected);
            return selected;
        }
        return new Vector2Int(-1, -1);
    }


    bool IsInsideBounds(Vector2Int pos)
    {
        return pos.x > 0 && pos.x < width - 1 && pos.y > 0 && pos.y < height - 1;
    }


    void PlaceAt(GameObject prefab, Vector2Int pos)
    {
        Instantiate(prefab, new Vector3(pos.x, pos.y, 0), Quaternion.identity);
    }

    void ClearMap()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}