using System.Collections.Generic;

public class PriorityOrderRegistry 
{
    private readonly Dictionary<Bus, int> _priorityOrderByBus = new Dictionary<Bus, int>();
    
    private int _nextPriorityOrder;
    
    public int GetOrAssign(Bus bus)
    {
        if (!_priorityOrderByBus.TryGetValue(bus, out int priorityOrder))
        {
            priorityOrder = _nextPriorityOrder;
            _nextPriorityOrder++;
            _priorityOrderByBus[bus] = priorityOrder;
        }

        return priorityOrder;
    }
}
