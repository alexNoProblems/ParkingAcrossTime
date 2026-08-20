using UnityEngine;

public class SpawnTestTrigger : MonoBehaviour
{
    [SerializeField] private StickmenSpawner spawner;
    [SerializeField] private StickmanColor testColor = StickmanColor.Red;
    [SerializeField] private int testCount = 6;

    private void Start()
    {
        StartCoroutine(spawner.Spawn(new StickmanSpawnData
        {
            Color = testColor,
            Count = testCount
        }));
    }
}