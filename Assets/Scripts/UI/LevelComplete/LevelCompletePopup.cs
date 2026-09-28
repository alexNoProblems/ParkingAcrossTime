using System.Collections;
using UnityEngine;

public class LevelCompletePopup : MonoBehaviour
{
    [SerializeField] private BusRegistry _busRegistry;
    [SerializeField] private GameObject _root;
    [SerializeField] private float _showDelay = 1.5f;

    private WaitForSeconds _delayWait;

    private void Awake()
    {
        _delayWait = new WaitForSeconds(_showDelay);
        _root.SetActive(false);
    }

    private void OnEnable()
    {
        _busRegistry.AllBusesLeft += OnAllBusesLeft;
    }

    private void OnDisable()
    {
        _busRegistry.AllBusesLeft -= OnAllBusesLeft;
    }
    
    private IEnumerator ShowAfterDelay()
    {
        yield return _delayWait;
        
        _root.SetActive(true);
    }

    private void OnAllBusesLeft()
    {
        StartCoroutine(ShowAfterDelay());
    }
}
