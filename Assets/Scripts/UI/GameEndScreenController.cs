using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class GameEndScreenController : MonoBehaviour
{
    [SerializeField] private Button _playAgainButton;

    private void Start()
    {
        _playAgainButton.onClick.AddListener(OnPlayAgainClicked);
    }

    private void OnDestroy()
    {
        _playAgainButton.onClick.AddListener(OnPlayAgainClicked);
    }

    private void OnPlayAgainClicked()
    {
        GameManager.Instance.StartGameAsync().Forget();
        this.gameObject.SetActive(false);
    }
}