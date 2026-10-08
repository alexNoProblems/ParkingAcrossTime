using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasScaler))]
public class CanvasMatchByAspect : MonoBehaviour
{
    private CanvasScaler _scaler;
    private int _lastWidth;
    private int _lastHeight;

    private void Awake()
    {
        _scaler = GetComponent<CanvasScaler>();
        Apply();
    }

    private void Update()
    {
        if (Screen.width != _lastWidth || Screen.height != _lastHeight)
            Apply();
    }

    private void Apply()
    {
        _lastWidth = Screen.width;
        _lastHeight = Screen.height;

        float referenceAspect = _scaler.referenceResolution.x / _scaler.referenceResolution.y;
        float screenAspect = (float)Screen.width / Screen.height;

        _scaler.matchWidthOrHeight = screenAspect >= referenceAspect ? 1f : 0f;
    }
}