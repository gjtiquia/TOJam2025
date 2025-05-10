using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public enum EFlavour
{
    Sweet = 0,
    Spicy = 1,
    Salty = 2,
    Sour = 3,
    Creamy = 4,
}

public interface IIngredientData
{
    public List<EFlavour> Flavours { get; }
}

public class IngredientController : MonoBehaviour, IInteractable, IPickupItem
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

    public bool IsInteractable(IInteractContext context)
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

    public IIngredientData Consume()
    {
        var ingredientData = IngredientData.Create(_settingsSO.Flavours);

        // TODO : object pool maybe
        Destroy(this.gameObject);

        return ingredientData;
    }

    // HELPER CLASSES
    private struct IngredientData : IIngredientData
    {
        public List<EFlavour> Flavours { get; private set; }

        public static IngredientData Create(List<EFlavour> flavours)
        {
            // Clone in-case the original reference is modified or destroyed
            var flavourListClone = new List<EFlavour>();
            foreach (var flavour in flavours)
                flavourListClone.Add(flavour);

            return new()
            {
                Flavours = flavourListClone,
            };
        }
    }
}