using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProceduralGeneration : MonoBehaviour
{
    [SerializeField] public int width, height;
    [SerializeField] private float smoothness;
    [SerializeField] private float seed;
    [SerializeField] private TileBase groundTile, skyTile;
    [SerializeField] private Tilemap groundTilemap, skyTilemap;
    [SerializeField] private GameObject spawnPlayer;

    [Header("Sky")]
    [Range(0, 1)]
    [SerializeField] private float modifier;

    public int[,] map;

    private void Start()
    {
        map = new int [width, height];
        Generation();

        // the ground needs to be rendered first, otherwise the map variable
        // will be empty when it is accessed from SpawnPlayer script
        spawnPlayer.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Generation();
        }
    }

    private void Generation()
    {
        seed = Random.Range(-10000, 10000);
        ClearMap();
        groundTilemap.ClearAllTiles();
        map = GenerateArray(width, height, true);
        map = TerrainGeneration(map);
        RenderMap(map, groundTilemap, skyTilemap, groundTile, skyTile);
    }

    private int[,] GenerateArray(int width, int height, bool empty)
    {
        int[,] map = new int[width, height];
        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
            {
                if (empty)
                    map[i, j] = 0;
                else
                    map[i, j] = 1;
            }

        return map;
    }

    private void ClearMap()
    {
        groundTilemap.ClearAllTiles();
        skyTilemap.ClearAllTiles();
    }

    private int[,] TerrainGeneration(int[,] map)
    {
        int perlinHeight;
        for (int i = 0; i < width; i++)
        {
            perlinHeight = Mathf.RoundToInt(Mathf.PerlinNoise(i / smoothness, seed) * height / 2);
            perlinHeight += height / 2;
            for (int j = 0; j < perlinHeight; j++)
            {
                int skyVal = Mathf.RoundToInt(Mathf.PerlinNoise((i * modifier) * seed, (j * modifier) + seed));
                if (skyVal == 1)
                    map[i, j] = 2;
                else
                    map[i, j] = 1;
            }
        }

        return map;
    }

    private void RenderMap(int[,] map, Tilemap groundTilemap, Tilemap skyTilemap, TileBase groundTile, TileBase skyTile)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (map[i,j] == 1)
                    groundTilemap.SetTile(new Vector3Int(i, j, 0), groundTile);
                else if (map[i,j] == 2 || map[i,j] == 0)
                    skyTilemap.SetTile(new Vector3Int(i, j, 0), skyTile);
            }
        }
    }
}
