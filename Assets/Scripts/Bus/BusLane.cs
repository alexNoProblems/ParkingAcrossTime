using System.Collections.Generic;
using UnityEngine;

public class BusLane : MonoBehaviour
{
    [SerializeField] private List<BusPrefabEntry> _busPrefabs;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private List<Transform> _parkingSlots;

    private readonly List<Bus> _busesInLane = new List<Bus>();
    private readonly Queue<BusRequest> _pendingRequests = new Queue<BusRequest>();

    public int SlotCount => _parkingSlots.Count;
    public int PendingAndActiveCount => _busesInLane.Count + _pendingRequests.Count;
    private bool HasFreeSlot => _busesInLane.Count < _parkingSlots.Count;
    
    public void FillInstant(BusRequest request)
    {
        if (!HasFreeSlot)
        {
            Enqueue(request);

            return;
        }

        int targetSlotIndex = _busesInLane.Count;
        Vector3 slotPosition = _parkingSlots[targetSlotIndex].position;

        SpawnBus(request, slotPosition, slotPosition, startMoving: false);
    }

    public void Enqueue(BusRequest request)
    {
        _pendingRequests.Enqueue(request);

        TrySpawnNext();
    }

    public void DepartFront()
    {
        if (_busesInLane.Count == 0)
            return;

        Bus frontBus = _busesInLane[0];
        _busesInLane.RemoveAt(0);

        // TODO: отправить frontBus по маршруту выезда со сцены, а не удалять сразу
        Destroy(frontBus.gameObject);

        ShiftBusesForward();
        TrySpawnNext();
    }

    private void TrySpawnNext()
    {
        if (!HasFreeSlot || _pendingRequests.Count == 0)
            return;

        BusRequest request = _pendingRequests.Dequeue();
        int targetSlotIndex = _busesInLane.Count;
        SpawnBus(request, _spawnPoint.position, _parkingSlots[targetSlotIndex].position, startMoving: true);
    }

    private void SpawnBus(BusRequest request, Vector3 startPosition, Vector3 targetPosition, bool startMoving)
    {
        Bus prefab = FindPrefabForCapacity(request.Capacity);
        Bus bus = Instantiate(prefab, startPosition, _spawnPoint.rotation);

        bus.Initialize(request, startPosition, targetPosition);

        if (startMoving)
            bus.Mover.StartMoving();

        _busesInLane.Add(bus);
    }

    private Bus FindPrefabForCapacity(int capacity)
    {
        foreach (BusPrefabEntry entry in _busPrefabs)
        {
            if (entry.Capacity == capacity)
                return entry.Prefab;
        }

        Debug.LogError($"Не найден префаб автобуса на {capacity} мест");

        return _busPrefabs[0].Prefab;
    }

    private void ShiftBusesForward()
    {
        for (int i = 0; i < _busesInLane.Count; i++)
            _busesInLane[i].Mover.SetTarget(_parkingSlots[i].position);
    }
}