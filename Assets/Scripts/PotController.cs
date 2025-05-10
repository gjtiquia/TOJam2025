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
        if (context.InteractInstigator.GetComponent<PickupController>() is PickupController pickupController)
        {
            if (pickupController.IsHoldingAnIngredient())
                return true;
        }

        // TODO : take out soup handling

        // false by default
        return false;
    }

    public void Interact(IInteractContext context)
    {
        if (context.InteractInstigator.GetComponent<PickupController>() is PickupController pickupController)
        {
            if (pickupController.TryDropCurrentIngredient(out var ingredientInstance))
            {
                var ingredientData = ingredientInstance.Consume();
                _soupIngredients.Add(ingredientData);
                return;
            }
        }
    }
}