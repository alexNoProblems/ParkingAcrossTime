using System;
using UnityEngine;

public class LevelCompleteFireworks : MonoBehaviour
{
    [SerializeField] private BusRegistry _busRegistry;
    [SerializeField] private ParticleSystem[] _fireworks;

    private void OnEnable()
    {
        _busRegistry.AllBusesLeft += PlayFireworks;
    }

    private void OnDisable()
    {
        _busRegistry.AllBusesLeft -= PlayFireworks;
    }

    private void PlayFireworks()
    {
        foreach (ParticleSystem firework in _fireworks)
            firework.Play();
    }
}
