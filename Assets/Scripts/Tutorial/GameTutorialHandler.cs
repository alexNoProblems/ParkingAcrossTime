using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TutorialPointer),  typeof(TutorialPopup))]
public class GameTutorialHandler : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private RectTransform _canvasRect;
    [SerializeField] private BusClickController _busClickController;
    [SerializeField] private StickmanQueue _stickmanQueue;
    [SerializeField] private List<BusLane> _lanes;

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
        StartCoroutine(WaitForBusesThenStart());
    }

    private IEnumerator WaitForBusesThenStart()
    {
        while (!AllLanesReady() || _stickmanQueue.PeekFront() == null)
            yield return null;

        _busSelector = _busClickController.Selector;

        Transform target = FindTargetAnchor();
        Vector2 screenPosition = WorldToCanvasPosition(target.position);
        _pointer.SetBasePosition(screenPosition);

        _popup.Show();
        _busSelector.BusSelected += OnBusSelected;
    }

    private bool AllLanesReady()
    {
        foreach (BusLane lane in _lanes)
        {
            if (lane.BusesInLane.Count == 0)
                return false;
        }

        return true;
    }

    private Transform FindTargetAnchor()
    {
        Stickman front = _stickmanQueue.PeekFront();

        foreach (BusLane lane in _lanes)
        {
            Bus frontBus = lane.BusesInLane[0];

            if (frontBus.Color == front.Color)
                return frontBus.transform;
        }

        return null;
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