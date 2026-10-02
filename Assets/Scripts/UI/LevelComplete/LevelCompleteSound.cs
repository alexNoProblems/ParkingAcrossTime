using System;
using UnityEngine;

public class LevelCompleteSound : MonoBehaviour
{
    [SerializeField] private BusRegistry _busRegistry;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _winSfx;

    private void OnEnable()
    {
        _busRegistry.AllBusesLeft += PlaySound;
    }

    private void OnDisable()
    {
        _busRegistry.AllBusesLeft -= PlaySound;
    }

    private void PlaySound()
    {
        if (_winSfx != null)
            _audioSource.PlayOneShot(_winSfx);
    }
}
