using UnityEngine;

public class AdaptiveButtonPlacer : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private RectTransform _canvasRect;
    [SerializeField] private Transform _stickmanWorldAnchor;
    [SerializeField] private Transform _busWorldAnchor;
    [SerializeField] private RectTransform _stickmanButton;
    [SerializeField] private RectTransform _busButton;
    [SerializeField] private Vector2 _stickmanLandscapePosition;
    [SerializeField] private Vector2 _busLandscapePosition;
    [SerializeField] private float _portraitAspectThreshold = 1f;
    [SerializeField] private float _safeMargin = 24f;
    
    private int _lastScreenWidth;
    private int _lastScreenHeight;

    private void Update()
    {
        if (Screen.width ==  _lastScreenWidth && Screen.height == _lastScreenHeight)
            return;
        
        _lastScreenWidth = Screen.width;
        _lastScreenHeight = Screen.height;

        Apply();
    }

    private void Apply()
    {
        float aspect = (float)Screen.width / Screen.height;
        bool isPortrait = aspect < _portraitAspectThreshold;

        if (isPortrait)
        {
            PlaceAtWorldAnchor(_stickmanButton, _stickmanWorldAnchor);
            PlaceAtWorldAnchor(_busButton, _busWorldAnchor);
        }
        else
        {
            _stickmanButton.anchoredPosition = _stickmanLandscapePosition;
            _busButton.anchoredPosition = _busLandscapePosition;
        }
    }

    private void PlaceAtWorldAnchor(RectTransform button, Transform worldAnchor)
    {
        Vector2 screenPoint = _camera.WorldToScreenPoint(worldAnchor.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRect, screenPoint, null, out Vector2 localPoint);
        button.anchoredPosition = ClampToSafeArea(localPoint, button);
    }

    private Vector2 ClampToSafeArea(Vector2 localPoint, RectTransform button)
    {
        Vector2 halfButtonSize = button.rect.size * 0.5f;
        Vector2 halfCanvasSize = _canvasRect.rect.size * 0.5f;

        float minX = -halfCanvasSize.x + halfButtonSize.x + _safeMargin;
        float maxX = halfCanvasSize.x - halfButtonSize.x - _safeMargin;
        float minY = -halfCanvasSize.y + halfButtonSize.y + _safeMargin;
        float maxY = halfCanvasSize.y - halfButtonSize.y - _safeMargin;

        localPoint.x = Mathf.Clamp(localPoint.x, minX, maxX);
        localPoint.y = Mathf.Clamp(localPoint.y, minY, maxY);

        return localPoint;
    }
}
