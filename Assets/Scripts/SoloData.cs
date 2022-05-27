using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SoloData
{
    public int[] scoreArr;
    public float[] timeArr;
    public int[] totalScoreArr;
    public int levelUnlocked;

    public SoloData (PlayerController player)
    {
        scoreArr = new int[6];
        timeArr = new float[6];
        totalScoreArr = new int[6];

        /*scoreArr = player.scoreArr;
        timeArr = player.timeArr;
        levelUnlocked = player.levelUnlocked;*/
    }
}
