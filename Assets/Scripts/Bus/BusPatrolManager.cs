using System.Collections.Generic;
using UnityEngine;

public class BusPatrolManager : MonoBehaviour
{
    [SerializeField] private int _maxConcurrentPatrols = 3;
    [SerializeField] private float _moveSpeed = 4f;
    [SerializeField] private float _minSpacing = 2f;
 
    private readonly List<BusPatrolState> _activePatrols = new List<BusPatrolState>();
 
    public bool HasFreeSlot => _activePatrols.Count < _maxConcurrentPatrols;

    private void Update()
    {
        foreach (BusPatrolState patrol in _activePatrols)
        {
            if (!patrol.IsActive)
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
}
