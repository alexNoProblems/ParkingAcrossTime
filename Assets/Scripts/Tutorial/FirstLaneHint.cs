using System.Collections;
using UnityEngine;
using YG;

public class FirstLaneHint : MonoBehaviour
{
   [SerializeField] private BusClickController _clickController;
   [SerializeField] private TutorialPopup _popup;
   [SerializeField] private GameTutorialHandler _gameTutorialHandler;
   [SerializeField] private float _displayDuration = 3f;

   private BusSelector _busSelector;
   private WaitForSecondsRealtime _displayWait;
   private bool _isShowing;

   private void Awake()
   {
      _displayWait = new WaitForSecondsRealtime(_displayDuration);
   }

   private void Start()
   {
      _popup.Hide();

      if (YG2.saves.firstLaneHintShown)
         return;
      
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
      _isShowing = true;

      if (_gameTutorialHandler != null)
         _gameTutorialHandler.Suspend();
     
      _popup.Show();
      
      yield return _displayWait;
      
      _popup.Hide();

      if (_gameTutorialHandler != null)
         _gameTutorialHandler.Resume();
      
      YG2.saves.firstLaneHintShown = true;
      YG2.SaveProgress();
      
      _isShowing = false;
   }
   
   private void OnNotFrontBusSelected()
   {
      if (_isShowing || YG2.saves.firstLaneHintShown)
         return;
      
      StartCoroutine(ShownRoutine());
   }
}
