using UnityEngine;

public class MovementRotator
{
    private const float MinMovementSqrMagnitude = 0.0001f;

    public void RotateTowards(Transform target, Vector3 newPosition)
    {
        RotateInDirection(target, newPosition - target.position, Quaternion.identity);
    }

    public void RotateInDirection(Transform target, Vector3 direction, Quaternion offset)
    {
        direction.y = 0f;
        
        if(direction.sqrMagnitude < MinMovementSqrMagnitude)
            return;
        
        Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
        target.rotation = lookRotation *  offset;
    }
}
