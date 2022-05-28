using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RunnerData
{
    public int score1, score2, score3;
    public int record1, record2, record3;

    public RunnerData (PlayerControllerRunner player)
    {
        score1 = player.score1;
        score2 = player.score2;
        score3 = player.score3;
        record1 = player.record1;
        record2 = player.record2;
        record3 = player.record3;
    }
}
