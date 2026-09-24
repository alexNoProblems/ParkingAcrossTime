using UnityEngine;

public class AdaptivePopupPosition : MonoBehaviour
{
    [SerializeField] private RectTransform _rect;
    [SerializeField] private RectTransform _canvasRect;
    [SerializeField] private Camera _camera;
    [SerializeField] private float _viewportDepth = 10f;
    [SerializeField] private float _bottomMarginPixels = 40f;

    private int _lastScreenWidth;
    private int _lastScreenHeight;

    private void Update()
    {
        if (Screen.width == _lastScreenWidth && Screen.height == _lastScreenHeight)
            return;

        _lastScreenWidth = Screen.width;
        _lastScreenHeight = Screen.height;

        Apply();
    }

    private void Apply()
    {
        Vector3 worldBottomCenter = _camera.ViewportToWorldPoint(new Vector3(0.5f, 0f, _viewportDepth));
        Vector2 screenPoint = _camera.WorldToScreenPoint(worldBottomCenter);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRect, screenPoint, null, out Vector2 localPoint);

        Vector2 position = _rect.anchoredPosition;
        position.x = 0f;
        position.y = localPoint.y + _bottomMarginPixels;
        _rect.anchoredPosition = position;
    }
}