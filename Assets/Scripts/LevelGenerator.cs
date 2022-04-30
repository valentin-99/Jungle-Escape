using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform startPlatform;
    [SerializeField] private List<Transform> PlatformsList;
    [SerializeField] private Transform background;

    private Transform currentPlatform;
    private Transform lastPlatform = null;
    private Vector3 lastMiddlePos = new Vector3(float.MinValue, 0);

    private Transform currentBackground;
    private Transform lastBackground = null;
    private Vector3 lastMiddlePosBackground = new Vector3(float.MinValue, 0);

    private void Start()
    {
        currentPlatform = startPlatform;
        currentBackground = background;
    }

    private Transform SpawnPlatform(Transform platform, Vector3 pos)
    {
        return Instantiate(platform, pos, Quaternion.identity);
    }

    private void Update()
    {
        //GenerateBackground();
        GeneratePlatforms();
    }

    private void GeneratePlatforms()
    {
        // get middle position of current chunk
        Vector3 currentMiddlePos = currentPlatform.Find("MiddlePos").position;

        // if player arrive at current middle position
        if (player.position.x >= currentMiddlePos.x)
        {
            // if there is any previous chunk and if player arrived at the middle of the
            // new chunk then delete the previous platform.
            if (lastMiddlePos.x != float.MinValue && player.position.x >= lastMiddlePos.x + 24)
            {
                Destroy(lastPlatform.gameObject);
            }

            // choose a random platform from list, assign it as the new current platform
            // and render it at previous current position + 12
            int randomIndex = Random.Range(0, PlatformsList.Count);
            Transform randomPlatform = PlatformsList[randomIndex];
            lastPlatform = currentPlatform;
            lastMiddlePos = currentMiddlePos;
            if (randomPlatform != null)
            {
                currentPlatform = SpawnPlatform(randomPlatform, new Vector3(lastMiddlePos.x + 12, lastMiddlePos.y));
            }
        }
    }

    private void GenerateBackground()
    {
        Vector3 currentMiddlePos = currentBackground.Find("MiddlePos").position;

        if (player.position.x >= currentMiddlePos.x)
        {
            if (lastMiddlePosBackground.x != float.MinValue)
            {
                Destroy(lastBackground.gameObject);
            }

            lastBackground = currentBackground;
            lastMiddlePosBackground = currentMiddlePos;
            currentPlatform = SpawnPlatform(background, new Vector3(lastMiddlePosBackground.x + 60, lastMiddlePosBackground.y));
        }
    }
}
