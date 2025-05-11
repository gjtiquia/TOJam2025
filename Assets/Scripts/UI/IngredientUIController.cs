using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class IngredientUIController : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private List<FlavourUI> _flavourUIPrefabs;

    [Header("References")]
    [SerializeField] private GameObject _canvas;
    [SerializeField] private Transform _flavourUIParent;

    private IngredientController _ingredient = null;

    public void Init(IngredientController ingredient)
    {
        _ingredient = ingredient;
        foreach (var flavour in ingredient.Flavours)
        {
            var uiPrefab = _flavourUIPrefabs.Find(x => x.Flavour == flavour);

            Assert.IsNotNull(uiPrefab);
            Assert.IsNotNull(_flavourUIParent);

            Instantiate(uiPrefab.UI, _flavourUIParent);
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