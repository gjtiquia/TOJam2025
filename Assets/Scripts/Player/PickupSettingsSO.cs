using UnityEngine;

[CreateAssetMenu(fileName = "PickupSettingsSO", menuName = "ScriptableObjects/PickupSettingsSO")]
public class PickupSettingsSO : ScriptableObject
{
    public static PickupSettingsSO Instance;

    [Min(0)] public float ThrowForce;
}