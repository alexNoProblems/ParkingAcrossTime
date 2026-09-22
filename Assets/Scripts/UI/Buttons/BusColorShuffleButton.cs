using UnityEngine;

public class BusColorShuffleButton : MonoBehaviour
{
    [SerializeField] private BusParkingShuffler _parkingShuffler;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _blinkSound;

    public void TriggerShuffle()
    {
        if (_blinkSound != null)
            _audioSource.PlayOneShot(_blinkSound);
        
        _parkingShuffler.Shuffle();
    }
}
