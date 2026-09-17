using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusPatrolManager : MonoBehaviour
{
    [SerializeField] private BusBoarding _boarding;
    [SerializeField] private int _maxConcurrentPatrols = 3;
    [SerializeField] private float _moveSpeed = 4f;
    [SerializeField] private float _minSpacing = 2f;
    [SerializeField] private float _boardingCheckpointRadius = 0.3f;
 
    private readonly List<IBusMovementState> _activeStates = new List<IBusMovementState>();
    private readonly Dictionary<Bus, int> _priorityOrderByBus = new Dictionary<Bus, int>();
    private int _nextPriorityOrder;
    private Bus _boardingSlotOccupant;
 
    public bool HasFreeSlot => CountActivePatrols() < _maxConcurrentPatrols;

    private void Update()
    {
        for (int i = _activeStates.Count - 1; i >= 0; i--)
        {
           IBusMovementState state = _activeStates[i];

           if (!state.IsActive)
           {
               _activeStates.RemoveAt(i);
               
               continue;
           }

           if (state is BusPatrolState patrol)
           {
               if (patrol.IsBoarding)
                   continue;
               
               if (TryHandleBoardingCheckpoint(patrol))
                   continue;
           }
                
            state.Tick(Time.deltaTime, _activeStates, _minSpacing);

            if (state is BusPathState pathState && pathState.IsComplete)
                pathState.Complete();
        }
    }

    public void StartPatrolling(Bus bus, BusRoute route, IReadOnlyList<Vector3> entryWaypoints = null)
    {
        var state = new BusPatrolState(bus, _moveSpeed, bus.ModelForwardOffsetY, GetOrAssignPriorityOrder(bus));

        state.BeginPatrol(route, entryWaypoints);
        
        _activeStates.Add(state);
    }

    public void StopPatrolling(Bus bus)
    {
        foreach (IBusMovementState state in _activeStates)
        {
            if (state.Bus != bus || !(state is BusPatrolState patrol))
                continue;

            patrol.EndPatrol();

            return;
        }
    }
    
    public IEnumerator MoveAlongPath(Bus bus, IReadOnlyList<Vector3> path, int effectivePriority)
    {
        var state = new BusPathState(bus, path, _moveSpeed, bus.ModelForwardOffsetY, effectivePriority, GetOrAssignPriorityOrder(bus));

        _activeStates.Add(state);

        yield return new WaitUntil(() => !state.IsActive);
    }

    public IBusMovementState RegisterStationary(Bus bus, int effectivePriority)
    {
        var state = new BusStationaryState(bus, effectivePriority, GetOrAssignPriorityOrder(bus));

        _activeStates.Add(state);

        return state;
    }

    private int GetOrAssignPriorityOrder(Bus bus)
    {
        if (!_priorityOrderByBus.TryGetValue(bus, out int priorityOrder))
        {
            priorityOrder = _nextPriorityOrder;
            _nextPriorityOrder++;
            _priorityOrderByBus[bus] = priorityOrder;
        }

        return priorityOrder;
    }

    public void UnregisterStationary(IBusMovementState state)
    {
        if (state is BusStationaryState stationary)
            stationary.Complete();
    }

    public bool TryAcquireBoardingSlot(Bus bus)
    {
        if (_boardingSlotOccupant != null && _boardingSlotOccupant != bus)
            return false;

        _boardingSlotOccupant = bus;

        return true;
    }

    public void ReleaseBoardingSlot(Bus bus)
    {
        if (_boardingSlotOccupant == bus)
            _boardingSlotOccupant = null;
    }
    
    private int CountActivePatrols()
    {
        int count = 0;

        foreach (IBusMovementState state in _activeStates)
        {
            if (state is BusPatrolState)
                count++;
        }

        return count;
    }

    private bool TryHandleBoardingCheckpoint(BusPatrolState patrol)
    {
        float distance = Vector3.Distance(patrol.Bus.transform.position, _boarding.BoardingPoint.position);
        bool atCheckpoint = distance <= _boardingCheckpointRadius;

        if (!atCheckpoint)
        {
            patrol.SetReachedCheckpoint(false);
            
            return false;
        }

        if (patrol.HasReachedCheckpointThisPass || !_boarding.ShouldBoard(patrol.Bus))
            return false;

        if (!TryAcquireBoardingSlot(patrol.Bus))
            return false;
        
        patrol.SetReachedCheckpoint(true);
        patrol.SetIsBoarding(true);
        
        StartCoroutine(BoardDuringPatrol(patrol));
        
        return true;
    }

    private IEnumerator BoardDuringPatrol(BusPatrolState patrol)
    {
        Bus bus = patrol.Bus;

        yield return _boarding.BoardAvailableStickmen(bus);

        ReleaseBoardingSlot(bus);

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