using System.Collections;
using UnityEngine;

public class PatrolBoardingCoordinator
{
    private readonly BusBoarding _boarding;
    private readonly float _checkpointRadius;

    private Bus _slotOcсupant;

    public PatrolBoardingCoordinator(BusBoarding boarding, float checkpointRadius)
    {
        _boarding = boarding;
        _checkpointRadius = checkpointRadius;
    }
    
    public bool TryHandleCheckpoint(BusPatrolState patrol, MonoBehaviour coroutineRunner)
    {
        float distance = Vector3.Distance(patrol.Bus.transform.position, _boarding.BoardingPoint.position);
        bool atCheckpoint = distance <= _checkpointRadius;

        if (!atCheckpoint)
        {
            patrol.SetReachedCheckpoint(false);
            
            return false;
        }

        if (patrol.HasReachedCheckpointThisPass || !_boarding.ShouldBoard(patrol.Bus))
            return false;

        if (!TryAcquireSlot(patrol.Bus))
            return false;
        
        patrol.SetReachedCheckpoint(true);
        patrol.SetIsBoarding(true);
        
        coroutineRunner.StartCoroutine(BoardDuringPatrol(patrol));
        
        return true;
    }
    
    public bool TryAcquireSlot(Bus bus)
    {
        if (_slotOcсupant != null && _slotOcсupant != bus)
            return false;
        
        _slotOcсupant = bus;
        
        return true;
    }
    
    public void ReleaseSlot(Bus bus)
    {
        if (_slotOcсupant == bus)
            _slotOcсupant = null;
    }

    private IEnumerator BoardDuringPatrol(BusPatrolState patrol)
    {
        Bus bus = patrol.Bus;

        yield return _boarding.BoardAvailableStickmen(bus);

        ReleaseSlot(bus);

        if (bus.Capacity.IsFull)
        {
            patrol.EndPatrol();
            bus.PlayExhaustEffect();

            yield return _boarding.DepartFull(bus);
            
            yield break;
        }
        
        bus.PlayExhaustEffect();
        patrol.SetIsBoarding(false);
    }
}
