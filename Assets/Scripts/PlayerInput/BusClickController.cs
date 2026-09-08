using UnityEngine;
using UnityEngine.InputSystem;

public class BusClickController : MonoBehaviour
{
   [SerializeField] private BusRoute _busRoute;
   [SerializeField] private BusPatrolManager _busPatrolManager;
   [SerializeField] private Camera _camera;
   [SerializeField] private AudioSource _feedbackAudioSource;
   [SerializeField] private AudioClip _wrongSelectionClip;

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
      if (bus.Lane != null && !bus.Lane.IsFront(bus))
      {
         PlayWrongSelectionFeedback(bus);
         
         return;
      }
      
      if (bus.Lane == null)
         return;

      if (!_busPatrolManager.HasFreeSlot)
      {
         PlayWrongSelectionFeedback(bus);
         
         return;
      }
      
      Bus releasedBus = bus.Lane.ReleaseFront();
      
      if (releasedBus == null)
         return;
      
      releasedBus.Mover.StopMoving();
      releasedBus.PlayExhaustEffect();
      releasedBus.PlayEngineSound();

      _busPatrolManager.StartPatrolling(releasedBus, _busRoute);
   }

   private void PlayWrongSelectionFeedback(Bus bus)
   {
      _feedbackAudioSource.PlayOneShot(_wrongSelectionClip);
      bus.PlayWrongSelectionShake();
   }
}
