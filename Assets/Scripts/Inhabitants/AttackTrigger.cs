using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AttackTrigger : MonoBehaviour
{
    [SerializeField]
    private float kickStrengthMin = 0.03f, kickStrengthMax = 0.05f;

    private float kickStrength;
    
    public event Action OnReached = delegate { };

    private void Awake()
    {
        kickStrength = Random.Range(kickStrengthMin, kickStrengthMax);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.layer.Equals(LayerMask.NameToLayer("Player"))) return;
        other.GetComponent<PlayerControls>().GetDamaged(transform.parent.position, kickStrength);
        OnReached?.Invoke();
    }
}
