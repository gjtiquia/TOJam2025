using UnityEngine;
using UnityEngine.Assertions;

public class PickupController : MonoBehaviour
{
    [SerializeField] private Transform _pickupAnchor;

    private IngredientController _currentIngredient = null;

    public void PickupIngredient(IngredientController ingredient)
    {
        TryDropCurrentIngredient();

        Assert.IsNotNull(_pickupAnchor);

        ingredient.transform.SetParent(_pickupAnchor);
        ingredient.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        ingredient.OnPickup();

        _currentIngredient = ingredient;
    }

    public bool TryDropCurrentIngredient()
    {
        if (_currentIngredient == null)
            return false;

        _currentIngredient.transform.SetParent(null);
        _currentIngredient.OnDrop();
        return true;
    }
}