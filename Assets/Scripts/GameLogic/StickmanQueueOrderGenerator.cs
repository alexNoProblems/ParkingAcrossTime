using System.Collections.Generic;
using UnityEngine;

public class StickmanQueueOrderGenerator
{
    private readonly int _minRunLength;
    private readonly int _maxRunLength;

    public StickmanQueueOrderGenerator(int minRunLength = 2, int maxRunLength = 10)
    {
        _minRunLength = minRunLength;
        _maxRunLength = maxRunLength;
    }

    public List<(StickmanColor Color, int Count)> GenerateOrder(Dictionary<StickmanColor, int> colorCounts)
    {
        var remaining = new Dictionary<StickmanColor, int>(colorCounts);
        var order = new List<(StickmanColor, int)>();
        StickmanColor? lastColor = null;

        while (HasRemaining(remaining))
        {
            StickmanColor color = PickRandomColor(remaining, lastColor);
            int runLength = PickRunLength(remaining[color]);
            
            order.Add((color, runLength));
            remaining[color] -= runLength;
            lastColor = color;
        }
        
        return order;
    }

    private int PickRunLength(int remainingForColor)
    {
        int capacity = Mathf.Min(_maxRunLength, remainingForColor);
        int lowerBound = Mathf.Min(_minRunLength, capacity);
        
        return Random.Range(lowerBound, capacity + 1);
    }

    private bool HasRemaining(Dictionary<StickmanColor, int> remaining)
    {
        foreach (int count in remaining.Values)
        {
            if (count > 0)
                return true;
        }
        
        return false;
    }

    private StickmanColor PickRandomColor(Dictionary<StickmanColor, int> remaining, StickmanColor? excludeColor)
    {
        var candidates = new List<StickmanColor>();

        foreach (var pair in remaining)
        {
            if (pair.Value > 0 && pair.Key != excludeColor)
                candidates.Add(pair.Key);
        }

        if (candidates.Count == 0)
        {
            foreach (var pair in remaining)
            {
                if(pair.Value > 0)
                    candidates.Add(pair.Key);
            }
        }
        
        return candidates[Random.Range(0, candidates.Count)];
    }
}
