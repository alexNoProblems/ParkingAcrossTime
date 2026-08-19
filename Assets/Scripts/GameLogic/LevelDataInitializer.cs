using System.Collections.Generic;
using UnityEngine;

public class LevelDataInitializer : MonoBehaviour
{
    [SerializeField] private int currentLevel = 1;
    
    private StickmenLevelDataGenerator _stickmenLevelDataGenerator;

    private void Awake()
    {
        _stickmenLevelDataGenerator = new StickmenLevelDataGenerator(new StickmenLevelProgression(),
            new ColorLevelProgression(), new StickmenColorDistributor(), new BusCapacitySolver());
    }

    private void Start()
    {
        Dictionary<StickmanColor, StickmenLevelDataGenerator.LevelColorData> levelData = 
            _stickmenLevelDataGenerator.GenerateLevelData(currentLevel);

        foreach (var (color, data) in levelData)
            Debug.Log($"{color}: {data.StickmenCount} стикманов, автобусы: {string.Join(", ", data.BusCapacities)}");
    }
}
