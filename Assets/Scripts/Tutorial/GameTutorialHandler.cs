using UnityEngine;

[RequireComponent(typeof(TutorialPointer),  typeof(TutorialPopup))]
public class GameTutorialHandler : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private RectTransform _canvasRect;
    [SerializeField] private Transform _targetWorldAnchor;
    [SerializeField] private BusClickController _busClickController;
    
    private TutorialPointer _pointer;
    private TutorialPopup _popup;
    private BusSelector _busSelector;

    private void Awake()
    {
        _pointer = GetComponent<TutorialPointer>();
        _popup = GetComponent<TutorialPopup>();
    }

    private void Start()
    {
        _busSelector = _busClickController.Selector;
        
        Vector2 screenPosition = WorldToCanvasPosition(_targetWorldAnchor.position);
        _pointer.SetBasePosition(screenPosition);
        
        _popup.Show();
        _busSelector.BusSelected += OnBusSelected;
    }

    private void OnBusSelected(Bus bus)
    {
        _busSelector.BusSelected -= OnBusSelected;
        _popup.Hide();
        gameObject.SetActive(false);
    }

    private Vector2 WorldToCanvasPosition(Vector3 worldPosition)
    {
        Vector2 screenPoint = _camera.WorldToScreenPoint(worldPosition);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRect, screenPoint, null, out Vector2 localPoint);
        
        return localPoint;
    }
}
