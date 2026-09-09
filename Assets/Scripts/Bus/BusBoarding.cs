using System.Collections;
using UnityEngine;

public class BusBoarding : MonoBehaviour
{
    [SerializeField] private Transform _boardingPoint;
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
        
        bus.PlayExhaustEffect();
        _patrolManager.StartPatrolling(bus, _busRoute);
    }

    private IEnumerator BoardStickmen(Bus bus)
    {
        bus.Mover.SetTarget(_boardingPoint.position);
        
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

    private bool ShouldBoard(Bus bus)
    {
        Stickman front = _queue.PeekFront();
        
        return front != null && front.Color == bus.Color && !bus.Capacity.IsFull;
    }
}
