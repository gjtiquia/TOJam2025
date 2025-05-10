using UnityEngine;
using UnityEngine.Assertions;

public class PickupController : MonoBehaviour
{
    [SerializeField] private Transform _pickupAnchor;

    private IngredientController _currentIngredient = null;

    public void PickupIngredient(IngredientController ingredient)
    {
        if (TryDropCurrentIngredient(out var droppedIngredient))
        {
            // var direction = Random.onUnitSphere; // a bit too chaotic
            var direction = (_pickupAnchor.position - transform.position).normalized;
            droppedIngredient.Throw(direction);
        }

        Assert.IsNotNull(_pickupAnchor);

        ingredient.transform.SetParent(_pickupAnchor);
        ingredient.transform.localPosition = Vector3.zero;
        // ingredient.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity); // Not setting the rotation seems more fun and chaotic😂

        ingredient.OnPickup();

        _currentIngredient = ingredient;
    }

    public bool TryDropCurrentIngredient(out IngredientController droppedIngredient)
    {
        if (_currentIngredient == null)
        {
            droppedIngredient = null;
            return false;
        }

        droppedIngredient = _currentIngredient;

        _currentIngredient.transform.SetParent(null);
        _currentIngredient.OnDrop();
        _currentIngredient = null;

        return true;
    }
}