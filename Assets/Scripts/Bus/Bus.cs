using System;
using UnityEngine;

[RequireComponent(typeof(BusMover), typeof(BusCapacity))]
public class Bus : MonoBehaviour
{
   [SerializeField] private ColorSetter _colorSetter;
   
   private BusMover _mover;
   private BusCapacity _capacity;
   
   public BusMover Mover => _mover;
   public BusCapacity Capacity => _capacity;

   private void Awake()
   {
      _mover = GetComponent<BusMover>();
      _capacity = GetComponent<BusCapacity>();
   }

   public void Initialize(BusRequest request, Vector3 startPosition, Vector3 targetPosition)
   {
      _colorSetter.SetColor(request.Color);
      _capacity.Initialize(request.Capacity);
      _mover.Initialize(startPosition, targetPosition);
   }
}
