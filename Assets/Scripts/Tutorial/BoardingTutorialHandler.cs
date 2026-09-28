using System.Collections;
using UnityEngine;

public class BoardingTutorialHandler : MonoBehaviour
{
   [SerializeField] private TutorialPopup _popup;
   [SerializeField] private float _displayDuration = 3f;

   private WaitForSecondsRealtime _displayWait;
   private bool _hasPlayed;

   private void Awake()
   {
      _displayWait = new WaitForSecondsRealtime(_displayDuration);
   }

   public IEnumerator PlayTutorial()
   {
      if (_hasPlayed)
         yield break;
      
      _hasPlayed = true;
      
      float previousTimeScale = Time.timeScale;
      Time.timeScale = 0;
      
      _popup.Show();
      
      yield return _displayWait;
      
      _popup.Hide();
      Time.timeScale = previousTimeScale;
   }
}
