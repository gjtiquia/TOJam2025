using System.Collections.Generic;
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

        const int MAX_FLAVOURS = 3;
        var flavourCount = Random.Range(1, MAX_FLAVOURS + 1);

        var flavours = new List<EFlavour>();
        while (flavours.Count < flavourCount)
        {
            var randomFlavour = (EFlavour)Random.Range(0, (int)EFlavour.COUNT);
            if (flavours.Contains(randomFlavour)) continue;

            flavours.Add(randomFlavour);
        }

        customerOrderInstance.SetFlavours(flavours);
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