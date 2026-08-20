using System.Collections.Generic;
using UnityEngine;

public class StickmanPath 
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

        float traveled = 0f;

        for (int i = 0; i < points.Count - 1; i++)
        {
            Vector3 segmentStart = points[i];
            Vector3 segmentEnd = points[i + 1];
            float segmentLength = Vector3.Distance(segmentStart, segmentEnd);

            if (traveled + segmentLength >= distance)
            {
                float remaining = distance - traveled;
                Vector3 direction = segmentLength > 0f ? (segmentEnd - segmentStart) / segmentLength : Vector3.zero;
                
                return segmentStart + direction * remaining;
            }
            
            traveled += segmentLength;
        }
        
        return points[points.Count - 1];
    }
}
