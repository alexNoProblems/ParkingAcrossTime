using System.Collections.Generic;
using UnityEngine;

public class RoutePath 
{
    public float GetTotalLength(IReadOnlyList<Vector3> points)
    {
        float totalDistance = 0f;

        for (int i = 0; i < points.Count - 1; i++)
            totalDistance += Vector3.Distance(points[i], points[i + 1]);
        
        return totalDistance;
    }

    public Vector3 GetPointAtDistance(IReadOnlyList<Vector3> points, float distance)
    {
        if (points.Count == 0)
            return Vector3.zero;

        if (distance <= 0f)
            return points[0];

        if (!TryFindSegment(points, distance, out int segmentIndex, out float traveledBeforeSegment))
            return points[points.Count - 1];

        Vector3 segmentStart = points[segmentIndex];
        Vector3 segmentEnd = points[segmentIndex + 1];
        float segmentLength = Vector3.Distance(segmentStart, segmentEnd);
        float remaining = distance - traveledBeforeSegment;
        Vector3 direction = segmentLength > 0f ? (segmentEnd - segmentStart) / segmentLength : Vector3.zero;

        return segmentStart + direction * remaining;
    }

    public Vector3 GetDirectionAtDistance(IReadOnlyList<Vector3> points, float distance)
    {
        if (points.Count < 2)
            return Vector3.zero;

        if (!TryFindSegment(points, distance, out int segmentIndex, out _))
            segmentIndex = points.Count - 2;

        Vector3 segmentStart = points[segmentIndex];
        Vector3 segmentEnd = points[segmentIndex + 1];
        float segmentLength = Vector3.Distance(segmentStart, segmentEnd);

        return segmentLength > 0f ? (segmentEnd - segmentStart) / segmentLength : Vector3.zero;
    }

    private bool TryFindSegment(IReadOnlyList<Vector3> points, float distance, out int segmentIndex,
        out float traveledBeforeSegment)
    {
        float traveled = 0f;

        for (int i = 0; i < points.Count - 1; i++)
        {
            float segmentLength = Vector3.Distance(points[i], points[i + 1]);

            if (traveled + segmentLength >= distance)
            {
                segmentIndex = i;
                traveledBeforeSegment = traveled;

                return true;
            }

            traveled += segmentLength;
        }

        segmentIndex = -1;
        traveledBeforeSegment = 0f;

        return false;
    }
}