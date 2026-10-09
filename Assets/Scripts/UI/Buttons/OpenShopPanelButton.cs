using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class OpenShopPanelButton : MonoBehaviour
{
    [SerializeField] private GameObject _shopPanel;
    
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(Open);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(Open);
    }

    private void Open()
    {
        _shopPanel.SetActive(true);
    }
}
