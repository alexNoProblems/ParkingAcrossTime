using UnityEngine;
using UnityEngine.InputSystem;

public class BusClickController : MonoBehaviour
{
   [SerializeField] private BusBoarding _busBoarding;
   [SerializeField] private BusPatrolManager _busPatrolManager;
   [SerializeField] private Camera _camera;
   [SerializeField] private AudioSource _feedbackAudioSource;
   [SerializeField] private AudioClip _wrongSelectionClip;

   private BusSelector _selector;

   private void Awake()
   {
      _selector = new BusSelector(_busBoarding, _busPatrolManager, _feedbackAudioSource, _wrongSelectionClip);
   }

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
         if (!hit.collider.TryGetComponent<Bus>(out var bus))
            continue;

         _selector.Select(bus);

         return;
      }
   }
}