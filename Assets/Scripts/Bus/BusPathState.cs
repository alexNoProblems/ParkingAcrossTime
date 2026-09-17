using System.Collections.Generic;
using UnityEngine;

public class BusPathState : IBusMovementState
{
    private readonly MovementPriorityComparer _priorityComparer = new MovementPriorityComparer();
    private readonly SpacingClamp _spacingClamp = new SpacingClamp();
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
    public bool BlocksAllTraffic => false;

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

    public void Tick(float deltaTime, IReadOnlyList<IBusMovementState> allStates, float minSpacing)
    {
        float desiredDistance = _currentDistance + _moveSpeed * deltaTime;
        List<Vector3> blockerPositions = CollectBlockerPositions(allStates);
        float clampedDistance = _spacingClamp.ClampDistance(GetPositionAtDistance, _currentDistance, desiredDistance,
            blockerPositions, minSpacing);
        
        MoveTo(clampedDistance);
    }

    private Vector3 GetPositionAtDistance(float distance)
    {
        return _pathCalculator.GetPointAtDistance(_path, Mathf.Min(distance, _pathLength));
    }
    
    private List<Vector3> CollectBlockerPositions(IReadOnlyList<IBusMovementState> allStates)
    {
        var positions = new List<Vector3>();

        foreach (IBusMovementState other in allStates)
        {
            if (ReferenceEquals(other, this) || !other.IsActive)
                continue;

            if (!other.BlocksAllTraffic && _priorityComparer.HasHigherPriority(this, other))
                continue;

            positions.Add(other.Position);
        }

        return positions;
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
}
