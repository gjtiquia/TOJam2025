using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Assertions;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameSettingsSO _settingsSO;

    private bool _isSpawningIngredients;
    private bool _isSpawningOrders;

    private void Start()
    {
        // leaving the flexibility to start game after title screen
        StartGame();
    }

    public void StartGame()
    {
        Assert.IsNotNull(_settingsSO);
        SpawnIngredientsLoopAsync().Forget();
        SpawnCustomerOrdersLoopAsync().Forget();
    }

    private async UniTask SpawnIngredientsLoopAsync()
    {
        _isSpawningIngredients = true;
        while (_isSpawningIngredients)
        {
            IngredientSpawner.Instance.RandomlySpawnIngredient();
            await UniTask.Delay(TimeSpan.FromSeconds(_settingsSO.IngredientSpawnInterval));
        }
    }

    private async UniTask SpawnCustomerOrdersLoopAsync()
    {
        _isSpawningOrders = true;
        while (_isSpawningOrders)
        {
            CustomerOrderListController.Instance.TryRandomlyAddOrder();
            await UniTask.Delay(TimeSpan.FromSeconds(_settingsSO.CustomerOrderSpawnInterval));
        }
    }
}