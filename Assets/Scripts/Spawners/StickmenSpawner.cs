using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickmenSpawner : MonoBehaviour, ISpawner<StickmanSpawnData>
{
    [SerializeField] private GameObject _stickmanPrefab;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private StickmanQueue _stickmanQueue;
    [SerializeField] private List<Transform> _pathWaypoints;
    [SerializeField] private List<Transform> _queueWaypoints;
    [SerializeField] private float _queueSlotSpacing = 1f;
    [SerializeField] private float _spawnInterval = 0.15f;

    private WaitForSeconds _waitForSeconds;
    private int _spawnedCount;
    
    private StickmanMover _lastSpawnedMover;
    private StickmanRouteBuilder _routeBuilder;

    private void Awake()
    {
        _waitForSeconds = new WaitForSeconds(_spawnInterval);
        _routeBuilder = new StickmanRouteBuilder(_spawnPoint, _pathWaypoints, _queueWaypoints);
        
        _stickmanQueue.Initialize(_routeBuilder.QueueStartDistance);
    }

    public IEnumerator Spawn(StickmanSpawnData data)
    {
        int maxCapacity = Mathf.FloorToInt(_routeBuilder.QueueTotalLength / _queueSlotSpacing) + 1;

        for (int i = 0; i < data.Count; i++)
        {
            if (_spawnedCount >= maxCapacity)
                yield break;

            var stickmanObject = Instantiate(_stickmanPrefab, _spawnPoint.position, Quaternion.identity);

            if (stickmanObject.TryGetComponent<Stickman>(out var stickman))
            {
                float maxDistance = _lastSpawnedMover == null ? _routeBuilder.QueueStartDistance : _routeBuilder.RouteTotalLength;

                stickman.Initialize(data.Color, _routeBuilder.Route, _lastSpawnedMover, _queueSlotSpacing, maxDistance);

                _lastSpawnedMover = stickman.Mover;
                _stickmanQueue.Register(stickman);
            }
            else
            {
                Debug.LogError($"На префабе {_stickmanPrefab.name} отсутствует компонент Stickman", stickmanObject);
            }

            _spawnedCount++;

            yield return _waitForSeconds;
        }
    }
}