using UnityEngine;

[CreateAssetMenu(fileName = "MovementSettingsSO", menuName = "ScriptableObjects/MovementSettingsSO")]
public class MovementSettingsSO : ScriptableObject
{
    [Min(0)] public float Speed;
    [Min(0)] public float RotationSpeed;
}