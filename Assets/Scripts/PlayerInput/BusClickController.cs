using UnityEngine;
using UnityEngine.InputSystem;

public class BusClickController : MonoBehaviour
{
   [SerializeField] private BusRoute _busRoute;
   [SerializeField] private Camera _camera;

   private void Update()
   {
      if (Pointer.current == null)
         return;
      
      if (Pointer.current.press.wasPressedThisFrame)
         TryStartPatrol(Pointer.current.position.ReadValue());
   }

   private void TryStartPatrol(Vector2 screenPosition)
   {
      Ray ray = _camera.ScreenPointToRay(screenPosition);
      RaycastHit[] hits = Physics.RaycastAll(ray);

      foreach (RaycastHit hit in hits)
      {
         if(!hit.collider.TryGetComponent<Bus>(out _))
            continue;

         if (hit.collider.TryGetComponent<BusRoutePatrol>(out var busRoutePatrol))
            busRoutePatrol.StartPatrolling(_busRoute);
         
         return;
      }
   }
}
