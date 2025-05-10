using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PotController : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject _hoverVisual;

    private List<IIngredientData> _soupIngredients = new();

    private void Awake()
    {
        Assert.IsNotNull(_hoverVisual);
        SetIsHoveredState(false);
    }

    public void SetIsHoveredState(bool isHovered)
    {
        _hoverVisual.SetActive(isHovered);
    }

    public bool IsInteractable(IInteractContext context)
    {
        // The order should mimic Interact()

        if (CanDropIngredient(context, out _))
            return true;

        if (CanPickupSoup())
            return true;

        // false by default
        return false;
    }

    public void Interact(IInteractContext context)
    {
        // The order should mimic IsInteractable()

        if (CanDropIngredient(context, out var pickupController))
        {
            DropIngredient(pickupController);
            return;
        }

        if (CanPickupSoup())
        {
            PickupSoup();
            return;
        }
    }

    private bool CanDropIngredient(IInteractContext context, out PickupController pickupController)
    {
        pickupController = context.InteractInstigator.GetComponent<PickupController>();
        if (pickupController == null) return false;

        var isHoldingAnItem = pickupController.IsHoldingAnItem();
        if (!isHoldingAnItem) return false;

        var isAnIngredient = pickupController.GetCurrentPickupItem() is IngredientController;
        if (!isAnIngredient) return false;

        return true;
    }

    private void DropIngredient(PickupController pickupController)
    {
        if (pickupController.TryDropCurrentItem(out var droppedItem))
        {
            var ingredientData = ((IngredientController)droppedItem).Consume();
            _soupIngredients.Add(ingredientData);

            Debug.Log($"Pot.DropIngredient: total ingredients: {_soupIngredients.Count}");
        }
    }

    private bool CanPickupSoup()
    {
        // TODO
        return _soupIngredients.Count > 0;
    }

    private void PickupSoup()
    {
        Debug.Log($"Pot.PickupSoup: total ingredients: {_soupIngredients.Count}");

        // TODO
        _soupIngredients.Clear();
    }
}