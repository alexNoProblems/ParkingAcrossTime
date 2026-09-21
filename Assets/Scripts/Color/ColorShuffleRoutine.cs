using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorShuffleRoutine 
{
    private readonly StickmanColor[] _allColors = (StickmanColor[])Enum.GetValues(typeof(StickmanColor));

    public IEnumerator Run(IReadOnlyList<IColorFlickerTarget> targets, float duration, float interval)
    {
        var waitForsecond = new WaitForSeconds(interval);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            foreach (IColorFlickerTarget target in targets)
                target.FlickerColor(RandomColor());
            
            yield return waitForsecond;

            elapsed += interval;
        }

        foreach (IColorFlickerTarget target in targets)
            target.SetColor(RandomColor());
    }

    private StickmanColor RandomColor()
    {
        return _allColors[UnityEngine.Random.Range(0, _allColors.Length)];
    }
}
