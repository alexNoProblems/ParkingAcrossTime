using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneStartupAudio : MonoBehaviour
{
   [SerializeField] private AudioSource _busHornSource;
   [SerializeField] private AudioClip _busHornSfx;
   [SerializeField] private AudioSource _cityNoiseSource;
   [SerializeField] private AudioClip _cityNoiseSfx;
   [SerializeField] private float _cityNoiseTargetVolume = 1f;
   [SerializeField] private float _cityNoiseFadeDuration = 2f;

   private void Start()
   {
      PlayBusHorn();
      PlayCityNoiseWithFadeIn();
   }
   
   private IEnumerator FadeIn(AudioSource source, float targetVolume, float duration)
   {
      float elapsed = 0f;

      while (elapsed < duration)
      {
         source.volume = Mathf.Lerp(0f, targetVolume, elapsed / duration);
         elapsed += Time.deltaTime;

         yield return null;
      }

      source.volume = targetVolume;
   }

   private void PlayBusHorn()
   {
      if (_busHornSfx == null)
         return;
      
      _busHornSource.PlayOneShot(_busHornSfx);
   }
   
   private void PlayCityNoiseWithFadeIn()
   {
      if (_cityNoiseSfx == null)
         return;

      _cityNoiseSource.clip = _cityNoiseSfx;
      _cityNoiseSource.loop = false;
      _cityNoiseSource.volume = 0f;
      _cityNoiseSource.Play();

      StartCoroutine(FadeIn(_cityNoiseSource, _cityNoiseTargetVolume, _cityNoiseFadeDuration));
   }
}
