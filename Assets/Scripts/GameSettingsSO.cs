using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettingsSO", menuName = "ScriptableObjects/GameSettingsSO")]
public class GameSettingsSO : ScriptableObject
{
    [Header("Settings")]
    [Min(1)] public int GameDuration;

    [Header("Spawn Intervals")]
    [Min(0)] public float IngredientSpawnInterval;
    [Min(0)] public float CustomerOrderSpawnInterval;
}