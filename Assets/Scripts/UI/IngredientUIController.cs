using UnityEngine;

public class IngredientUIController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _canvas;

    private IngredientController _ingredient = null;

    public void Init(IngredientController ingredient)
    {
        _ingredient = ingredient;
        foreach (var flavour in ingredient.Flavours)
        {
            // TODO
        }
    }

    private void Update()
    {
        if (_ingredient != null)
        {
            _canvas.SetActive(_ingredient.IsHovered);
            transform.position = _ingredient.transform.position;
        }
    }
}