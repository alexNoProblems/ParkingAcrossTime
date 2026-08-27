using System.Collections.Generic;
using UnityEngine;

public class BusRoute : MonoBehaviour
{
    [SerializeField] private List<Transform> _waypoints;

    public IReadOnlyList<Vector3> GetWaypointPositions()
    {
        var positions = new List<Vector3>(_waypoints.Count);

        foreach (Transform waypoint in _waypoints)
            positions.Add(waypoint.position);
        
        return positions;
    }
}
