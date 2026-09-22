using System.Collections.Generic;
using UnityEngine;

public class BusMover : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 4f;
    
    private readonly MovementRotator _rotator = new MovementRotator();
    
    private Vector3 _targetPosition;
    private Quaternion? _finalRotation;
    private Queue<Vector3> _pathQueue;
    private bool _isMoving;
    private WaitUntil _stoppedWait;
    private BusMovementGate _gate;
    
    public bool IsMoving { get; private set; }
    public WaitUntil StoppedWait => _stoppedWait ??= new WaitUntil(() => !IsMoving);

    private void Update()
    {
        if (_gate != null && _gate.IsPaused)
            return;
        
        if (!_isMoving)
        {
            IsMoving = false;
            
            return;
        }

        Vector3 newPosition = Vector3.MoveTowards(transform.position, _targetPosition, _moveSpeed * Time.deltaTime);

        if (newPosition == transform.position)
        {
            if (_pathQueue != null && _pathQueue.Count > 0)
            {
                _targetPosition = _pathQueue.Dequeue();

                return;
            }
            
            if (_finalRotation.HasValue)
                transform.rotation = _finalRotation.Value;
            
            _isMoving = false;
            IsMoving = false;
            
            return;
        }
        
        _rotator.RotateTowards(transform, newPosition);
        
        transform.position = newPosition;
        IsMoving = true;
    }

    public void Initialize(Vector3 startPosition, Vector3 targetPosition)
    {
        transform.position = startPosition;
        _targetPosition = targetPosition;
    }

    public void SetGate(BusMovementGate gate)
    {
        _gate = gate;
    }

    public void StartMoving()
    {
        _isMoving = true;
    }

    public void StopMoving()
    {
        _isMoving = false;
        _pathQueue = null;
    }

    public void SetTarget(Vector3 targetPosition, Quaternion? finalRotation = null)
    {
        _targetPosition = targetPosition;
        _pathQueue = null;
        _finalRotation = finalRotation;
        _isMoving = true;
    }

    public void SetPath(IReadOnlyList<Vector3> waypoints)
    {
        if (waypoints == null || waypoints.Count == 0)
            return;
        
        _pathQueue = new Queue<Vector3>(waypoints);
        _targetPosition = _pathQueue.Dequeue();
        _finalRotation = null;
        _isMoving = true;
    }
}