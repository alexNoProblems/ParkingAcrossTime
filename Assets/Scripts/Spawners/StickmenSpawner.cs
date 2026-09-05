using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StickmenSpawner : MonoBehaviour, ISpawner<StickmanSpawnData>
{
    [SerializeField] private GameObject _stickmanPrefab;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private List<Transform> _pathWaypoints;
    [SerializeField] private List<Transform> _queueWaypoints;
    [SerializeField] private float _queueSlotSpacing = 1f;
    [SerializeField] private float _spawnInterval = 0.15f;

    private WaitForSeconds _waitForSeconds;
    private int _spawnedCount;

    private RoutePath routePath;
    private StickmanMover _lastSpawnedMover;
    private List<Vector3> _route;
    
    private float _routeTotalLength;
    private float _queueStartDistance;
    private float _queueTotalLength;

    private void Awake()
    {
        _waitForSeconds = new WaitForSeconds(_spawnInterval);
        routePath = new RoutePath();

        _route = BuildRoute();
        _routeTotalLength = routePath.GetTotalLength(_route);

        _queueStartDistance = CalculateQueueStartDistance();
        _queueTotalLength = routePath.GetTotalLength(_queueWaypoints.Select(w => w.position).ToList());
    }

    public IEnumerator Spawn(StickmanSpawnData data)
    {
        int maxCapacity = Mathf.FloorToInt(_queueTotalLength / _queueSlotSpacing) + 1;

        for (int i = 0; i < data.Count; i++)
        {
            if (_spawnedCount >= maxCapacity)
                yield break;

            var stickmanObject = Instantiate(_stickmanPrefab, _spawnPoint.position, Quaternion.identity);

            if (stickmanObject.TryGetComponent<Stickman>(out var stickman))
            {
                float maxDistance = _lastSpawnedMover == null ? _queueStartDistance : _routeTotalLength;

                stickman.Initialize(data.Color, _route, _lastSpawnedMover, _queueSlotSpacing, maxDistance);

                _lastSpawnedMover = stickman.Mover;
            }
            else
            {
                Debug.LogError($"На префабе {_stickmanPrefab.name} отсутствует компонент Stickman", stickmanObject);
            }

            _spawnedCount++;

            yield return _waitForSeconds;
        }
    }

    private List<Vector3> BuildRoute()
    {
        var route = new List<Vector3> { _spawnPoint.position };
        route.AddRange(_pathWaypoints.Select(w => w.position));
        route.AddRange(_queueWaypoints.Select(w => w.position));

        return route;
    }

    private float CalculateQueueStartDistance()
    {
        var corridor = new List<Vector3> { _spawnPoint.position };
        corridor.AddRange(_pathWaypoints.Select(w => w.position));
        corridor.Add(_queueWaypoints[0].position);

        return routePath.GetTotalLength(corridor);
    }
}