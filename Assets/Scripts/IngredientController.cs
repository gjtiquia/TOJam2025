using UnityEngine;
using UnityEngine.Assertions;

public class IngredientController : MonoBehaviour, IInteractable
{
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Material _idleMaterial;
    [SerializeField] private Material _hoverMaterial;

    private bool _isInteractable = true;

    private void Awake()
    {
        Assert.IsNotNull(_meshRenderer);
        Assert.IsNotNull(_idleMaterial);
        Assert.IsNotNull(_hoverMaterial);

        _meshRenderer.material = _idleMaterial;
    }

    public bool IsInteractable()
    {
        return _isInteractable;
    }

    public void SetIsHoveredState(bool isHovered)
    {
        // TODO : temp handling
        _meshRenderer.material = isHovered ? _hoverMaterial : _idleMaterial;
    }

    public void Interact()
    {
        // TODO
        Debug.Log("Ingredient: Interact");

        // TODO : if player is currently holding another ingredient... swap

        // TODO : idle state -> isPickedUp state
        _isInteractable = false;
    }
}