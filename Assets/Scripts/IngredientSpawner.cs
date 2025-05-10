using System.Collections.Generic;
using UnityEngine;

public class IngredientSpawner : MonoBehaviour
{
    public static IngredientSpawner Instance;

    [Header("Prefabs")]
    [SerializeField] private List<IngredientController> _ingredientPrefabs;

    [Header("References")]
    [SerializeField] private BoxCollider _boxCollider;

    private void Awake()
    {
        Instance = this;
    }

    public void RandomlySpawnIngredient()
    {
        var bounds = _boxCollider.bounds;

        var randX = Random.Range(bounds.min.x, bounds.max.x);
        var randY = Random.Range(bounds.min.y, bounds.max.y);
        var randZ = Random.Range(bounds.min.z, bounds.max.z);
        var randPos = new Vector3(randX, randY, randZ);

        var randomIndex = Random.Range(0, _ingredientPrefabs.Count);
        var randomPrefab = _ingredientPrefabs[randomIndex];

        var instance = Instantiate(randomPrefab, randPos, Quaternion.identity);

        var randomDirection = Random.onUnitSphere;
        instance.Throw(randomDirection);
    }
}