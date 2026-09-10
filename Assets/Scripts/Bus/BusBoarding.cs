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

    public void HandleReleasedBus(Bus bus)
    {
        StartCoroutine(ProcessBus(bus));
    }

    private IEnumerator ProcessBus(Bus bus)
    {
        if (ShouldBoard(bus))
            yield return BoardStickmen(bus);

        if (bus.Capacity.IsFull)
        {
            yield return DepartFull(bus);
        }
        else
        {
            var entryWaypoints = new List<Vector3>(_exitRoutePoints.Count);
            
            foreach (Transform point in _exitRoutePoints)
                entryWaypoints.Add(point.position); 
            
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

        Stickman front = _queue.PeekFront();

        while (front != null && front.Color == bus.Color && !bus.Capacity.IsFull)
        {
            _queue.DequeueFront();
            bus.Seat(front);
            front = _queue.PeekFront();
        }
    }

    private IEnumerator DepartFull(Bus bus)
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

    private bool ShouldBoard(Bus bus)
    {
        Stickman front = _queue.PeekFront();
        
        return front != null && front.Color == bus.Color && !bus.Capacity.IsFull;
    }
}
