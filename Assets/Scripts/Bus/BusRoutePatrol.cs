using System.Collections.Generic;
using UnityEngine;

public class BusRoutePatrol : MonoBehaviour
{
    private const float MinMovementSqrMagnitude = 0.0001f;

    [SerializeField] private float _moveSpeed = 7f;
    [SerializeField] private float _minSpacing = 2f;
    [SerializeField] private float _modelForwardOffsetY;

    private readonly RoutePath _pathCalculator =  new RoutePath();

    private List<Vector3> _loopRoute;
    private float _loopLength;
    private BusRoutePatrol _leader;
    private BusPatrolManager _patrolManager;
    private bool _isPatrolling;

    public float CurrentDistance { get; private set; }
    public bool IsPatrolling => _isPatrolling;
    public BusRoutePatrol Leader => _leader;
    
    private void Update()
    {
        if (!_isPatrolling)
            return;

        float desireDistance = CalculateDesireDistance();

        if (desireDistance <= CurrentDistance)
            return;
        
        MoveTo(desireDistance);
    }

    public void StartPatrolling(BusRoute route, BusPatrolManager patrolManager)
    {
       _loopRoute = BuildLoopRoute(route);
       _loopLength = _pathCalculator.GetTotalLength(_loopRoute);
       CurrentDistance = 0f;
        _isPatrolling = _loopRoute.Count > 1 && _loopLength > 0f;
        _patrolManager = patrolManager;
        
        _patrolManager.BusRegister(this);
    }

    public void StopPatrolling()
    {
        _isPatrolling = false;
        _patrolManager?.BusUnregister(this);
    }

    public void SetLeader(BusRoutePatrol leader)
    {
        _leader = leader;
    }

    private List<Vector3> BuildLoopRoute(BusRoute route)
    {
        var waypoints = new List<Vector3>(route.GetWaypointPositions());
        
        if (waypoints.Count > 0)
            waypoints.Add(waypoints[0]);
        
        return waypoints;
    }

    private float CalculateDesireDistance()
    {
        float allowDistance = float.MaxValue;

        if (_leader != null)
            allowDistance = _leader.CurrentDistance - _minSpacing;

        return Mathf.Min(CurrentDistance + _moveSpeed * Time.deltaTime, allowDistance);
    }

    private void MoveTo(float distance)
    {
        CurrentDistance = distance;
        float loopDistance = CurrentDistance % _loopLength;
        
        Vector3 newPosition = _pathCalculator.GetPointAtDistance(_loopRoute, loopDistance);
        
        RotateTowards(loopDistance);
        
        transform.position = newPosition;
    }

    private void RotateTowards(float loopDistance)
    {
        Vector3 direction = _pathCalculator.GetDirectionAtDistance(_loopRoute, loopDistance);
        direction.y = 0f;

        if (direction.sqrMagnitude < MinMovementSqrMagnitude)
            return;

        Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = lookRotation * Quaternion.Euler(0f, _modelForwardOffsetY, 0f);
    }
}