using UnityEngine;
using UnityEngine.Assertions;

public class MovementController : MonoBehaviour
{
    [SerializeField] private MovementSettingsSO _settingsSO;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        Assert.IsNotNull(_rigidbody);
    }

    public void ConsumeInputOnFixedUpdate(InputStruct input)
    {
        var directionVector = Vector3.zero;

        if (input.IsUpPressed) directionVector += Vector3.forward;
        if (input.IsDownPressed) directionVector += Vector3.back;
        if (input.IsLeftPressed) directionVector += Vector3.left;
        if (input.IsRightPressed) directionVector += Vector3.right;

        directionVector.Normalize();

        _rigidbody.velocity = directionVector * _settingsSO.Speed;
    }
}