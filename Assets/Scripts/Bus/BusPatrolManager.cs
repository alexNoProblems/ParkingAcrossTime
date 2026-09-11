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
 
    private readonly List<BusPatrolState> _activePatrols = new List<BusPatrolState>();
 
    public bool HasFreeSlot => _activePatrols.Count < _maxConcurrentPatrols;

    private void Update()
    {
        for (int i = _activePatrols.Count - 1; i >= 0; i--)
        {
            BusPatrolState patrol = _activePatrols[i];

            if (!patrol.IsActive)
                continue;

            if (patrol.IsBoarding)
                continue;

            if (TryHandleBoardingCheckpoint(patrol))
                continue;

            if (patrol.IsBlockedAhead(_activePatrols, _minSpacing))
                continue;
                
            patrol.Tick(Time.deltaTime);
        }
    }

    public void StartPatrolling(Bus bus, BusRoute route, IReadOnlyList<Vector3> entryWaypoints = null)
    {
        var state = new BusPatrolState(bus, _moveSpeed, bus.ModelForwardOffsetY);
        state.BeginPatrol(route, entryWaypoints);
        
        _activePatrols.Add(state);
    }

    public void StopPatrolling(Bus bus)
    {
        BusPatrolState state = _activePatrols.Find(patrol => patrol.Bus == bus);
        
        if (state == null)
            return;
        
        state.EndPatrol();
        _activePatrols.Remove(state);
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
        
        patrol.SetReachedCheckpoint(true);
        patrol.SetIsBoarding(true);
        
        StartCoroutine(BoardDuringPatrol(patrol));
        
        return true;
    }

    private IEnumerator BoardDuringPatrol(BusPatrolState patrol)
    {
        Bus bus = patrol.Bus;

        yield return _boarding.BoardAvailableStickmen(bus);

        if (bus.Capacity.IsFull)
        {
            patrol.EndPatrol();
            _activePatrols.Remove(patrol);

            yield return _boarding.DepartFull(bus);
            
            yield break;
        }
        
        patrol.SetIsBoarding(false);
    }
}
