using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class SoupController : MonoBehaviour, IPickupItem, IInteractable
{
    public enum EState
    {
        Idle,
        PickedUp,
    }

    [SerializeField] private GameObject _hoverVisual;

    private Rigidbody _rigidbody;
    private Collider _collider;

    private EState _state = EState.Idle;

    private List<EFlavour> _flavours = new();

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponentInChildren<Collider>();

        Assert.IsNotNull(_rigidbody);
        Assert.IsNotNull(_collider);
        Assert.IsNotNull(_hoverVisual);

        SetIsHoveredState(false);
    }

    public bool IsInteractable(IInteractContext context)
    {
        return _state == EState.Idle;
    }

    public void SetIsHoveredState(bool isHovered)
    {
        _hoverVisual.SetActive(isHovered);
    }

    public void Interact(IInteractContext context)
    {
        if (context.InteractInstigator.GetComponent<PickupController>() is not PickupController pickupController)
            return;

        pickupController.PickupItem(this);
    }

    public void OnPickup()
    {
        _rigidbody.isKinematic = true;
        _collider.enabled = false;
        _state = EState.PickedUp;
    }

    public void OnDrop()
    {
        _rigidbody.isKinematic = false;
        _collider.enabled = true;
        _state = EState.Idle;
    }

    public void Throw(Vector3 normalizedDirection)
    {
        _rigidbody.AddForce(normalizedDirection * PickupSettingsSO.Instance.ThrowForce, ForceMode.Impulse);
    }

    public void SetFlavours(List<EFlavour> flavours)
    {
        Debug.Log($"Soup.SetFlavours: flavour count: {flavours.Count}");

        // Clone to prevent issues with references
        _flavours.Clear();
        foreach (var flavour in flavours)
            _flavours.Add(flavour);
    }
}