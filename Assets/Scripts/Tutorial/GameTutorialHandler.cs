using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TutorialPointer), typeof(TutorialPopup))]
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
    private bool _isReady;
    private bool _isCompleted;
    private bool _isSuspended;

    private void Awake()
    {
        _pointer = GetComponent<TutorialPointer>();
        _popup = GetComponent<TutorialPopup>();
    }

    private void Start()
    {
        StartCoroutine(WaitForBusesThenStart());
    }

    public void Suspend()
    {
        if (_isCompleted || _isSuspended)
            return;

        _isSuspended = true;

        if (_isReady)
            HideTutorial();
    }

    public void Resume()
    {
        if (_isCompleted || !_isSuspended)
            return;

        _isSuspended = false;

        if (_isReady)
            ShowTutorial();
    }

    private IEnumerator WaitForBusesThenStart()
    {
        Transform target = null;

        while (target == null)
        {
            if (AllLanesReady() && _stickmanQueue.PeekFront() != null)
                target = FindTargetAnchor();

            if (target == null)
                yield return null;
        }

        _busSelector = _busClickController.Selector;

        Vector2 screenPosition = WorldToCanvasPosition(target.position);
        _pointer.SetBasePosition(screenPosition);

        _busSelector.BusSelected += OnBusSelected;
        _isReady = true;

        if (!_isSuspended)
            ShowTutorial();
    }

    private void ShowTutorial()
    {
        gameObject.SetActive(true);
        _popup.Show();
    }

    private void HideTutorial()
    {
        _popup.Hide();
        gameObject.SetActive(false);
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
        _isCompleted = true;
        HideTutorial();
    }

    private Vector2 WorldToCanvasPosition(Vector3 worldPosition)
    {
        Vector2 screenPoint = _camera.WorldToScreenPoint(worldPosition);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRect, screenPoint, null, out Vector2 localPoint);

        return localPoint;
    }
}