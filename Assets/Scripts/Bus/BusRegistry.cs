using System.Collections.Generic;
using UnityEngine;

public class BusRegistry : MonoBehaviour
{
    private readonly List<Bus> _buses = new List<Bus>();

    public void Register(Bus bus)
    {
        _buses.Add(bus);
    }
    
    public void Unregister(Bus bus)
    {
        _buses.Remove(bus);
    }

    public IReadOnlyList<Bus> GetAll()
    {
        return _buses;
    }
}
