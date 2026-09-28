using System;
using System.Collections.Generic;
using UnityEngine;

public class BusRegistry : MonoBehaviour
{
    private readonly List<Bus> _buses = new List<Bus>();

    public event Action AllBusesLeft;

    public void Register(Bus bus)
    {
        _buses.Add(bus);
    }
    
    public void Unregister(Bus bus)
    {
        _buses.Remove(bus);
        
        if (_buses.Count == 0)
            AllBusesLeft?.Invoke();
    }

    public IReadOnlyList<Bus> GetAll()
    {
        return _buses;
    }
}
