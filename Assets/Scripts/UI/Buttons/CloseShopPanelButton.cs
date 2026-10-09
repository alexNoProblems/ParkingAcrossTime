using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CloseShopPanelButton : MonoBehaviour
{
    [SerializeField] private GameObject _shopPanel;
    
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(Close);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(Close);
    }

    private void Close()
    {
        _shopPanel.SetActive(false);
    }
}