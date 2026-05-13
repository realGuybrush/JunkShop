using System.Collections;
using UnityEngine;

public class Item : BaseInteractable
{
    [SerializeField]
    private bool valuable;
    
    [SerializeField]
    private float cost;

    [SerializeField]
    private float lifeTime = 60f;

    private void Start()
    {
        //StartCoroutine("SelfDestruct");
    }

    private IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    protected override void Interact()
    {
        WorldManager.Instance.AddToSack(valuable, cost);
        Destroy(gameObject);
    }
}
