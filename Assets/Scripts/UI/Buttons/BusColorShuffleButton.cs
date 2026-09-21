using UnityEngine;

public class BusColorShuffleButton : MonoBehaviour
{
    [SerializeField] private BusRegistry _registry;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _blinkSound;
    [SerializeField] private float _blinkDuration = 3f;
    [SerializeField] private float _blinkInterval = 0.1f;
    
    private readonly ColorShuffleRoutine _shuffleRoutine = new ColorShuffleRoutine();

    public void TriggerShuffle()
    {
        if (_blinkSound != null)
            _audioSource.PlayOneShot(_blinkSound);
        
        StartCoroutine(_shuffleRoutine.Run(_registry.GetAll(), _blinkDuration, _blinkInterval));
    }
}
