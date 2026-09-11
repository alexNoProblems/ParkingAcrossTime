using System;
using System.Collections.Generic;
using UnityEngine;

public class Stickman : MonoBehaviour
{
   private const string SeatAnchorName = "SeatAnchor";
   
   [SerializeField] private StickmanMover _mover;
   [SerializeField] private ColorSetter _colorSetter;
   [SerializeField] private StickmanAnimator _animator;
   [SerializeField] private Vector3 _seatRotationOffset;
   [SerializeField] private float _seatScaleMultiplier = 1f;
   [SerializeField] private Vector3 _seatPositionOffset;
   
   public StickmanMover Mover => _mover;
   public StickmanColor Color { get;  private set; }
   
   private readonly ScaleNeutralizer _scaleNeutralizer = new ScaleNeutralizer();

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

      Transform anchor = _scaleNeutralizer.CreateNeutralAnchor(seat, SeatAnchorName);

      transform.SetParent(anchor, false);
      transform.localPosition = _seatPositionOffset;
      transform.localRotation = Quaternion.Euler(_seatRotationOffset);
      transform.localScale = Vector3.one * _seatScaleMultiplier;

      _animator.SetRunning(false);
   }
}
