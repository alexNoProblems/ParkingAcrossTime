using System.Collections.Generic;
using UnityEngine;

public class BusSeats : MonoBehaviour
{
    [SerializeField] private List<Transform> _seatsPoints;

    public Transform GetSeat(int index)
    {
        return _seatsPoints[index];
    }
}
