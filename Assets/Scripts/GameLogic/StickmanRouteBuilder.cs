using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StickmanRouteBuilder
{
    private readonly RoutePath _routePath = new RoutePath();

    public List<Vector3> Route { get; }
    public float RouteTotalLength { get; }
    public float QueueStartDistance { get; }
    public float QueueTotalLength { get; }

    public StickmanRouteBuilder(Transform spawnPoint, IReadOnlyList<Transform> pathWaypoints,
        IReadOnlyList<Transform> queueWaypoints)
    {
        Route = BuildRoute(spawnPoint, pathWaypoints, queueWaypoints);
        RouteTotalLength = _routePath.GetTotalLength(Route);

        QueueStartDistance = CalculateQueueStartDistance(spawnPoint, pathWaypoints, queueWaypoints);
        QueueTotalLength = _routePath.GetTotalLength(queueWaypoints.Select(waypoint => waypoint.position).ToList());
    }

    private List<Vector3> BuildRoute(Transform spawnPoint, IReadOnlyList<Transform> pathWaypoints,
        IReadOnlyList<Transform> queueWaypoints)
    {
        var route = new List<Vector3> { spawnPoint.position };
        route.AddRange(pathWaypoints.Select(waypoint => waypoint.position));
        route.AddRange(queueWaypoints.Select(waypoint => waypoint.position));

        return route;
    }

    private float CalculateQueueStartDistance(Transform spawnPoint, IReadOnlyList<Transform> pathWaypoints,
        IReadOnlyList<Transform> queueWaypoints)
    {
        var corridor = new List<Vector3> { spawnPoint.position };
        corridor.AddRange(pathWaypoints.Select(waypoint => waypoint.position));
        corridor.Add(queueWaypoints[0].position);

        return _routePath.GetTotalLength(corridor);
    }
}
