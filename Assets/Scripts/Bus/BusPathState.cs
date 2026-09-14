using System.Collections.Generic;
using UnityEngine;

public class BusPathState : IBusMovementState
{
    private const float MinMovementSqrMagnitude = 0.0001f;

    private readonly RoutePath _pathCalculator = new RoutePath();
    private readonly MovementRotator _rotator = new MovementRotator();
    private readonly Transform _transform;
    private readonly List<Vector3> _path;
    private readonly float _moveSpeed;
    private readonly float _modelForwardOffsetY;
    private readonly float _pathLength;
    private readonly int _priorityOrder;

    private float _currentDistance;

    public Bus Bus { get; }
    public bool IsActive { get; private set; } = true;
    public int EffectivePriority { get; }
    public int PriorityOrder => _priorityOrder;
    public Vector3 Position => _transform.position;
    public bool IsComplete { get; private set; }

    public BusPathState(Bus bus, IReadOnlyList<Vector3> path, float moveSpeed, float modelForwardOffsetY,
        int effectivePriority, int priorityOrder)
    {
        Bus = bus;
        _transform = bus.transform;
        _moveSpeed = moveSpeed;
        _modelForwardOffsetY = modelForwardOffsetY;
        EffectivePriority = effectivePriority;
        _priorityOrder = priorityOrder;

        _path = new List<Vector3> { _transform.position };
        _path.AddRange(path);
        _pathLength = _pathCalculator.GetTotalLength(_path);
    }

    public void Complete()
    {
        IsActive = false;
    }

    public bool IsBlockedAhead(IReadOnlyList<IBusMovementState> allStates, float minSpacing)
    {
        foreach (IBusMovementState other in allStates)
        {
            if (ReferenceEquals(other, this) || !other.IsActive)
                continue;

            if (HasHigherPriorityThan(other))
                continue;

            Vector3 toOther = other.Position - _transform.position;
            toOther.y = 0f;

            if (toOther.magnitude <= minSpacing)
                continue;
        }

        return false;
    }

    public void Tick(float deltaTime)
    {
        float desiredDistance = _currentDistance + _moveSpeed * deltaTime;
        MoveTo(desiredDistance);
    }

    private void MoveTo(float distance)
    {
        _currentDistance = Mathf.Min(distance, _pathLength);

        Vector3 newPosition = _pathCalculator.GetPointAtDistance(_path, _currentDistance);
        Vector3 direction = _pathCalculator.GetDirectionAtDistance(_path, _currentDistance);
        Quaternion offset = Quaternion.Euler(0f, _modelForwardOffsetY, 0f);
        _rotator.RotateInDirection(_transform, direction, offset);

        _transform.position = newPosition;

        if (_currentDistance >= _pathLength)
            IsComplete = true;
    }

    private bool HasHigherPriorityThan(IBusMovementState other)
    {
        if (EffectivePriority != other.EffectivePriority)
            return EffectivePriority > other.EffectivePriority;

        return _priorityOrder > other.PriorityOrder;
    }
}
