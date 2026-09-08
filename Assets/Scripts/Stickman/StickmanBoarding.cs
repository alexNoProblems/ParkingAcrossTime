using System;
using UnityEngine;

public class StickmanBoarding : MonoBehaviour
{
    private const float MinMovementSqrMagnitude = 0.0001f;
    
    [SerializeField] private float _moveSpeed;
    
    private Vector3? _targetPosition;

    private Action _onComplete;
    
    public bool IsBoarding => _targetPosition.HasValue;

    private void Update()
    {
        if (!_targetPosition.HasValue)
            return;
        
        Vector3 target =  _targetPosition.Value;
        Vector3 newPosition = Vector3.MoveTowards(transform.position, target, Time.deltaTime * _moveSpeed);
        
        RotateTowards(newPosition);
        transform.position = newPosition;

        if (newPosition != target)
            return;

        _targetPosition = null;
        
        Action onComplete = _onComplete;
        _onComplete = null;
        onComplete?.Invoke();
    }
    
    public void MoveTo(Vector3 targetPosition, Action onComplete)
    {
        _targetPosition = targetPosition;
        _onComplete = onComplete;
    }
    
    private void RotateTowards(Vector3 newPosition)
    {
        Vector3 direction = newPosition - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude >= MinMovementSqrMagnitude)
            transform.rotation = Quaternion.LookRotation(direction,  Vector3.up);
    }
}
