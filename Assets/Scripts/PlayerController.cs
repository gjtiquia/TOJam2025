using UnityEngine;
using UnityEngine.Assertions;

public class PlayerController : MonoBehaviour
{
    private InputController _inputController;
    private MovementController _movementController;
    private InteractController _interactController;

    private void Awake()
    {
        _inputController = GetComponent<InputController>();
        Assert.IsNotNull(_inputController);

        _movementController = GetComponent<MovementController>();
        Assert.IsNotNull(_movementController);

        _interactController = GetComponent<InteractController>();
        Assert.IsNotNull(_interactController);
    }

    private void Update()
    {
        // Check on each tick
        _inputController.PollandCacheInputOnUpdate();
    }

    private void FixedUpdate()
    {
        var input = _inputController.CachedInput;

        // Rigidbody is part of physics loop and should be called onFixedUpdate
        _movementController.ConsumeInputOnFixedUpdate(input);

        // Update before consuming input.
        // onFixedUpdate because uses physics raycast
        _interactController.UpdateNearestInteractableOnFixedUpdate();

        // Consume onFixedUpdate just in-case not to mess up with any physics
        _interactController.ConsumeInputOnFixedUpdate(input);
    }
}

