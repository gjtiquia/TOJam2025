using UnityEngine;

[CreateAssetMenu(fileName = "IngredientSettingsSO", menuName = "ScriptableObjects/IngredientSettingsSO")]
public class IngredientSettingsSO : ScriptableObject
{
    [Min(0)] public float ThrowForce;
}