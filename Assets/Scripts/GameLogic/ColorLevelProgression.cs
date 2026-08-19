using System.Collections.Generic;
using System.Linq;

public class ColorLevelProgression
{
    private struct ColorStage
    {
        public readonly int LevelThreshold;
        public readonly int ColorCount;

        public ColorStage(int levelThreshold, int colorCount)
        {
            LevelThreshold = levelThreshold;
            ColorCount = colorCount;
        }
    }
    
    private readonly StickmanColor[] _colorOrder =
    {
        StickmanColor.Red,
        StickmanColor.Yellow,
        StickmanColor.Blue,
        StickmanColor.Green,
        StickmanColor.Purple
    };

    private readonly ColorStage[] _colorStages =
    {
        new ColorStage(levelThreshold: 1, colorCount: 2),
        new ColorStage(levelThreshold: 4, colorCount: 3),
        new ColorStage(levelThreshold: 18, colorCount: 4),
        new ColorStage(levelThreshold: 32, colorCount: 5),
    };

    public int GetColorCount(int level)
    {
        int count = _colorStages[0].ColorCount;

        foreach (var stage in _colorStages)
        {
            if (level >= stage.LevelThreshold)
                count = stage.ColorCount;
        }
        
        return count;
    }

    public List<StickmanColor> GetColorsForLevel(int level)
    {
        int count = GetColorCount(level);
        
        return _colorOrder.Take(count).ToList();
    }
}
