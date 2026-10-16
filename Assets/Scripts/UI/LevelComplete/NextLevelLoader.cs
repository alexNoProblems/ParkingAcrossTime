using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class NextLevelLoader
{
   private readonly string _sceneName;
   
   public NextLevelLoader(string sceneName)
   {
      _sceneName = sceneName;
   }

   public void LoadNextLevel()
   {
      YG2.saves.level = Mathf.Max(1, YG2.saves.level) + 1;
      YG2.SaveProgress();

      Time.timeScale = 1f;
      SceneManager.LoadScene(_sceneName);
   }
}
