using System;
using System.Collections.Generic;
using UnityEngine;

public class Stickman : MonoBehaviour
{
   [SerializeField] private StickmanMover _mover;
   [SerializeField] private ColorSetter _colorSetter;
   [SerializeField] private StickmanAnimator _animator;
   
   public StickmanMover Mover => _mover;
   public StickmanColor Color { get;  private set; }

   private void Update()
   {
      _animator.SetRunning(_mover.IsMoving);
   }
   
   public void Initialize(StickmanColor color, IReadOnlyList<Vector3> path, StickmanMover leader, float minSpacing,
      float maxDistance)
   {
      Color  = color;
      _colorSetter.SetColor(color);
      _mover.Initialize(path, leader, minSpacing, maxDistance);
      _mover.StartMoving();
   }

   public void SitAt(Transform seat)
   {
      _mover.LeaveQueue();
      _mover.enabled = false;
      
      transform.SetParent(seat, false);
      transform.localEulerAngles = Vector3.zero;
      transform.localRotation = Quaternion.identity;
      
      _animator.SetRunning(false);
   }
}
