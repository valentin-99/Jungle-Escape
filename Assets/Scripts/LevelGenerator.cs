using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform startPlatform;
    [SerializeField] private List<Transform> PlatformsList;
    [SerializeField] private Transform background;
    [SerializeField] private Transform fallingZone;

    private Transform currentPlatform;
    private Transform lastPlatform = null;
    private Vector3 lastMiddlePos = new Vector3(float.MinValue, 0);

    private Transform currentBackground;
    private Transform lastBackground = null;
    private Vector3 lastMiddlePosBackground = new Vector3(float.MinValue, 0);

    private Transform currentFallingZone;
    private Transform lastFallingZone = null;
    private Vector3 lastMiddlePosFall = new Vector3(float.MinValue, -8.5f);


    private void Awake()
    {
        currentBackground = SpawnPlatform(background, new Vector3(0, 0));
        currentPlatform = SpawnPlatform(startPlatform, new Vector3(-11, 0));
        currentFallingZone = SpawnPlatform(fallingZone, new Vector3(0, -8.5f));
    }

    private void Update()
    {
        GenerateBackground();
        GeneratePlatforms();
        GenerateFallingZone();
    }

    private Transform SpawnPlatform(Transform platform, Vector3 pos)
    {
        return Instantiate(platform, pos, Quaternion.identity);
    }

    private void GenerateBackground()
    {
        // get middle position of current background
        Vector3 currentMiddlePosBackground = currentBackground.Find("MiddlePos").position;

        // if player arrived at current middle position
        if (player.position.x >= currentMiddlePosBackground.x)
        {
            // if there is any previous background and if player arrived at the middle of the
            // new background then delete the previous background.
            if (lastMiddlePosBackground.x != float.MinValue && player.position.x >= lastMiddlePosBackground.x + 60)
            {
                Destroy(lastBackground.gameObject);
            }

            // assign the current background and it's position to the last background
            // render the new background and assign it to the current one
            lastBackground = currentBackground;
            lastMiddlePosBackground = currentMiddlePosBackground;
            currentBackground = SpawnPlatform(background, new Vector3(lastMiddlePosBackground.x + 60, lastMiddlePosBackground.y));
        }
    }

    private void GeneratePlatforms()
    {
        // get middle position of current chunk
        Vector3 currentMiddlePos = currentPlatform.Find("MiddlePos").position;

        // if player arrive at current middle position
        if (player.position.x >= currentMiddlePos.x)
        {
            // if there is any previous chunk and if player arrived at the middle of the
            // new chunk then delete the previous chunk.
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

    private void GenerateFallingZone()
    {
        Vector3 currentMiddlePosFall = currentFallingZone.position;

        if (player.position.x >= currentMiddlePosFall.x)
        {
            if (lastMiddlePosFall.x != float.MinValue && player.position.x >= lastMiddlePosFall.x + 60)
            {
                Destroy(lastFallingZone.gameObject);
            }

            lastFallingZone = currentFallingZone;
            lastMiddlePosFall = currentMiddlePosFall;
            currentFallingZone = SpawnPlatform(fallingZone, new Vector3(lastMiddlePosFall.x + 60, lastMiddlePosFall.y));
        }
    }
}
