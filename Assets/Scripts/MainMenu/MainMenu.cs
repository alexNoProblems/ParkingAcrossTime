using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   [SerializeField] private string _gameSceneName = "Level_1";
   [SerializeField] private GameObject _settingsPanel;
   [SerializeField] private GameObject _shopPanel;
   [SerializeField] private GameObject _leaderboardPanel;

   public void StartGame()
   {
      SceneManager.LoadScene(_gameSceneName);
   }

   public void OpenSettings()
   {
      _settingsPanel.SetActive(true);
   }

   public void CloseSettings()
   {
      _settingsPanel.SetActive(false);
   }
   
   public void OpenShop()
   {
      _shopPanel.SetActive(true);
   }
   
   public void CloseShop()
   {
      _shopPanel.SetActive(false);
   }
   
   public void OpenLeaderboard()
   {
      _leaderboardPanel.SetActive(true);
   }

   public void CloseLeaderboard()
   {
      _leaderboardPanel.SetActive(false);
   }
}
