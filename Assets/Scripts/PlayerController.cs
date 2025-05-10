using UnityEngine;
using UnityEngine.Assertions;

public class PlayerController : MonoBehaviour
{
    private InputController _inputController;
    private MovementController _movementController;

    private void Awake()
    {
        _inputController = GetComponent<InputController>();
        _movementController = GetComponent<MovementController>();

        Assert.IsNotNull(_inputController);
        Assert.IsNotNull(_movementController);
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
    }
}

