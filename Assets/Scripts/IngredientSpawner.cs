using System.Collections.Generic;
using UnityEngine;

public class IngredientSpawner : MonoBehaviour
{
    public static IngredientSpawner Instance;

    [Header("Prefabs")]
    [SerializeField] private List<IngredientController> _ingredientPrefabs;

    [Header("References")]
    [SerializeField] private BoxCollider _boxCollider;

    private List<int> _previouslySpawnedIndexes = new(); // ensures each prefab gets spawned once before resetting the "pool" again

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

        if (_previouslySpawnedIndexes.Count >= _ingredientPrefabs.Count)
            _previouslySpawnedIndexes.Clear();

        var randomIndex = Random.Range(0, _ingredientPrefabs.Count);
        while (_previouslySpawnedIndexes.Contains(randomIndex))
            randomIndex = Random.Range(0, _ingredientPrefabs.Count);

        _previouslySpawnedIndexes.Add(randomIndex);

        var randomPrefab = _ingredientPrefabs[randomIndex];
        var instance = Instantiate(randomPrefab, randPos, Quaternion.identity);

        var randomDirection = Random.onUnitSphere;
        instance.Throw(randomDirection);
    }
}