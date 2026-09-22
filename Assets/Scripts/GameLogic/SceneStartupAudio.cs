using System.Collections;
using UnityEngine;

public class SceneStartupAudio : MonoBehaviour
{
    [SerializeField] private AudioSource _busHornSource;
    [SerializeField] private AudioClip _busHornSfx;
    [SerializeField] private AudioSource _cityNoiseSource;
    [SerializeField] private AudioClip _cityNoiseSfx;
    [SerializeField] private AudioSource _backgroungMusicSource;
    [SerializeField] private AudioClip _backgroundMusic;
    [SerializeField] private float _cityNoiseTargetVolume = 1f;
    [SerializeField] private float _cityNoiseFadeDuration = 2f;
    [SerializeField] private float _cityNoiseFadeStartDelay = 2f;

    private void Start()
    {
        PlayBusHorn();
        PlayCityNoiseWithFadeOut();
        PlayBackgroundMusic();
    }
    
    private IEnumerator FadeOutAfterDelay(AudioSource source, float startVolume, float duration, float delay)
    {
        yield return new WaitForSeconds(delay);

        float elapsed = 0f;

        while (elapsed < duration)
        {
            source.volume = Mathf.Lerp(startVolume, 0f, elapsed / duration);
            elapsed += Time.deltaTime;

            yield return null;
        }

        source.volume = 0f;
    }
    
    private void PlayBackgroundMusic()
    {
        if (_backgroundMusic == null)
            return;

        _backgroungMusicSource.clip = _backgroundMusic;
        _backgroungMusicSource.loop = true;
        _backgroungMusicSource.Play();
    }

    private void PlayBusHorn()
    {
        if (_busHornSfx == null)
            return;

        _busHornSource.PlayOneShot(_busHornSfx);
    }

    private void PlayCityNoiseWithFadeOut()
    {
        if (_cityNoiseSfx == null)
            return;

        _cityNoiseSource.clip = _cityNoiseSfx;
        _cityNoiseSource.loop = false;
        _cityNoiseSource.volume = _cityNoiseTargetVolume;
        _cityNoiseSource.Play();

        StartCoroutine(FadeOutAfterDelay(_cityNoiseSource, _cityNoiseTargetVolume, _cityNoiseFadeDuration,
            _cityNoiseFadeStartDelay));
    }
}