using TMPro;
using UnityEngine;

public class QueueCounterView : MonoBehaviour
{
    [SerializeField] private QueueStickmenCounter _counter;
    [SerializeField] private TMP_Text _counterText;

    private void OnEnable()
    {
        _counter.CountChanged += UpdateText;
        UpdateText(_counter.Count);
    }

    private void OnDisable()
    {
        _counter.CountChanged -= UpdateText;
    }

    private void UpdateText(int count)
    {
        _counterText.text = count.ToString();
    }
}
