using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CustomerOrderController : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private List<FlavourUI> _flavourUIPrefabs;

    [Header("References")]
    [SerializeField] private Transform _flavourUIParent;

    private List<EFlavour> _flavours = new();

    public void SetFlavours(List<EFlavour> flavours)
    {
        Debug.Log($"CustomerOrderController.SetFlavours: flavour count: {flavours.Count}");

        // Clone to prevent issues with references
        _flavours.Clear();
        foreach (var flavour in flavours)
        {
            _flavours.Add(flavour);

            var uiPrefab = _flavourUIPrefabs.Find(x => x.Flavour == flavour);

            Assert.IsNotNull(uiPrefab);
            Assert.IsNotNull(_flavourUIParent);

            Instantiate(uiPrefab.UI, _flavourUIParent);
        }
    }
}

// HELPERS
[System.Serializable]
public class FlavourUI
{
    public EFlavour Flavour;
    public GameObject UI;
}