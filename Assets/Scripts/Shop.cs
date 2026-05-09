using UnityEngine;

public class Shop : BaseInteractable
{
    [SerializeField]
    private bool valuable;

    protected override void Interact()
    {
        WorldManager.Instance.Sell(valuable);
    }
}

