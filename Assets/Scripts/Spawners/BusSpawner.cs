using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusSpawner : MonoBehaviour, ISpawner<BusSpawnData>
{
    [SerializeField] private List<BusLane> _lanes;
    [SerializeField] private float _spawnInterval = 0.3f;
    
    private WaitForSeconds _waitForSeconds;

    private void Awake()
    {
        _waitForSeconds = new WaitForSeconds(_spawnInterval);
    }

    public void FillInitial(List<BusRequest> requests)
    {
        int totalSlots = 0;

        foreach (BusLane lane in _lanes)
            totalSlots += lane.SlotCount;
        
        int instantCount = Mathf.Min(requests.Count, totalSlots);

        for (int i = 0; i < instantCount; i++)
        {
            BusLane lane = _lanes[i % _lanes.Count];
            lane.FillInstant(requests[i]);
        }

        for (int i = instantCount; i < requests.Count; i++)
            RequestBus(requests[i]);
    }

    public IEnumerator Spawn(BusSpawnData data)
    {
        foreach (int capacity in data.Capacities)
        {
            RequestBus(new BusRequest { Color = data.Color, Capacity = capacity });

            yield return _waitForSeconds;
        }
    }

    private void RequestBus(BusRequest request)
    {
        BusLane leastLoadedLine = FindLeastLoadedLane();
        leastLoadedLine.Enqueue(request);
    }

    private BusLane FindLeastLoadedLane()
    {
        BusLane best = _lanes[0];

        foreach (BusLane lane in _lanes)
        {
            if (lane.PendingAndActiveCount < best.PendingAndActiveCount)
                best = lane;
        }
        
        return best;
    }
}
