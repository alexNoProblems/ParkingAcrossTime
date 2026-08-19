using System.Collections.Generic;
using UnityEngine;

public class StickmanMover : MonoBehaviour
{
   [SerializeField] private float moveSpeed = 5f;
   [SerializeField] private float arrivalThreshold = 0.05f;

   private Queue<Vector3> _remainingWaypoints;
   private Vector3 _currentTarget;
   private bool _isMoving = false;

   private void Update()
   {
      if (!_isMoving)
         return;
      
      transform.position = Vector3.MoveTowards(transform.position, _currentTarget, moveSpeed * Time.deltaTime);
      
      if (Vector3.Distance(transform.position, _currentTarget) < arrivalThreshold)
         MoveToNextWaypoint();
   }

   public void StartMoving(List<Vector3> path)
   {
      _remainingWaypoints = new Queue<Vector3>(path);

      MoveToNextWaypoint();
   }

   private void MoveToNextWaypoint()
   {
      if (_remainingWaypoints.Count > 0)
      {
         _currentTarget = _remainingWaypoints.Dequeue();
         _isMoving = true;
      }
      else
      {
         _isMoving = false;
      }
   }
}
