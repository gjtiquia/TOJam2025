using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class TimeLeftUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    public void Start()
    {
        UpdateText();
        GameManager.OnSecondsLeftChanged += UpdateText;
    }

    public void OnDestroy()
    {
        GameManager.OnSecondsLeftChanged -= UpdateText;
    }

    private void UpdateText()
    {
        Assert.IsNotNull(_text);

        var totalSecondsLeft = GameManager.Instance.SecondsLeft;

        var minutes = Mathf.FloorToInt((float)totalSecondsLeft / 60f);
        var seconds = totalSecondsLeft % 60;

        _text.text = $"Time Left: {(minutes < 10 ? "0" : "")}{minutes}:{(seconds < 10 ? "0" : "")}{seconds}";
    }
}