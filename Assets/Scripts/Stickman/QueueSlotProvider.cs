using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QueueSlotProvider
{
    private readonly List<Vector3> _pathPoints;
    private readonly float _slotSpacing;

    public QueueSlotProvider(List<Transform> pathWaypoints, float slotSpacing)
    {
        _pathPoints = pathWaypoints.Select(waypoint => waypoint.position).ToList();
        _slotSpacing = slotSpacing;
    }

    public Vector3 GetSlotPosition(int index)
    {
        float targetDistance = index * _slotSpacing;
        float traveledDistance = 0f;

        for (int i = 0; i < _pathPoints.Count - 1; i++)
        {
            Vector3 segmentStart = _pathPoints[i];
            Vector3 segmentEnd = _pathPoints[i + 1];
            
            float segmentLength = Vector3.Distance(segmentStart, segmentEnd);

            if (traveledDistance + segmentLength >= targetDistance)
            {
                float remainingInSegment = targetDistance - traveledDistance;
                Vector3 direction = (segmentEnd - segmentStart).normalized;
                
                return segmentStart +  direction * remainingInSegment;
            }
            
            traveledDistance += segmentLength;
        }
        
        return _pathPoints[_pathPoints.Count - 1];
    }
}
