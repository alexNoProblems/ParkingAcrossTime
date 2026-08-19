using System.Collections.Generic;

public class StickmenLevelDataGenerator
{
    private readonly StickmenLevelProgression _stickmenProgression;
    private readonly ColorLevelProgression _colorProgression;
    private readonly StickmenColorDistributor _stickmenColorDistributor;
    private readonly BusCapacitySolver _busCapacitySolver;

    public StickmenLevelDataGenerator(StickmenLevelProgression stickmenProgression,
        ColorLevelProgression colorProgression, StickmenColorDistributor stickmenColorDistributor, 
        BusCapacitySolver busCapacitySolver)
    {
        _stickmenProgression = stickmenProgression;
        _colorProgression = colorProgression;
        _stickmenColorDistributor = stickmenColorDistributor;
        _busCapacitySolver = busCapacitySolver;
    }

    public Dictionary<StickmanColor, LevelColorData> GenerateLevelData(int level)
    {
        int totalStickmen = _stickmenProgression.GetStickmenCount(level);
        var colors = _colorProgression.GetColorsForLevel(level);
        var stickmenPerColor = _stickmenColorDistributor.Distribute(totalStickmen, colors);
        
        var levelData = new Dictionary<StickmanColor, LevelColorData>();

        foreach (var color in colors)
        {
            int stickmenCount = stickmenPerColor[color];
            List<int> busCapacities = _busCapacitySolver.SolveCapacities(stickmenCount);

            levelData[color] = new LevelColorData
            {
                StickmenCount = stickmenCount,
                BusCapacities = busCapacities
            };
        }
        
        return levelData;
    }
    
    public class LevelColorData
    {
        public int StickmenCount { get; set; }
        public List<int> BusCapacities { get; set; }
    }
}
