using UnityEngine;
using UnityEngine.Assertions;

public class IngredientController : MonoBehaviour, IInteractable
{
    public enum EState
    {
        Idle,
        PickedUp,
    }

    [Header("Settings")]
    [SerializeField] private IngredientSettingsSO _settingsSO;

    [Header("References")]
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Material _idleMaterial;
    [SerializeField] private Material _hoverMaterial;
    [SerializeField] private GameObject _hoverVisual;

    private Rigidbody _rigidbody;
    private Collider _collider;

    private EState _state = EState.Idle;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponentInChildren<Collider>();

        Assert.IsNotNull(_rigidbody);
        Assert.IsNotNull(_collider);

        Assert.IsNotNull(_settingsSO);
        Assert.IsNotNull(_meshRenderer);
        Assert.IsNotNull(_idleMaterial);
        Assert.IsNotNull(_hoverMaterial);
        Assert.IsNotNull(_hoverVisual);

        SetIsHoveredState(false);
    }

    public bool IsInteractable()
    {
        return _state == EState.Idle;
    }

    public void SetIsHoveredState(bool isHovered)
    {
        // _meshRenderer.material = isHovered ? _hoverMaterial : _idleMaterial; // prototype phase stuff
        _hoverVisual.SetActive(isHovered);
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

    public void Throw(Vector3 normalizedDirection)
    {
        _rigidbody.AddForce(normalizedDirection * _settingsSO.ThrowForce, ForceMode.Impulse);
    }
}