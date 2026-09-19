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
    private readonly PriorityOrderRegistry _priorityOrderRegistry = new PriorityOrderRegistry();
   
    private PatrolBoardingCoordinator _boardingCoordinator;
 
    public bool HasFreeSlot => CountActivePatrols() < _maxConcurrentPatrols;

    private void Awake()
    {
        _boardingCoordinator = new PatrolBoardingCoordinator(_boarding, _boardingCheckpointRadius);
    }

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
               
               if (_boardingCoordinator.TryHandleCheckpoint(patrol, this))
                   continue;
           }
                
           state.Tick(Time.deltaTime, _activeStates, _minSpacing);

           if (state is BusPathState pathState && pathState.IsComplete)
               pathState.Complete();
        }
    }

    public void StartPatrolling(Bus bus, BusRoute route, IReadOnlyList<Vector3> entryWaypoints = null)
    {
        var state = new BusPatrolState(bus, _moveSpeed, bus.ModelForwardOffsetY, _priorityOrderRegistry.GetOrAssign(bus));

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
        var state = new BusPathState(bus, path, _moveSpeed, bus.ModelForwardOffsetY, effectivePriority, _priorityOrderRegistry.GetOrAssign(bus));

        _activeStates.Add(state);

        yield return new WaitUntil(() => !state.IsActive);
    }

    public IBusMovementState RegisterStationary(Bus bus, int effectivePriority)
    {
        var state = new BusStationaryState(bus, effectivePriority, _priorityOrderRegistry.GetOrAssign(bus));

        _activeStates.Add(state);

        return state;
    }

    public void UnregisterStationary(IBusMovementState state)
    {
        if (state is BusStationaryState stationary)
            stationary.Complete();
    }

    public bool TryAcquireBoardingSlot(Bus bus) => _boardingCoordinator.TryAcquireSlot(bus);
    public void ReleaseBoardingSlot(Bus bus) => _boardingCoordinator.ReleaseSlot(bus);
    
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
}