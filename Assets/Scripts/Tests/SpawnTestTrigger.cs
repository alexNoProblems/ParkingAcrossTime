using UnityEngine;

public class SpawnTestTrigger : MonoBehaviour
{
    [SerializeField] private StickmenSpawner spawner;
    [SerializeField] private QueueStickmenCounter _queueStickmenCounter;
    [SerializeField] private StickmanColor testColor = StickmanColor.Red;
    [SerializeField] private int testCount = 6;

    private void Start()
    {
        _queueStickmenCounter.SetInitialCount(testCount)
            ;
        StartCoroutine(spawner.Spawn(new StickmanSpawnData
        {
            Color = testColor,
            Count = testCount
        }));
    }
}