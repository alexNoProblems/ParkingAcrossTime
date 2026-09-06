using UnityEngine;
using UnityEngine.InputSystem;

public class BusClickController : MonoBehaviour
{
   [SerializeField] private BusRoute _busRoute;
   [SerializeField] private BusPatrolManager _busPatrolManager;
   [SerializeField] private Camera _camera;

   private void Update()
   {
      if (Pointer.current == null)
         return;
      
      if (Pointer.current.press.wasPressedThisFrame)
         TryHandleClick(Pointer.current.position.ReadValue());
   }

   private void TryHandleClick(Vector2 screenPosition)
   {
      Ray ray = _camera.ScreenPointToRay(screenPosition);
      RaycastHit[] hits = Physics.RaycastAll(ray);

      foreach (RaycastHit hit in hits)
      {
         if(!hit.collider.TryGetComponent<Bus>(out var bus))
            continue;

         HandleBusClicked(bus);
         
         return;
      }
   }

   private void HandleBusClicked(Bus bus)
   {
      if (bus.Lane == null || !bus.Lane.IsFront(bus))
         return;

      if (!_busPatrolManager.HasFreeSlot)
         return;

      Bus realeasedBus = bus.Lane.ReleaseFront();
      
      if (realeasedBus == null)
         return;
      
      realeasedBus.Mover.StopMoving();

      _busPatrolManager.StartPatrolling(realeasedBus, _busRoute);
   }
}
