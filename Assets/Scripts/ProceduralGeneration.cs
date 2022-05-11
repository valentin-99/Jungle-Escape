using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProceduralGeneration : MonoBehaviour
{
    [SerializeField] int width, height;
    [SerializeField] float smoothness;
    [SerializeField] float seed;
    [SerializeField] TileBase natureTile, skyTile;
    [SerializeField] Tilemap natureTilemap, skyTilemap;
    int[,] map;

    [Header("Sky")]
    [Range(0, 1)]
    [SerializeField] float modifier;

    private void Start()
    {
        Generation();
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
        natureTilemap.ClearAllTiles();
        map = GenerateArray(width, height, true);
        map = TerrainGeneration(map);
        RenderMap(map, natureTilemap, skyTilemap, natureTile, skyTile);
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
        natureTilemap.ClearAllTiles();
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
                //map[i, j] = 1;
                int skyVal = Mathf.RoundToInt(Mathf.PerlinNoise((i * modifier) * seed, (j * modifier) + seed));
                if (skyVal == 1)
                    map[i, j] = 2;
                else
                    map[i, j] = 1;
            }
        }

        return map;
    }

    private void RenderMap(int[,] map, Tilemap natureTilemap, Tilemap skyTilemap, TileBase natureTile, TileBase skyTile)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (map[i,j] == 1)
                    natureTilemap.SetTile(new Vector3Int(i, j, 0), natureTile);
                else if (map[i,j] == 2 || map[i,j] == 0)
                    skyTilemap.SetTile(new Vector3Int(i, j, 0), skyTile);
            }
        }
    }
}
