using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StickmenColorDistributor
{
    private const int RemainderStep = 2;
    
    public Dictionary<StickmanColor, int> Distribute(int totalStickmen, List<StickmanColor> colors)
    {
        int colorCount = colors.Count;
        int baseCount = totalStickmen / colorCount;

        if (baseCount % 2 != 0)
            baseCount -= 1;
        
        var result = colors.ToDictionary(color => color, color => baseCount);

        int remainder = totalStickmen - baseCount * colorCount;
        var shuffledColors = colors.OrderBy(_ => Random.value).ToList();
        int index = 0;

        while (remainder > 0)
        {
            result[shuffledColors[index % colorCount]] += RemainderStep;
            remainder -= RemainderStep;
            index++;
        }
        
        return result;
    }
}
