using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class OrdersFilledUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    public void Start()
    {
        UpdateText();
        CustomerOrderListController.OnFulfilledOrdersChanged += UpdateText;
    }

    public void OnDestroy()
    {
        CustomerOrderListController.OnFulfilledOrdersChanged -= UpdateText;
    }

    private void UpdateText()
    {
        Assert.IsNotNull(_text);
        _text.text = $"Orders Filled: {CustomerOrderListController.Instance.FulfilledOrders}";
    }
}