using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorShuffleRoutine 
{
    public IEnumerator Run(IReadOnlyList<IColorFlickerTarget> targets, IReadOnlyList<StickmanColor> flickerColors, 
        IReadOnlyDictionary<IColorFlickerTarget, StickmanColor> finalColors, float duration, float interval)
    {
        var waitForsecond = new WaitForSeconds(interval);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            foreach (IColorFlickerTarget target in targets)
                target.FlickerColor(flickerColors[Random.Range(0, flickerColors.Count)]);
            
            yield return waitForsecond;

            elapsed += interval;
        }

        foreach (IColorFlickerTarget target in targets)
            target.SetColor(finalColors[target]);
    }
}
