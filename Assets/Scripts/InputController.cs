using UnityEngine;

public struct InputStruct
{
    public bool IsUpPressed;
    public bool IsDownPressed;
    public bool IsLeftPressed;
    public bool IsRightPressed;
    public bool IsInteractPressed;

    public static InputStruct Create()
    {
        return new()
        {
            IsUpPressed = false,
            IsDownPressed = false,
            IsLeftPressed = false,
            IsRightPressed = false,
            IsInteractPressed = false,
        };
    }
}

public class InputController : MonoBehaviour
{
    public InputStruct CachedInput => _cachedInput;
    private InputStruct _cachedInput;

    public void PollandCacheInputOnUpdate()
    {
        var inputStruct = InputStruct.Create();

        inputStruct.IsUpPressed = Input.GetKey(KeyCode.W);
        inputStruct.IsDownPressed = Input.GetKey(KeyCode.S);
        inputStruct.IsLeftPressed = Input.GetKey(KeyCode.A);
        inputStruct.IsRightPressed = Input.GetKey(KeyCode.D);

        inputStruct.IsInteractPressed = Input.GetKey(KeyCode.F);

        _cachedInput = inputStruct;
    }
}