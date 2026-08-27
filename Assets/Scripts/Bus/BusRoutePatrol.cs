using System.Collections.Generic;
using UnityEngine;

public class BusRoutePatrol : MonoBehaviour
{
    private const float ArrivalThreshold = 0.05f;
    private const float MinMovementSqrMagnitude = 0.0001f;

    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float modelForwardOffsetY;

    private IReadOnlyList<Vector3> _waypoints;
    private int _currentIndex;
    private bool _isPatrolling;

    public bool IsPatrolling => _isPatrolling;

    public void StartPatrolling(BusRoute route)
    {
        _waypoints = route.GetWaypointPositions();
        _currentIndex = 0;
        _isPatrolling = _waypoints.Count > 0;
    }

    public void StopPatrolling()
    {
        _isPatrolling = false;
    }

    private void Update()
    {
        if (!_isPatrolling)
            return;

        Vector3 target = _waypoints[_currentIndex];
        Vector3 newPosition = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

        RotateTowards(newPosition);

        transform.position = newPosition;

        if (Vector3.Distance(newPosition, target) <= ArrivalThreshold)
            _currentIndex = (_currentIndex + 1) % _waypoints.Count;
    }

    private void RotateTowards(Vector3 newPosition)
    {
        Vector3 direction = newPosition - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < MinMovementSqrMagnitude)
            return;

        Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = lookRotation * Quaternion.Euler(0f, modelForwardOffsetY, 0f);
    }
}