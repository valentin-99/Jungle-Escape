using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SoloData
{
    public int score1, score2, score3, score4, score5, score6;
    public int time1, time2, time3, time4, time5, time6;
    public int record1, record2, record3, record4, record5, record6;

    public SoloData (PlayerController player)
    {  
        score1 = player.score1;
        score2 = player.score2;
        score3 = player.score3;
        score4 = player.score4;
        score5 = player.score5;
        score6 = player.score6;
        time1 = player.time1;
        time2 = player.time2;
        time3 = player.time3;
        time4 = player.time4;
        time5 = player.time5;
        time6 = player.time6;
        record1 = player.record1;
        record2 = player.record2;
        record3 = player.record3;
        record4 = player.record4;
        record5 = player.record5;
        record6 = player.record6;
    }
}
