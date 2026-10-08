using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class RestartLevelButton : MonoBehaviour
{
   private Button _restartButton;

   private void Awake()
   {
      _restartButton = GetComponent<Button>();
   }

   private void OnEnable()
   {
      _restartButton.onClick.AddListener(Restart);
   }

   private void OnDisable()
   {
      _restartButton.onClick.RemoveListener(Restart);
   }

   private void Restart()
   {
      _restartButton.interactable = false;
      
      Time.timeScale = 1;
      
      Scene activeScene = SceneManager.GetActiveScene();
      SceneManager.LoadScene(activeScene.buildIndex);
   }
}
