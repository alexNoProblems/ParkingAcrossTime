using UnityEngine;

public class BusMovementGate : MonoBehaviour
{
    public bool IsPaused { get; private set; }

    public void Pause()
    {
        IsPaused = true;
    }

    public void Resume()
    {
        IsPaused = false;
    }
}
