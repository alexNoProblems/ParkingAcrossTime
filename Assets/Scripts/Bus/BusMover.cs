using UnityEngine;

public class BusMover : MonoBehaviour
{
    private const float MinMovementSqrMagnitude = 0.0001f;

    [SerializeField] private float _moveSpeed = 4f;
    
    private Vector3 _targetPosition;
    private bool _isMoving;
    
    public bool IsMoving { get; private set; }

    private void Update()
    {
        if (!_isMoving)
        {
            IsMoving = false;
            
            return;
        }

        Vector3 newPosition = Vector3.MoveTowards(transform.position, _targetPosition, _moveSpeed * Time.deltaTime);

        if (newPosition == transform.position)
        {
            IsMoving = false;
            
            return;
        }
        
        RotateTowards(newPosition);
        
        transform.position = newPosition;
        IsMoving = true;
    }

    public void Initialize(Vector3 startPosition, Vector3 targetPosition)
    {
        transform.position = startPosition;
        _targetPosition = targetPosition;
    }

    public void StartMoving()
    {
        _isMoving = true;
    }

    public void StopMoving()
    {
        _isMoving = false;
    }

    public void SetTarget(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
        _isMoving = true;
    }

    private void RotateTowards(Vector3 newPosition)
    {
        Vector3 direction = newPosition - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude >= MinMovementSqrMagnitude)
            transform.rotation = Quaternion.LookRotation(direction,  Vector3.up);
    }
}