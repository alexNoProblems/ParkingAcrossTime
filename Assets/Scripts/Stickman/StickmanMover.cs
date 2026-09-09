using System.Collections.Generic;
using UnityEngine;

public class StickmanMover : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;

    private readonly RoutePath _pathCalculator = new RoutePath();
    private readonly MovementRotator _rotator = new MovementRotator();

    private IReadOnlyList<Vector3> _route;
    private StickmanMover _leader;
    
    private float _minSpacing;
    private float _maxDistance;
    private bool _isMoving;

    public float CurrentDistance { get; private set; }
    public bool IsMoving { get; private set; }

    private void Update()
    {
        if (!_isMoving || _route == null)
        {
            IsMoving = false;
            
            return;
        }

        float desiredDistance = CalculateDesiredDistance();
        
        if (desiredDistance <= CurrentDistance)
        {
            IsMoving = false;
            
            return;
        }
        
        MoveTo(desiredDistance);
        
        IsMoving = true;
    }
    
    public void Initialize(IReadOnlyList<Vector3> route, StickmanMover leader, float minSpacing, float maxDistance)
    {
        _route = route;
        _leader = leader;
        _minSpacing = minSpacing;
        _maxDistance = maxDistance;
        CurrentDistance = 0f;
    }

    public void StartMoving()
    {
        _isMoving = true;
    }

    public void SetLeader(StickmanMover leader)
    {
        _leader = leader;
    }

    public void LeaveQueue()
    {
        _isMoving = false;
        _route = null;
        _leader = null;
        IsMoving = false;
    }

    private float CalculateDesiredDistance()
    {
        float allowedDistance = _maxDistance;
        
        if (_leader != null)
            allowedDistance = Mathf.Min(allowedDistance, _leader.CurrentDistance - _minSpacing);
        
        return Mathf.Min(CurrentDistance + _moveSpeed * Time.deltaTime, allowedDistance);
    }

    private void MoveTo(float distance)
    {
        CurrentDistance = distance;
        Vector3 newPosition = _pathCalculator.GetPointAtDistance(_route, CurrentDistance);
        
        _rotator.RotateTowards(transform, newPosition);
        
        transform.position = newPosition;
    }
}