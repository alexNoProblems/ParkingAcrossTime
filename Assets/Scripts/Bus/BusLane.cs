using System.Collections.Generic;
using UnityEngine;

public class BusLane : MonoBehaviour
{
   [SerializeField] private Bus _busPrefab;
   [SerializeField] private Transform _spawnPoint;
   [SerializeField] private List<Transform> _parkingSlots;
   
   private readonly List<Bus> _busesInLine = new List<Bus>();
   private readonly Queue<BusRequest> _pendingRequests = new Queue<BusRequest>();
   
   public int SlotCount => _parkingSlots.Count;
   public int PendingAndActiveCount => _busesInLine.Count + _pendingRequests.Count;
   private bool HasFreeSlot => _busesInLine.Count < _parkingSlots.Count;

   public void FillInstant(BusRequest busRequest)
   {
      if (!HasFreeSlot)
      {
         Enqueue(busRequest);
         
         return;
      }
      
      int targetSlotIndex = _busesInLine.Count;
      Vector3 slotPosition = _parkingSlots[targetSlotIndex].position;
      
      SpawnBus(busRequest, slotPosition, slotPosition, startMoving : false);
   }

   public void Enqueue(BusRequest busRequest)
   {
      _pendingRequests.Enqueue(busRequest);

      TrySpawnNext();
   }

   public void DepartFront()
   {
      if (_busesInLine.Count == 0)
         return;
      
      Bus frontBus = _busesInLine[0];
      _busesInLine.RemoveAt(0);
      
      // TODO: отправить frontBus по маршруту выезда со сцены, а не удалять сразу
      
      Destroy(frontBus.gameObject);
      ShiftBussesForward();
      TrySpawnNext();
   }

   private void TrySpawnNext()
   {
      if (!HasFreeSlot || _pendingRequests.Count == 0)
         return;
      
      BusRequest busRequest = _pendingRequests.Dequeue();
      int targetSlotIndex = _busesInLine.Count;
      
      SpawnBus(busRequest, _spawnPoint.position, _parkingSlots[targetSlotIndex].position, startMoving : true);
   }

   private void SpawnBus(BusRequest busRequest, Vector3 startPosition, Vector3 targetPosition, bool startMoving)
   {
      Bus bus = Instantiate(_busPrefab, startPosition, Quaternion.identity);
      bus.Initialize(busRequest, startPosition, targetPosition);
      
      if(startMoving == true)
         bus.Mover.StartMoving();
      
      _busesInLine.Add(bus);
   }

   private void ShiftBussesForward()
   {
      for (int i = 0; i < _busesInLine.Count; i++)
         _busesInLine[i].Mover.SetTarget(_parkingSlots[i].position);
   }
}
