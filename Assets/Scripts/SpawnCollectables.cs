using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCollectables : MonoBehaviour
{
    [SerializeField] GameObject cherry;
    [SerializeField] GameObject proceduralGeneration;

    // Start is called before the first frame update
    private void Start()
    {
        Spawn();
    }

    // Spawn player on the top of the map
    private void Spawn()
    {
        ProceduralGeneration pg = proceduralGeneration.GetComponent<ProceduralGeneration>();

        // list with highest ground of each column
        List<Pair> spawnArray = new List<Pair>(pg.width);
        bool findGround;

        // for each column, skip first chunk of sky tiles and populate the
        // spawnArray with positions of the rest of sky chunks.
        for (int x = 0; x < pg.width; x++)
        {
            findGround = false;
            for (int y = pg.height - 1; y >= 0; y--)
            {
                if (pg.map[x, y] == 1 && !findGround)
                {
                    for (int k = y; k >= 0; k--)
                    {
                        if (pg.map[x, k] == 2)
                        {
                            spawnArray.Add(new Pair(x, k));
                        }
                    }
                    findGround = true;
                }
            }
        }

        // shuffle the list
        for (int i = 0; i < spawnArray.Count; i++)
        {
            Pair temp = spawnArray[i];
            int randomIdx = Random.Range(i, spawnArray.Count);
            spawnArray[i] = spawnArray[randomIdx];
            spawnArray[randomIdx] = temp;
        }

        // render the collectables with correction (+.5f, +.5f)
        for (int i = 0; i < 100; i++)
        {
            Vector2 spawnPos = new Vector2(spawnArray[i].X + .5f, spawnArray[i].Y + .5f);
            Instantiate(cherry, spawnPos, Quaternion.identity);
        }
    }
}
