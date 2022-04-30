using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private Transform NewPlatform;

    void Awake()
    {
        //SpawnPlatform(new Vector3(24, 1));
        //SpawnPlatform(new Vector3(24, 1) + new Vector3(8, 1));
        //SpawnPlatform(new Vector3(24, 1) + new Vector3(8 + 8, -1));
    }

    private void SpawnPlatform(Vector3 pos)
    {
        Instantiate(NewPlatform, pos, Quaternion.identity);
    }

}
