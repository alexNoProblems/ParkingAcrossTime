using System.Collections.Generic;
using UnityEngine;

public class BusStationaryState : IBusMovementState
{
    public Bus Bus { get; }
    public bool IsActive { get; private set; } = true;
    public int EffectivePriority { get; }
    public int PriorityOrder { get; }
    public Vector3 Position => Bus.transform.position;
    public bool BlocksAllTraffic => true;

    public BusStationaryState(Bus bus, int effectivePriority, int priorityOrder)
    {
        Bus = bus;
        EffectivePriority = effectivePriority;
        PriorityOrder = priorityOrder;
    }

    public void Complete()
    {
        IsActive = false;
    }

    public void Tick(float deltaTime, IReadOnlyList<IBusMovementState> allStates, float minSpacing)
    {
    }
}