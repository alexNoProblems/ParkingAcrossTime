using UnityEngine;

public class MovementRotator
{
    private const float MinMovementSqrMagnitude = 0.0001f;

    public void RotateTowards(Transform target, Vector3 newPosition)
    {
        Vector3 direction = newPosition - target.position;
        direction.y = 0f;
        
        if (direction.sqrMagnitude >= MinMovementSqrMagnitude)
            target.rotation = Quaternion.LookRotation(direction, Vector3.up);
    }
}
