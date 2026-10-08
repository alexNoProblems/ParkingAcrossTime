using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;
using YG.Utils.Pay;

public class ShopProductView : MonoBehaviour
{
    [SerializeField] private string _productID;
    [SerializeField] private int _coins;
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _priceText;
    
    public string ProductID => _productID;
    public int Coins => _coins; 
    public Button Button => _button;

    public void RefreshPrice()
    {
        Purchase purchase = YG2.PurchaseByID(_productID);

        if (purchase != null)
            _priceText.text = purchase.price;
    }
}
