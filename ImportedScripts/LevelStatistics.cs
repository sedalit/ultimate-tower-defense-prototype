using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStatistics : MonoBehaviour
{
    public int numKills;
    public int score;
    public int time;

    public void Reset()
    {
        numKills = 0;
        score = 0;
        time = 0;
    }
}
