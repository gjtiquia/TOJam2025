using UnityEngine;

public class DeveloperTools : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("DeveloperTools: RandomlySpawnIngredient");
            IngredientSpawner.Instance.RandomlySpawnIngredient();
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            Debug.Log("DeveloperTools: RandomlyAddOrder");
            CustomerOrderListController.Instance.RandomlyAddOrder();
        }
    }
}