using System.Collections;
using TMPro;
using UnityEngine;

public class LevelStartPopup : MonoBehaviour
{
    [SerializeField] private LevelDataInitializer _levelData;
    [SerializeField] private GameObject _root;
    [SerializeField] private TextMeshProUGUI _levelNumber;
    [SerializeField] private float _displayDuration = 2f;

    private WaitForSeconds _waitForSeconds;

    private void Awake()
    {
        _waitForSeconds = new WaitForSeconds(_displayDuration);
    }

    private void Start()
    {
        _levelNumber.text = _levelData.CurrentLevel.ToString();
        _root.SetActive(true);

        StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        yield return _waitForSeconds;

        _root.SetActive(false);
    }
}