using System.Collections.Generic;
using UnityEngine;

public class BusPatrolState
{
    private const float MinMovementSqrMagnitude = 0.0001f;
    
    private readonly RoutePath _pathCalculator = new RoutePath();
    private readonly Transform _transform;
    private readonly MovementRotator _rotator = new MovementRotator();
    private readonly float _moveSpeed;
    private readonly float _modelForwardOffsetY;
 
    private List<Vector3> _entryPath;
    private List<Vector3> _loopRoute;
    private float _entryLength;
    private float _loopLength;
 
    public Bus Bus { get; }
    public float CurrentDistance { get; private set; }
    public bool HasEnteredLoop { get; private set; }
    public bool IsActive { get; private set; }
 
    public BusPatrolState(Bus bus, float moveSpeed, float modelForwardOffsetY)
    {
        Bus = bus;
        _transform = bus.transform;
        _moveSpeed = moveSpeed;
        _modelForwardOffsetY = modelForwardOffsetY;
    }
 
    public void BeginPatrol(BusRoute route, IReadOnlyList<Vector3> entryWaypoints = null)
    {
        var waypoints = new List<Vector3>(route.GetWaypointPositions());
 
        _entryPath = BuildEntryPath(waypoints, entryWaypoints);
        _entryLength = _pathCalculator.GetTotalLength(_entryPath);
 
        _loopRoute = BuildLoopRoute(waypoints);
        _loopLength = _pathCalculator.GetTotalLength(_loopRoute);
 
        CurrentDistance = 0f;
        HasEnteredLoop = false;
        IsActive = waypoints.Count > 0;
    }
 
    public void EndPatrol()
    {
        IsActive = false;
    }
 
    public bool IsBlockedAhead(IReadOnlyList<BusPatrolState> allPatrols, float minSpacing)
    {
        foreach (BusPatrolState other in allPatrols)
        {
            if (other == this || !other.IsActive)
                continue;
 
            if (HasEnteredLoop && !other.HasEnteredLoop)
                continue;
 
            Vector3 toOther = other._transform.position - _transform.position;
            toOther.y = 0f;
 
            if (toOther.magnitude > minSpacing)
                continue;
 
            if (toOther.sqrMagnitude < MinMovementSqrMagnitude)
                return true;
 
            if (Vector3.Dot(toOther.normalized, _transform.forward) > 0f)
                return true;
        }
 
        return false;
    }
 
    public void Tick(float deltaTime)
    {
        float desiredDistance = CurrentDistance + _moveSpeed * deltaTime;
 
        MoveTo(desiredDistance);
    }
 
    private void MoveTo(float distance)
    {
        CurrentDistance = distance;
 
        Vector3 newPosition;
 
        if (CurrentDistance <= _entryLength)
        {
            newPosition = _pathCalculator.GetPointAtDistance(_entryPath, CurrentDistance);
            RotateTowards(_entryPath, CurrentDistance);
        }
        else
        {
            HasEnteredLoop = true;
 
            float loopDistance = _loopLength > 0f ? (CurrentDistance - _entryLength) % _loopLength : 0f;
            newPosition = _pathCalculator.GetPointAtDistance(_loopRoute, loopDistance);
            RotateTowards(_loopRoute, loopDistance);
        }
 
        _transform.position = newPosition;
    }
 
    private void RotateTowards(List<Vector3> route, float distance)
    {
        Vector3 direction = _pathCalculator.GetDirectionAtDistance(route, distance);
        Quaternion offset = Quaternion.Euler(0f, _modelForwardOffsetY, 0f);
        _rotator.RotateInDirection(_transform, direction, offset);
    }
 
    private List<Vector3> BuildEntryPath(List<Vector3> waypoints, IReadOnlyList<Vector3> entryWaypoints)
    {
        var entryPath = new List<Vector3> { _transform.position };

        if (entryWaypoints != null)
            entryPath.AddRange(entryWaypoints);
        
        if (waypoints.Count > 0)
            entryPath.Add(waypoints[0]);
 
        return entryPath;
    }
 
    private List<Vector3> BuildLoopRoute(List<Vector3> waypoints)
    {
        var loopRoute = new List<Vector3>(waypoints);
 
        if (loopRoute.Count > 0)
            loopRoute.Add(loopRoute[0]);
 
        return loopRoute;
    }
}
