using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Pair
{
    public Pair(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X { get; }
    public int Y { get; }

    public override string ToString() => $"({X}, {Y})";
}

public class SpawnPlayer : MonoBehaviour
{
    [SerializeField] Transform player;
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
        bool elemAdded;

        for (int x = 0; x < pg.width; x++)
        {
            elemAdded = false;
            for (int y = pg.height - 1; y >= 0; y--)
            {
                if (pg.map[x, y] == 1 && !elemAdded)
                {
                    spawnArray.Add(new Pair(x, y));
                    elemAdded = true;
                }
            }
        }

        int randomIdx = Random.Range(0, spawnArray.Count);

        /*Debug.Log(spawnArray[0]);
        Debug.Log(pg.map[spawnArray[0].X, spawnArray[0].X]);
        Debug.Log(spawnArray[1]);
        Debug.Log(pg.map[spawnArray[1].X, spawnArray[1].X]);
        Debug.Log(spawnArray[2]);   
        Debug.Log(pg.map[spawnArray[2].X, spawnArray[2].X]);
        Debug.Log(spawnArray[3]);   
        Debug.Log(pg.map[spawnArray[3].X, spawnArray[3].X]);

        Debug.Log(spawnArray[randomIdx]);*/

        // render the player with correction (+.5f, +2f)
        Instantiate(player, new Vector3(spawnArray[randomIdx].X + .5f, spawnArray[randomIdx].Y + 2), Quaternion.identity);
    }
}
