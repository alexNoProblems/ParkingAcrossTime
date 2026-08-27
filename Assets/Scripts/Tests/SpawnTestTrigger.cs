using System.Collections.Generic;
using UnityEngine;

public class SpawnTestTrigger : MonoBehaviour
{
    [SerializeField] private StickmenSpawner _spawner;
    [SerializeField] private BusSpawner _busSpawner;
    [SerializeField] private QueueStickmenCounter _queueStickmenCounter;
    [SerializeField] private List<BusRequest> _testBusRequests;
    [SerializeField] private StickmanColor _testColor = StickmanColor.Red;
    [SerializeField] private int _testCount = 6;

    private void Start()
    {
        _queueStickmenCounter.SetInitialCount(_testCount)
            ;
        StartCoroutine(_spawner.Spawn(new StickmanSpawnData
        {
            Color = _testColor,
            Count = _testCount
        }));
        
        _busSpawner.FillInitial(_testBusRequests);
    }
}