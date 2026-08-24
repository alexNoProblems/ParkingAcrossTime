using UnityEngine;

public class BusCapacity : MonoBehaviour
{
    public int Capacity { get; private set; }
    public int SeatedCount { get; private set; }
    public bool IsFull => SeatedCount >= Capacity;

    public void Initialize(int capacity)
    {
        Capacity = capacity;
        SeatedCount = 0;
    }

    public bool TryBoard()
    {
        if (IsFull)
            return false;
        
        SeatedCount++;
        
        return true;
    }
}
