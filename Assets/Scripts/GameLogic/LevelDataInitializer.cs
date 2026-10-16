using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class LevelDataInitializer : MonoBehaviour
{
    [SerializeField] private StickmenSpawner _stickmenSpawner;
    [SerializeField] private BusSpawner _busSpawner;
    [SerializeField] private QueueStickmenCounter _queueStickmenCounter;
    [SerializeField] private int _minColorRunLength = 2;
    [SerializeField] private int _maxColorRunLength = 10;
    
    private StickmenLevelDataGenerator _stickmenLevelDataGenerator;

    public int CurrentLevel => Mathf.Max(1, YG2.saves.level);

    private void Awake()
    {
        _stickmenLevelDataGenerator = new StickmenLevelDataGenerator(new StickmenLevelProgression(),
            new ColorLevelProgression(), new StickmenColorDistributor(), new BusCapacitySolver());
    }

    private void Start()
    {
        int level = CurrentLevel;
        
        Dictionary<StickmanColor, StickmenLevelDataGenerator.LevelColorData> levelData =
            _stickmenLevelDataGenerator.GenerateLevelData(level);
 
        int totalStickmen = 0;
        var busRequests = new List<BusRequest>();
        var colorCounts = new Dictionary<StickmanColor, int>();
 
        foreach (var (color, data) in levelData)
        {
            totalStickmen += data.StickmenCount;
            colorCounts[color] = data.StickmenCount;
 
            foreach (int capacity in data.BusCapacities)
                busRequests.Add(new BusRequest { Color = color, Capacity = capacity });
        }
 
        var orderGenerator = new StickmanQueueOrderGenerator(_minColorRunLength, _maxColorRunLength);
        List<(StickmanColor Color, int Count)> spawnOrder = orderGenerator.GenerateOrder(colorCounts);

        if (spawnOrder.Count == 0)
            return;
        
        _queueStickmenCounter.SetInitialCount(totalStickmen);
        _busSpawner.FillInitial(busRequests, spawnOrder[0].Color);

        StartCoroutine(SpawnStickmenSequentially(spawnOrder));
    }
 
    private IEnumerator SpawnStickmenSequentially(List<(StickmanColor Color, int Count)> spawnOrder)
    {
        foreach (var (color, count) in spawnOrder)
        {
            yield return StartCoroutine(_stickmenSpawner.Spawn(new StickmanSpawnData
            {
                Color = color,
                Count = count
            }));
        }
    }
}
