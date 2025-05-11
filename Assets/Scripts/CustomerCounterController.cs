using UnityEngine;
using UnityEngine.Assertions;

public class CustomerCounterController : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject _hoverVisual;

    public void SetIsHoveredState(bool isHovered)
    {
        Assert.IsNotNull(_hoverVisual);
        _hoverVisual.SetActive(isHovered);
    }

    public bool IsInteractable(IInteractContext context)
    {
        var pickupController = context.InteractInstigator.GetComponent<PickupController>();
        Assert.IsNotNull(pickupController);

        if (!pickupController.IsHoldingAnItem())
            return false;

        if (pickupController.GetCurrentPickupItem() is not SoupController)
            return false;

        return true;
    }

    public void Interact(IInteractContext context)
    {
        var pickupController = context.InteractInstigator.GetComponent<PickupController>();

        var success = pickupController.TryDropCurrentItem(out var droppedItem);
        Assert.IsTrue(success);

        var soup = droppedItem as SoupController;
        Assert.IsNotNull(soup);

        var soupData = soup.Consume();

        // TODO : check what order is fulfilled
    }

}