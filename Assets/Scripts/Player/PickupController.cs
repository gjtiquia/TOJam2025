using UnityEngine;
using UnityEngine.Assertions;

public interface IPickupItem
{
    public Transform transform { get; }
    public void OnPickup();
    public void OnDrop();
    public void Throw(Vector3 direction);
}

public class PickupController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private PickupSettingsSO _settingsSO;

    [Header("References")]
    [SerializeField] private Transform _pickupAnchor;

    private IPickupItem _currentPickupItem = null;

    private void Awake()
    {
        Assert.IsNotNull(_settingsSO);
        PickupSettingsSO.Instance = _settingsSO;
    }

    public bool IsHoldingAnItem()
    {
        return _currentPickupItem != null;
    }

    public IPickupItem GetCurrentPickupItem()
    {
        return _currentPickupItem;
    }

    public void PickupItem(IPickupItem item)
    {
        if (TryDropCurrentItem(out var droppedIngredient))
        {
            // var direction = Random.onUnitSphere; // a bit too chaotic
            var direction = (_pickupAnchor.position - transform.position).normalized;
            droppedIngredient.Throw(direction);
        }

        Assert.IsNotNull(_pickupAnchor);

        item.transform.SetParent(_pickupAnchor);
        item.transform.localPosition = Vector3.zero;
        // ingredient.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity); // Not setting the rotation seems more fun and chaotic😂

        item.OnPickup();

        _currentPickupItem = item;
    }

    public bool TryDropCurrentItem(out IPickupItem droppedItem)
    {
        if (_currentPickupItem == null)
        {
            droppedItem = null;
            return false;
        }

        droppedItem = _currentPickupItem;

        _currentPickupItem.transform.SetParent(null);
        _currentPickupItem.OnDrop();
        _currentPickupItem = null;

        return true;
    }
}