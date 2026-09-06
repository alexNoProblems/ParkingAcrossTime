using System.Collections.Generic;
using UnityEngine;

public class BusRoutePatrol : MonoBehaviour
{
    private const float MinMovementSqrMagnitude = 0.0001f;
 
    [SerializeField] private float _moveSpeed = 4f;
    [SerializeField] private float _minSpacing = 2f;
    [SerializeField] private float _modelForwardOffsetY;
 
    private readonly RoutePath _pathCalculator = new RoutePath();
 
    private List<Vector3> _entryPath;
    private List<Vector3> _loopRoute;
    private float _entryLength;
    private float _loopLength;
    private BusPatrolManager _patrolManager;
    private bool _isPatrolling;
    private bool _hasEnteredLoop;
 
    public float CurrentDistance { get; private set; }
    public bool IsPatrolling => _isPatrolling;
    public bool HasEnteredLoop => _hasEnteredLoop;
 
    public void StartPatrolling(BusRoute route, BusPatrolManager patrolManager)
    {
        var waypoints = new List<Vector3>(route.GetWaypointPositions());
 
        _entryPath = BuildEntryPath(waypoints);
        _entryLength = _pathCalculator.GetTotalLength(_entryPath);
 
        _loopRoute = BuildLoopRoute(waypoints);
        _loopLength = _pathCalculator.GetTotalLength(_loopRoute);
 
        CurrentDistance = 0f;
        _hasEnteredLoop = false;
        _isPatrolling = waypoints.Count > 0;
        _patrolManager = patrolManager;
 
        _patrolManager.BusRegister(this);
    }
 
    public void StopPatrolling()
    {
        _isPatrolling = false;
 
        _patrolManager?.BusUnregister(this);
    }
 
    private List<Vector3> BuildEntryPath(List<Vector3> waypoints)
    {
        if (waypoints.Count == 0)
            return new List<Vector3> { transform.position };
 
        return new List<Vector3> { transform.position, waypoints[0] };
    }
 
    private List<Vector3> BuildLoopRoute(List<Vector3> waypoints)
    {
        var loopRoute = new List<Vector3>(waypoints);
 
        if (loopRoute.Count > 0)
            loopRoute.Add(loopRoute[0]);
 
        return loopRoute;
    }
 
    private void Update()
    {
        if (!_isPatrolling)
            return;
 
        if (IsBlockedAhead())
            return;
 
        float desiredDistance = CurrentDistance + _moveSpeed * Time.deltaTime;
 
        MoveTo(desiredDistance);
    }
 
    private bool IsBlockedAhead()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _minSpacing);
 
        foreach (Collider hit in hits)
        {
            if (!hit.TryGetComponent<BusRoutePatrol>(out var otherPatrol))
                continue;
 
            if (otherPatrol == this)
                continue;
 
            if (!otherPatrol.IsPatrolling)
                continue;
            
            if (_hasEnteredLoop && !otherPatrol.HasEnteredLoop)
                continue;
 
            Vector3 toOther = otherPatrol.transform.position - transform.position;
            toOther.y = 0f;
 
            if (toOther.sqrMagnitude < MinMovementSqrMagnitude)
                return true;
 
            if (Vector3.Dot(toOther.normalized, transform.forward) > 0f)
                return true;
        }
 
        return false;
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
            _hasEnteredLoop = true;
 
            float loopDistance = _loopLength > 0f ? (CurrentDistance - _entryLength) % _loopLength : 0f;
            newPosition = _pathCalculator.GetPointAtDistance(_loopRoute, loopDistance);
            RotateTowards(_loopRoute, loopDistance);
        }
 
        transform.position = newPosition;
    }
 
    private void RotateTowards(List<Vector3> route, float distance)
    {
        Vector3 direction = _pathCalculator.GetDirectionAtDistance(route, distance);
        direction.y = 0f;
 
        if (direction.sqrMagnitude < MinMovementSqrMagnitude)
            return;
 
        Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = lookRotation * Quaternion.Euler(0f, _modelForwardOffsetY, 0f);
    }
}