using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct StickmanSpawnData
{
    public StickmanColor Color;
    public int Count;
}

public class StickmenSpawner : MonoBehaviour, ISpawner<StickmanSpawnData>
{
    [SerializeField] private GameObject stickmanPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private List<Transform> pathWaypoints;
    [SerializeField] private List<Transform> queueWaypoints;
    [SerializeField] private float queueSlotSpacing = 1f;
    [SerializeField] private float spawnInterval = 0.15f;

    private WaitForSeconds _waitForSeconds;
    private int _spawnedCount;

    private StickmanPath _stickmanPath;
    private StickmanMover _lastSpawnedMover;
    private List<Vector3> _route;
    
    private float _routeTotalLength;
    private float _queueStartDistance;
    private float _queueTotalLength;

    private void Awake()
    {
        _waitForSeconds = new WaitForSeconds(spawnInterval);
        _stickmanPath = new StickmanPath();

        _route = BuildRoute();
        _routeTotalLength = _stickmanPath.GetTotalLength(_route);

        _queueStartDistance = CalculateQueueStartDistance();
        _queueTotalLength = _stickmanPath.GetTotalLength(queueWaypoints.Select(w => w.position).ToList());
    }

    public IEnumerator Spawn(StickmanSpawnData data)
    {
        int maxCapacity = Mathf.FloorToInt(_queueTotalLength / queueSlotSpacing) + 1;

        for (int i = 0; i < data.Count; i++)
        {
            if (_spawnedCount >= maxCapacity)
                yield break;

            var stickmanObject = Instantiate(stickmanPrefab, spawnPoint.position, Quaternion.identity);

            if (stickmanObject.TryGetComponent<Stickman>(out var stickman))
            {
                float maxDistance = _lastSpawnedMover == null ? _queueStartDistance : _routeTotalLength;

                stickman.Initialize(data.Color, _route, _lastSpawnedMover, queueSlotSpacing, maxDistance);

                _lastSpawnedMover = stickman.Mover;
            }
            else
            {
                Debug.LogError($"На префабе {stickmanPrefab.name} отсутствует компонент Stickman", stickmanObject);
            }

            _spawnedCount++;

            yield return _waitForSeconds;
        }
    }

    private List<Vector3> BuildRoute()
    {
        var route = new List<Vector3> { spawnPoint.position };
        route.AddRange(pathWaypoints.Select(w => w.position));
        route.AddRange(queueWaypoints.Select(w => w.position));

        return route;
    }

    private float CalculateQueueStartDistance()
    {
        var corridor = new List<Vector3> { spawnPoint.position };
        corridor.AddRange(pathWaypoints.Select(w => w.position));
        corridor.Add(queueWaypoints[0].position);

        return _stickmanPath.GetTotalLength(corridor);
    }
}