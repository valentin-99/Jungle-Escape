using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoloData
{
    public int record1, record2, record3, record4, record5, record6;

    public SoloData (PlayerController player)
    {
        record1 = player.record1;
        record2 = player.record2;
        record3 = player.record3;
        record4 = player.record4;
        record5 = player.record5;
        record6 = player.record6;
    }
}
