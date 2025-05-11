using System;
using System.Collections.Generic;
using UnityEngine;

public class CustomerOrderListController : MonoBehaviour
{
    public static event Action OnFulfilledOrdersChanged;
    public static CustomerOrderListController Instance;

    public int FulfilledOrders => _fulfilledOrders;

    [SerializeField] private CustomerOrderController _customerOrderPrefab;

    private int _fulfilledOrders { get => m_fulfilledOrders; set { m_fulfilledOrders = value; OnFulfilledOrdersChanged?.Invoke(); } }
    private int m_fulfilledOrders = 0;

    private void Awake()
    {
        Instance = this;
        Reset();
    }

    public bool TryRandomlyAddOrder()
    {
        const int MAX_ORDERS = 5;
        var currentOrders = GetComponentsInChildren<CustomerOrderController>();
        if (currentOrders.Length >= MAX_ORDERS)
            return false;

        var customerOrderInstance = Instantiate(_customerOrderPrefab, this.transform);

        const int MAX_FLAVOURS = 3;
        var flavourCount = UnityEngine.Random.Range(1, MAX_FLAVOURS + 1);

        var flavours = new List<EFlavour>();
        while (flavours.Count < flavourCount)
        {
            var randomFlavour = (EFlavour)UnityEngine.Random.Range(0, (int)EFlavour.COUNT);
            if (flavours.Contains(randomFlavour)) continue;

            flavours.Add(randomFlavour);
        }

        customerOrderInstance.SetFlavours(flavours);
        return true;
    }

    public bool TryFulfillOrder(ISoupData soupData)
    {
        CustomerOrderController matchedOrder = null;
        foreach (var order in GetComponentsInChildren<CustomerOrderController>())
        {
            if (order.IsFlavourMatched(soupData.Flavours))
            {
                matchedOrder = order;
                break;
            }
        }

        if (matchedOrder == null)
            return false;

        // TODO : object pool?
        Destroy(matchedOrder.gameObject);

        _fulfilledOrders++;
        // TODO : score

        return true;
    }

    public void Reset()
    {
        _fulfilledOrders = 0;
        foreach (var order in GetComponentsInChildren<CustomerOrderController>())
            Destroy(order.gameObject);
    }
}