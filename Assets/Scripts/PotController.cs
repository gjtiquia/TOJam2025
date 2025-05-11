using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PotController : MonoBehaviour, IInteractable, IFlavourUIParent
{
    public List<EFlavour> Flavours => GetFlavours();

    [Header("Prefabs")]
    [SerializeField] private GameObject _soupPrefab;

    [Header("References")]
    [SerializeField] private GameObject _hoverVisual;
    [SerializeField] private GameObject _potSoupVisual;
    [SerializeField] private FlavourUIController _flavourUI;

    private List<IIngredientData> _soupIngredients = new();

    private void Awake()
    {
        Assert.IsNotNull(_hoverVisual);
        Assert.IsNotNull(_soupPrefab);
        Assert.IsNotNull(_potSoupVisual);
        Assert.IsNotNull(_flavourUI);

        SetIsHoveredState(false);
        _flavourUI.Init(this);

        ResetPot();
    }

    public void SetIsHoveredState(bool isHovered)
    {
        _hoverVisual.SetActive(isHovered);
    }

    public bool IsInteractable(IInteractContext context)
    {
        // The order should mimic Interact()

        var pickupController = context.InteractInstigator.GetComponent<PickupController>();
        Assert.IsNotNull(pickupController);

        if (CanDropIngredient(pickupController))
            return true;

        if (CanPickupSoup(pickupController))
            return true;

        // false by default
        return false;
    }

    public void Interact(IInteractContext context)
    {
        // The order should mimic IsInteractable()

        var pickupController = context.InteractInstigator.GetComponent<PickupController>();
        Assert.IsNotNull(pickupController);

        if (CanDropIngredient(pickupController))
        {
            DropIngredient(pickupController);
            return;
        }

        if (CanPickupSoup(pickupController))
        {
            PickupSoup(pickupController);
            return;
        }
    }

    private bool CanDropIngredient(PickupController pickupController)
    {
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
            _potSoupVisual.SetActive(true);
            _flavourUI.UpdateFlavours();

            Debug.Log($"Pot.DropIngredient: total ingredients: {_soupIngredients.Count}");
        }
    }

    private bool CanPickupSoup(PickupController pickupController)
    {
        // allow to pickup soup even if holding something
        // will simply "swap" the holding object

        return _soupIngredients.Count > 0;
    }

    private void PickupSoup(PickupController pickupController)
    {
        Debug.Log($"Pot.PickupSoup: total ingredients: {_soupIngredients.Count}");

        var soupInstance = Instantiate(_soupPrefab);
        var soup = soupInstance.GetComponent<SoupController>();

        var flavours = GetFlavours();
        soup.SetFlavours(flavours);

        pickupController.PickupItem(soup); // this will automatically throw any held item (if any)

        ResetPot();
    }

    public void ResetPot()
    {
        _soupIngredients.Clear();
        _potSoupVisual.SetActive(false);
        _flavourUI.UpdateFlavours();
    }

    private List<EFlavour> GetFlavours()
    {
        var flavours = new List<EFlavour>();
        foreach (var ingredient in _soupIngredients)
        {
            foreach (var flavour in ingredient.Flavours)
            {
                if (!flavours.Contains(flavour))
                    flavours.Add(flavour);
            }
        }
        return flavours;
    }

    public bool CanShowFlavourUI()
    {
        return _hoverVisual.activeSelf;
    }
}