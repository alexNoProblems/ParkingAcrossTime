using System.Collections;
using UnityEngine;

public class FirstLaneHint : MonoBehaviour
{
   [SerializeField] private BusClickController _clickController;
   [SerializeField] private TutorialPopup _popup;
   [SerializeField] private GameTutorialHandler _gameTutorialHandler;
   [SerializeField] private float _displayDuration = 3f;

   private BusSelector _busSelector;
   private WaitForSecondsRealtime _displayWait;
   private bool _hasShown;

   private void Awake()
   {
      _displayWait = new WaitForSecondsRealtime(_displayDuration);
   }

   private void Start()
   {
      _popup.Hide();
      
      _busSelector = _clickController.Selector;
      _busSelector.NotFrontBusSelected += OnNotFrontBusSelected;
   }

   private void OnDestroy()
   {
      if (_busSelector != null)
         _busSelector.NotFrontBusSelected -= OnNotFrontBusSelected;
   }

   private IEnumerator ShownRoutine()
   {
      _gameTutorialHandler.Suspend();
      _popup.Show();
      
      yield return _displayWait;
      
      _popup.Hide();
      _gameTutorialHandler.Resume();
   }
   
   private void OnNotFrontBusSelected()
   {
      if (_hasShown)
         return;
      
      _hasShown = true;
      StartCoroutine(ShownRoutine());
   }
}
