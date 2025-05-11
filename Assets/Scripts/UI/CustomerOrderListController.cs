using UnityEngine;

public class CustomerOrderListController : MonoBehaviour
{
    public static CustomerOrderListController Instance;

    [SerializeField] private CustomerOrderController _customerOrderPrefab;

    private void Awake()
    {
        Instance = this;
    }

    public void RandomlyAddOrder()
    {
        var customerOrderInstance = Instantiate(_customerOrderPrefab, this.transform);

        // TODO : init flavours
    }

    public void FulfillOrder(ISoupData soupData)
    {
        CustomerOrderController matchedOrder = null;
        foreach (var order in GetComponentsInChildren<CustomerOrderController>())
        {
            // TODO : check which order matches
            matchedOrder = order;
            break;
        }

        if (matchedOrder == null)
            return;

        // TODO : object pool?
        Destroy(matchedOrder.gameObject);

        // TODO : score
    }
}