using System;
using UnityEngine;
using UnityEngine.UI;

public class SoundToggleButton : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private Sprite _volumeOnSprite;
    [SerializeField] private Sprite _volumeOffSprite;

    private bool _isMuted;

    private void Start()
    {
        _isMuted = AudioListener.volume <= 0;
        UpdateIcon();
    }

    public void ToggleSound()
    {
        _isMuted = !_isMuted;
        AudioListener.volume = _isMuted ? 0f : 1f;

        UpdateIcon();
    }

    private void UpdateIcon()
    {
        _icon.sprite = _isMuted ? _volumeOffSprite : _volumeOnSprite;
    }
}
