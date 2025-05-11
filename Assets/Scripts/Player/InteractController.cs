using UnityEngine;
using UnityEngine.Assertions;

public interface IInteractable
{
    public bool IsInteractable(IInteractContext context);
    public void SetIsHoveredState(bool isHovered);
    public void Interact(IInteractContext context);
}

public interface IInteractContext
{
    public GameObject InteractInstigator { get; }
}

public class InteractController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private LayerMask _layermask;

    [Header("References")]
    [SerializeField] private SphereCollider _interactTriggerCollider;

    const int MAX_COLLIDERS = 10;
    private Collider[] _hitColliders = new Collider[MAX_COLLIDERS];
    private IInteractable _nearestInteractable = null;

    private bool _wasInteractPressed = false; // Prevent interact from calling for each tick interact is pressed

    private IInteractContext _interactContext;

    private void Awake()
    {
        _interactContext = new InteractContext(this.gameObject);
    }

    public void UpdateNearestInteractableOnFixedUpdate()
    {
        Assert.IsNotNull(_interactTriggerCollider);

        if (_nearestInteractable != null && !_nearestInteractable.IsInteractable(_interactContext))
        {
            _nearestInteractable.SetIsHoveredState(false);
            _nearestInteractable = null;
        }

        var hitCount = Physics.OverlapSphereNonAlloc(_interactTriggerCollider.transform.position, _interactTriggerCollider.radius, _hitColliders, _layermask);
        if (hitCount == 0)
        {
            if (_nearestInteractable != null)
            {
                _nearestInteractable.SetIsHoveredState(false);
                _nearestInteractable = null;
            }
            return;
        }

        float nearestSqrDistance = float.MaxValue;
        IInteractable newNearestInteractable = null;
        for (int i = 0; i < hitCount; i++)
        {
            var collider = _hitColliders[i];

            var interactable = collider.GetComponentInParent<IInteractable>();
            if (interactable == null || !interactable.IsInteractable(_interactContext)) continue;

            var distanceVector = collider.transform.position - _interactTriggerCollider.transform.position;
            var sqrDistance = distanceVector.sqrMagnitude;

            if (sqrDistance < nearestSqrDistance)
            {
                nearestSqrDistance = sqrDistance;
                newNearestInteractable = interactable;
            }
        }

        // Note : careful, newNearestInteractable may be null (eg. it is not interactable)

        if (_nearestInteractable != null && _nearestInteractable == newNearestInteractable)
        {
            // Do nothing, because the nearest interactable is the same
        }
        else
        {
            if (_nearestInteractable != null)
                _nearestInteractable.SetIsHoveredState(false);

            if (newNearestInteractable != null)
                newNearestInteractable.SetIsHoveredState(true);
        }

        _nearestInteractable = newNearestInteractable;
    }

    public void ConsumeInputOnFixedUpdate(InputStruct input)
    {
        if (_wasInteractPressed && !input.IsInteractPressed)
            _wasInteractPressed = false;

        if (!_wasInteractPressed && input.IsInteractPressed)
        {
            _wasInteractPressed = true;

            if (_nearestInteractable != null && _nearestInteractable.IsInteractable(_interactContext))
            {
                _nearestInteractable.Interact(_interactContext);
            }
            else
            {
                OnInteractPressedFallback();
            }
        }
    }

    private void OnInteractPressedFallback()
    {
        var pickupController = GetComponent<PickupController>();
        Assert.IsNotNull(pickupController);

        // First try to drop existing ingredient
        if (pickupController.TryDropCurrentItem(out _))
            return;

        // Add other fallback handling here
    }

    // HELPERS
    private class InteractContext : IInteractContext
    {
        public GameObject InteractInstigator { get; private set; }

        public InteractContext(GameObject instigator)
        {
            InteractInstigator = instigator;
        }
    }
}
