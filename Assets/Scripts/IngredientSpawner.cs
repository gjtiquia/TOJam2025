using UnityEngine;

public class IngredientSpawner : MonoBehaviour
{
    public static IngredientSpawner Instance;

    [Header("Prefabs")]
    [SerializeField] private GameObject _ingredientPrefab;

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

        Instantiate(_ingredientPrefab, randPos, Quaternion.identity);
    }
}