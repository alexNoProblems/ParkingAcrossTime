using System;
using UnityEngine;

public class StickmenLevelProgression
{
    private const int TotalLevels = 45;
    private const int StartStickmen = 20;
    private const int MaxStickmen = 390;


    public int GetStickmenCount(int level)
    {
        if (level <= 1)
            return StartStickmen;

        if (level >= TotalLevels)
            return MaxStickmen;

       float progress = CalculateProgress(level);
       float interpolatedCount = Mathf.Lerp(StartStickmen, MaxStickmen, progress);
       
       return RoundToEven(interpolatedCount);
    }

    private float CalculateProgress(int level)
    {
        int levelsCompleted = level - 1;
        int totalLevelSteps = TotalLevels - 1;
        return (float)levelsCompleted / totalLevelSteps;
    }

    private int RoundToEven(float value)
    {
        int rounded = Mathf.RoundToInt(value);

        if (rounded % 2 != 0)
            rounded += 1;
        
        return rounded;
    }
}
