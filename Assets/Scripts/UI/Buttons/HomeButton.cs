using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class HomeButton : MonoBehaviour
{
    [SerializeField] private string _mainMenuSceneName = "MainMenu";
    
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(GoHome);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(GoHome);
    }

    private void GoHome()
    {
        _button.interactable = false;
        Time.timeScale = 1f;
        
        SceneManager.LoadScene(_mainMenuSceneName);
    }
}
