using System.Collections.Generic;
using UnityEngine;

public class StickmanMover : MonoBehaviour
{
    private const float MinMovementSqrMagnitude = 0.0001f;
    
    [SerializeField] private float moveSpeed = 5f;

    private readonly StickmanPath _pathCalculator = new StickmanPath();

    private IReadOnlyList<Vector3> _route;
    private StickmanMover _leader;
    
    private float _minSpacing;
    private float _maxDistance;
    private bool _isMoving;

    public float CurrentDistance { get; private set; }
    public bool IsMoving { get; private set; }

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

    private float CalculateDesiredDistance()
    {
        float allowedDistance = _maxDistance;
        
        if (_leader != null)
            allowedDistance = Mathf.Min(allowedDistance, _leader.CurrentDistance - _minSpacing);
        
        return Mathf.Min(CurrentDistance + moveSpeed * Time.deltaTime, allowedDistance);
    }

    private void MoveTo(float distance)
    {
        CurrentDistance = distance;
        Vector3 newPosition = _pathCalculator.GetPointAtDistance(_route, CurrentDistance);
        
        RotateTowards(newPosition);
        
        transform.position = newPosition;
    }

    private void RotateTowards(Vector3 newPosition)
    {
        Vector3 movementDirection = newPosition - transform.position;
        movementDirection.y = 0f;
        
        if (movementDirection.sqrMagnitude >= MinMovementSqrMagnitude)
            transform.rotation = Quaternion.LookRotation(movementDirection,  Vector3.up);
    }
}