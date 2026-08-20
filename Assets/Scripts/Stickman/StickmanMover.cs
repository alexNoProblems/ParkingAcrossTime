using System.Collections.Generic;
using UnityEngine;

public class StickmanMover : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private readonly StickmanPath _pathCalculator = new StickmanPath();

    private IReadOnlyList<Vector3> _route;
    private StickmanMover _leader;
    
    private float _minSpacing;
    private float _maxDistance;
    private bool _isMoving;

    public float CurrentDistance { get; private set; }

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
            return;

        float allowedDistance = _maxDistance;

        if (_leader != null)
            allowedDistance = Mathf.Min(allowedDistance, _leader.CurrentDistance - _minSpacing);

        float desiredDistance = Mathf.Min(CurrentDistance + moveSpeed * Time.deltaTime, allowedDistance);

        if (desiredDistance <= CurrentDistance)
            return;

        CurrentDistance = desiredDistance;
        transform.position = _pathCalculator.GetPointAtDistance(_route, CurrentDistance);
    }
}