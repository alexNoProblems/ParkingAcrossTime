using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TutorialPopup : MonoBehaviour
{
    [SerializeField] private GameObject _root;
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] private float _characterPerSecond = 30f;

    private Coroutine _typingRoutine;
    private WaitForSecondsRealtime _typingDelay;
    private string _fullMessage;
    
    private void Awake()
    {
        _fullMessage =  _text.text;
        _typingDelay = new WaitForSecondsRealtime(1f / _characterPerSecond);
    }

    public void Show()
    {
        _root.SetActive(true);
        _text.text = string.Empty;

        _typingRoutine = StartCoroutine(TypeText());
    }

    public void Hide()
    {
        if (_typingRoutine != null)
            StopCoroutine(_typingRoutine);
        
        _root.SetActive(false);
    }
    
    private IEnumerator TypeText()
    {
        int characterCount = 0;

        while (characterCount < _fullMessage.Length)
        {
            characterCount++;
            _text.text = _fullMessage.Substring(0, characterCount);

            yield return _typingDelay;
        }
    }
}
