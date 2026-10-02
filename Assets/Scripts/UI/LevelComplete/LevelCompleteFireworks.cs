using System;
using UnityEngine;

public class LevelCompleteFireworks : MonoBehaviour
{
    [SerializeField] private BusRegistry _busRegistry;
    [SerializeField] private FireworksUIEffect _fireworksEffect;

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
        _fireworksEffect.Play();
    }
}
