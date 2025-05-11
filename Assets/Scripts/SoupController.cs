using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public interface ISoupData
{
    public List<EFlavour> Flavours { get; }
}

public class SoupController : MonoBehaviour, IPickupItem, IInteractable, IFlavourUIParent
{
    public enum EState
    {
        Idle,
        PickedUp,
    }

    public List<EFlavour> Flavours => _flavours;

    [Header("Prefabs")]
    [SerializeField] private FlavourUIController _flavourUIPrefab;

    [Header("References")]
    [SerializeField] private GameObject _hoverVisual;

    private Rigidbody _rigidbody;
    private Collider _collider;

    private EState _state = EState.Idle;

    private List<EFlavour> _flavours = new();

    private FlavourUIController _uiInstance;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponentInChildren<Collider>();

        Assert.IsNotNull(_rigidbody);
        Assert.IsNotNull(_collider);
        Assert.IsNotNull(_hoverVisual);

        SetIsHoveredState(false);
    }

    private void Start()
    {
        LazySpawnOrUpdateUI();
    }

    private void LazySpawnOrUpdateUI()
    {
        if (_uiInstance == null)
        {
            Assert.IsNotNull(_flavourUIPrefab);
            _uiInstance = Instantiate(_flavourUIPrefab);
            _uiInstance.Init(this);
        }
        else
        {
            _uiInstance.UpdateFlavours();
        }
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

        // cuz apparently Start is not called yet
        LazySpawnOrUpdateUI();
    }

    public ISoupData Consume()
    {
        var soupData = SoupData.Create(_flavours);

        // TODO : object pool maybe
        Destroy(_uiInstance.gameObject);
        Destroy(this.gameObject);

        return soupData;
    }

    public bool CanShowFlavourUI()
    {
        return _hoverVisual.activeSelf;
    }

    // HELPER CLASSES
    private struct SoupData : ISoupData
    {
        public List<EFlavour> Flavours { get; private set; }

        public static SoupData Create(List<EFlavour> flavours)
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