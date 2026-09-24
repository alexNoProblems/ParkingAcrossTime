using UnityEngine;

public class TutorialPointer : MonoBehaviour
{
    [SerializeField] private RectTransform _handSprite;
    [SerializeField] private float _bounceDistance = 20f;
    [SerializeField] private float _bounceSpeed = 3f;
    [SerializeField] private float _offsetX = 0f;
    [SerializeField] private float _offsetY = 0f;

    private Vector2 _basePosition;

    private void Update()
    {
        float bounce = Mathf.Sin(Time.time * _bounceSpeed) * _bounceDistance;

        Vector2 position = _basePosition + new Vector2(0f, bounce);
        _handSprite.anchoredPosition = position;
    }

    public void SetBasePosition(Vector2 anchoredPosition)
    {
        _basePosition = new Vector2(anchoredPosition.x + _offsetX, anchoredPosition.y + _offsetY);
    }
}