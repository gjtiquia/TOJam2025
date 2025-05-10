using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IngredientSettingsSO", menuName = "ScriptableObjects/IngredientSettingsSO")]
public class IngredientSettingsSO : ScriptableObject
{
    public List<EFlavour> Flavours;
}