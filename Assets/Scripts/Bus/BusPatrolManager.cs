using System.Collections.Generic;
using UnityEngine;

public class BusPatrolManager : MonoBehaviour
{
    [SerializeField] private int _maxConcurrentPatrols = 3;
 
    private readonly List<BusRoutePatrol> _activePatrols = new List<BusRoutePatrol>();
 
    public bool HasFreeSlot => _activePatrols.Count < _maxConcurrentPatrols;
 
    public void BusRegister(BusRoutePatrol patrol)
    {
        if (!_activePatrols.Contains(patrol))
            _activePatrols.Add(patrol);
    }
 
    public void BusUnregister(BusRoutePatrol patrol)
    {
        _activePatrols.Remove(patrol);
    }
}
