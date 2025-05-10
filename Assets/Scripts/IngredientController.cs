using UnityEngine;
using UnityEngine.Assertions;

public class IngredientController : MonoBehaviour, IInteractable
{
    public enum EState
    {
        Idle,
        PickedUp,
    }

    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Material _idleMaterial;
    [SerializeField] private Material _hoverMaterial;

    private Rigidbody _rigidbody;
    private Collider _collider;

    private EState _state = EState.Idle;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponentInChildren<Collider>();

        Assert.IsNotNull(_rigidbody);
        Assert.IsNotNull(_collider);

        Assert.IsNotNull(_meshRenderer);
        Assert.IsNotNull(_idleMaterial);
        Assert.IsNotNull(_hoverMaterial);

        _meshRenderer.material = _idleMaterial;
    }

    public bool IsInteractable()
    {
        return _state == EState.Idle;
    }

    public void SetIsHoveredState(bool isHovered)
    {
        // TODO : temp handling
        _meshRenderer.material = isHovered ? _hoverMaterial : _idleMaterial;
    }

    public void Interact(IInteractContext context)
    {
        Debug.Log("Ingredient: Interact");

        if (context.InteractInstigator.GetComponent<PickupController>() is not PickupController pickupController)
            return;

        pickupController.PickupIngredient(this);
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
}