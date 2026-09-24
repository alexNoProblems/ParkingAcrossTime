using UnityEngine;

public class AdaptivePopupPosition : MonoBehaviour
{
    [SerializeField] private RectTransform _rect;
    [SerializeField] private RectTransform _canvasRect;
    [SerializeField] private float _bottomMarginFraction = -0.35f;

    private int _lastScreenWidth;
    private int _lastScreenHeight;

    private void Start()
    {
        Apply();
    }

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
        float margin = _canvasRect.rect.height * _bottomMarginFraction;

        Vector2 position = _rect.anchoredPosition;
        position.x = 0f;
        position.y = margin;
        _rect.anchoredPosition = position;
    }
}