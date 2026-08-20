using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QueueSlotProvider
{
    private readonly List<Vector3> _pathPoints;
    private readonly float _slotSpacing;

    public QueueSlotProvider(List<Transform> combinedPath, float slotSpacing)
    {
        _pathPoints = combinedPath.Select(waypoint => waypoint.position).ToList();
        _slotSpacing = slotSpacing;
    }

    public List<Vector3> GetPathToSlot(int index)
    {
        float targetDistance = index * _slotSpacing;
        float traveledDistance = 0f;
        var result = new List<Vector3>();

        for (int i = 0; i < _pathPoints.Count - 1; i++)
        {
            Vector3 segmentStart = _pathPoints[i];
            Vector3 segmentEnd = _pathPoints[i + 1];

            float segmentLength = Vector3.Distance(segmentStart, segmentEnd);

            if (traveledDistance + segmentLength >= targetDistance)
            {
                float remainingInSegment = targetDistance - traveledDistance;
                Vector3 direction = (segmentEnd - segmentStart).normalized;

                result.Add(segmentStart + direction * remainingInSegment);
                return result;
            }

            result.Add(segmentEnd);
            traveledDistance += segmentLength;
        }

        result.Add(_pathPoints[_pathPoints.Count - 1]);
        
        return result;
    }
}