using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Assertions;
using System;

public class GameManager : MonoBehaviour
{
    public static event Action OnSecondsLeftChanged;
    public static GameManager Instance;

    public int SecondsLeft => _secondsLeft;

    [Header("Settings")]
    [SerializeField] private GameSettingsSO _settingsSO;

    [Header("References")]
    [SerializeField] private GameObject _gameStartScreen;
    [SerializeField] private GameObject _gameEndScreen;

    private int _secondsLeft { get => m_secondsLeft; set { m_secondsLeft = value; OnSecondsLeftChanged?.Invoke(); } }
    private int m_secondsLeft;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Assert.IsNotNull(_gameStartScreen);
        _gameStartScreen.SetActive(true);
    }

    public async UniTask StartGameAsync()
    {
        // Cleanup
        ResetPlayerState(); // order matters! should drop items before destroying them
        DestroyAllIngredients();
        DestroyAllSoup();
        ResetCustomerOrderList();
        ResetAllPots();

        Assert.IsNotNull(_settingsSO);

        var ingredientCancelToken = new CancelToken();
        SpawnIngredientsLoopAsync(ingredientCancelToken).Forget();

        var orderCancelToken = new CancelToken();
        SpawnCustomerOrdersLoopAsync(orderCancelToken).Forget();

        _secondsLeft = _settingsSO.GameDuration;
        while (_secondsLeft > 0)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            _secondsLeft--;
        }

        // Stop game
        ingredientCancelToken.IsCancelled = true;
        orderCancelToken.IsCancelled = true;

        // Game end screen
        Assert.IsNotNull(_gameEndScreen);
        _gameEndScreen.SetActive(true);
    }

    private async UniTask SpawnIngredientsLoopAsync(CancelToken cancelToken)
    {
        while (!cancelToken.IsCancelled)
        {
            IngredientSpawner.Instance.RandomlySpawnIngredient();
            await UniTask.Delay(TimeSpan.FromSeconds(_settingsSO.IngredientSpawnInterval));
        }
    }

    private async UniTask SpawnCustomerOrdersLoopAsync(CancelToken cancelToken)
    {
        while (!cancelToken.IsCancelled)
        {
            CustomerOrderListController.Instance.TryRandomlyAddOrder();
            await UniTask.Delay(TimeSpan.FromSeconds(_settingsSO.CustomerOrderSpawnInterval));
        }
    }

    private void ResetPlayerState()
    {
        var players = FindObjectsByType<PlayerController>(FindObjectsSortMode.None);
        foreach (var player in players)
        {
            var pickupController = player.GetComponent<PickupController>();
            pickupController.TryDropCurrentItem(out _);
        }
    }

    private void DestroyAllIngredients()
    {
        var ingredients = FindObjectsByType<IngredientController>(FindObjectsSortMode.None);
        foreach (var ingredient in ingredients)
            ingredient.Consume();
    }

    private void DestroyAllSoup()
    {
        var soups = FindObjectsByType<SoupController>(FindObjectsSortMode.None);
        foreach (var soup in soups)
            soup.Consume();
    }

    private void ResetCustomerOrderList()
    {
        CustomerOrderListController.Instance.Reset();
    }

    private void ResetAllPots()
    {
        var pots = FindObjectsByType<PotController>(FindObjectsSortMode.None);
        foreach (var pot in pots)
            pot.ResetPot();
    }

    // HELPERS
    private class CancelToken
    {
        public bool IsCancelled = false;
    }
}