using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Assertions;
using System;

public class GameManager : MonoBehaviour
{
    public static event Action OnSecondsLeftChanged;
    public static GameManager Instance;

    public int SecondsLeft => _secondsLeft;

    [SerializeField] private GameSettingsSO _settingsSO;

    private bool _isSpawningIngredients;
    private bool _isSpawningOrders;

    private int _secondsLeft { get => m_secondsLeft; set { m_secondsLeft = value; OnSecondsLeftChanged?.Invoke(); } }
    private int m_secondsLeft;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // leaving the flexibility to start game after title screen
        StartGameAsync().Forget();
    }

    public async UniTask StartGameAsync()
    {
        // TODO : attempt cleanup

        Assert.IsNotNull(_settingsSO);
        SpawnIngredientsLoopAsync().Forget();
        SpawnCustomerOrdersLoopAsync().Forget();

        _secondsLeft = _settingsSO.GameDuration;
        while (_secondsLeft > 0)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            _secondsLeft--;
        }

        // Stop game
        _isSpawningIngredients = false;
        _isSpawningOrders = false;

        // TODO : end screen
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