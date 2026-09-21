using UnityEngine;

public class StickmanColorShuffleButton : MonoBehaviour
{
    [SerializeField] private LevelColorShuffleCoordinator _levelCoordinator;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _blinkSound;

    public void TriggerShuffle()
    {
        if (_blinkSound != null)
            _audioSource.PlayOneShot(_blinkSound);

        _levelCoordinator.TriggerShuffle();
    }
}
