using UnityEngine;

public class Item : BaseInteractable
{
    [SerializeField]
    private bool valuable;
    
    [SerializeField]
    private float cost;

    protected override void Interact()
    {
        WorldManager.Instance.AddToSack(valuable, cost);
        Destroy(gameObject);
    }
}
