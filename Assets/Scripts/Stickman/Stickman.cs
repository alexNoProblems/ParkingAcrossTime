using System.Collections.Generic;
using UnityEngine;

public class Stickman : MonoBehaviour
{
   [SerializeField] private StickmanMover _mover;
   [SerializeField] private ColorSetter _colorSetter;
   [SerializeField] private StickmanAnimator _animator;
   
   public StickmanMover Mover => _mover;

   public void Initialize(StickmanColor color, IReadOnlyList<Vector3> path, StickmanMover leader, float minSpacing,
      float maxDistance)
   {
      _colorSetter.SetColor(color);
      _mover.Initialize(path, leader, minSpacing, maxDistance);
      _mover.StartMoving();
   }

   private void Update()
   {
      _animator.SetRunning(_mover.IsMoving);
   }
}
