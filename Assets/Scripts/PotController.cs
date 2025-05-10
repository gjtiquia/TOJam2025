using UnityEngine;
using UnityEngine.Assertions;

public class PotController : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject _hoverVisual;

    private void Awake()
    {
        Assert.IsNotNull(_hoverVisual);
        SetIsHoveredState(false);
    }

    public void SetIsHoveredState(bool isHovered)
    {
        _hoverVisual.SetActive(isHovered);
    }

    public bool IsInteractable(IInteractContext context)
    {
        // always interactable for now i guess...?
        // probably need to check context, if has ingredient picked up or if full (stretch goal)
        return true;
    }

    public void Interact(IInteractContext context)
    {
        // TODO
    }
}