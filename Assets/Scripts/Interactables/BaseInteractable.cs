using System;
using UnityEngine;

public class BaseInteractable : MonoBehaviour
{
    private LayerMask picker;

    private void Awake()
    {
        picker = LayerMask.NameToLayer("Picker");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer.Equals(picker))
        {
            Interact();
        }
    }

    protected virtual void Interact()
    {
    }
}