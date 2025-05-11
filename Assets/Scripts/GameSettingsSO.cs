using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettingsSO", menuName = "ScriptableObjects/GameSettingsSO")]
public class GameSettingsSO : ScriptableObject
{
    public float IngredientSpawnInterval;
    public float CustomerOrderSpawnInterval;
}