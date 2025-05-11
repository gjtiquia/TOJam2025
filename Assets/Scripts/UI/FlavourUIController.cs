using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public interface IFlavourUIParent
{
    public GameObject gameObject { get; }
    public Transform transform { get; }
    public List<EFlavour> Flavours { get; }
    public bool CanShowFlavourUI();
}

public class FlavourUIController : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private List<FlavourUI> _flavourUIPrefabs;

    [Header("References")]
    [SerializeField] private GameObject _canvas;
    [SerializeField] private Transform _flavourUIParent;

    private IFlavourUIParent _parent = null;
    private List<GameObject> _flavourInstances = new();

    public void Init(IFlavourUIParent parent)
    {
        _parent = parent;
        transform.position = parent.transform.position;
        UpdateFlavours();
    }

    public void UpdateFlavours()
    {
        // delete existing flavours (if any)
        foreach (var instance in _flavourInstances)
            Destroy(instance.gameObject);

        _flavourInstances.Clear();
        foreach (var flavour in _parent.Flavours)
        {
            var uiPrefab = _flavourUIPrefabs.Find(x => x.Flavour == flavour);

            Assert.IsNotNull(uiPrefab);
            Assert.IsNotNull(_flavourUIParent);

            var instance = Instantiate(uiPrefab.UI, _flavourUIParent);
            _flavourInstances.Add(instance);
        }
    }

    private void Update()
    {
        // the (Component) cast hack because if even if it was destroyed, _parent == null will return false, force casting makes it return true
        if ((Component)_parent == null || _parent != null && _parent.gameObject == null)
            _parent = null;

        if (_parent != null && _parent.gameObject != null)
        {
            _canvas.SetActive(_parent.CanShowFlavourUI());
            transform.position = _parent.transform.position;
        }
    }
}