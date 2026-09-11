using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusBoarding : MonoBehaviour
{
    [SerializeField] private List<Transform> _routePoints;
    [SerializeField] private List<Transform> _exitRoutePoints;
    [SerializeField] private Transform _boardingPoint;
    [SerializeField] private Transform _exitPoint;
    [SerializeField] private StickmanQueue _queue;
    [SerializeField] private BusRoute _busRoute;
    [SerializeField] private BusPatrolManager _patrolManager;
    [SerializeField] private float _boardingInterval = 0.3f;
    
    private WaitForSeconds _boardingWait;
    
    public Transform BoardingPoint => _boardingPoint;

    private void Awake()
    {
        _boardingWait = new WaitForSeconds(_boardingInterval);
    }
    
    public void HandleReleasedBus(Bus bus)
    {
        StartCoroutine(ProcessBus(bus));
    }

    public IEnumerator BoardAvailableStickmen(Bus bus)
    {
        Stickman front = _queue.PeekFront();

        while (front != null && front.Color == bus.Color && !bus.Capacity.IsFull)
        {
            _queue.DequeueFront();
            bus.Seat(front);
            
            yield return _boardingWait;
            
            front = _queue.PeekFront();
        }
    }
    
    public IEnumerator DepartFull(Bus bus)
    {
        var path = new List<Vector3>(_exitRoutePoints.Count + 1);
        
        foreach (Transform point in _exitRoutePoints)
            path.Add(point.position);

        path.Add(_exitPoint.position);

        bus.Mover.SetPath(path);

        yield return null;

        yield return bus.Mover.StoppedWait;

        Destroy(bus.gameObject);
    }
    
    public bool ShouldBoard(Bus bus)
    {
        Stickman front = _queue.PeekFront();
        
        return front != null && front.Color == bus.Color && !bus.Capacity.IsFull;
    }
    
    private IEnumerator ProcessBus(Bus bus)
    { 
        bool boarded = ShouldBoard(bus);

        if (boarded)
            yield return BoardStickmen(bus);

        if (bus.Capacity.IsFull)
        {
            yield return DepartFull(bus);
        }
        else
        {
            List<Vector3> entryWaypoints = null;

            if (boarded)
            {
                entryWaypoints = new List<Vector3>(_exitRoutePoints.Count);

                foreach (Transform point in _exitRoutePoints)
                    entryWaypoints.Add(point.position);
            }

            _patrolManager.StartPatrolling(bus, _busRoute, entryWaypoints);
        }
    }

    private IEnumerator BoardStickmen(Bus bus)
    {
        var path = new List<Vector3>(_routePoints.Count + 1);
        
        foreach (Transform point in _routePoints)
            path.Add(point.position);
        
        path.Add(_boardingPoint.position);
        
        bus.Mover.SetPath(path);
        
        yield return null;

        yield return bus.Mover.StoppedWait;

        yield return BoardAvailableStickmen(bus);
    }
}
