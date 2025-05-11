using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class GameStartScreenController : MonoBehaviour
{
    [SerializeField] private Button _startButton;

    private void Start()
    {
        _startButton.onClick.AddListener(OnClicked);
    }

    private void OnDestroy()
    {
        _startButton.onClick.AddListener(OnClicked);
    }

    private void OnClicked()
    {
        GameManager.Instance.StartGameAsync().Forget();
        this.gameObject.SetActive(false);
    }
}