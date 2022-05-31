using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RunnerData
{
    public int record1, record2, record3;

    public RunnerData (PlayerControllerRunner player)
    {
        record1 = player.record1;
        record2 = player.record2;
        record3 = player.record3;
    }
}
