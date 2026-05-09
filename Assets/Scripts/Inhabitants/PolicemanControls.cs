using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class PolicemanControls : RandomWalker
{
    [SerializeField]
    private AttackTrigger attackTrigger;
    
    [SerializeField]
    private CircleCollider2D chaseTrigger;

    [SerializeField]
    private float chaseSpeedMin, chaseSpeedMax;
    
    private float defaultRadius, bonusChaseSpeed;
    
    public event Action OnAttack = delegate { };

    private void Awake()
    {
        defaultRadius = chaseTrigger.radius;
        bonusChaseSpeed = Random.Range(chaseSpeedMin, chaseSpeedMax);
        attackTrigger.OnReached += Attack;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
            Follow(other.transform.position);
    }

    private void Attack()
    {
        attackTrigger.gameObject.SetActive(false);
        chaseTrigger.enabled = false;
        OnAttack?.Invoke();
        StartCoroutine("Wait");
    }

    private IEnumerator Wait()
    {
        body.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(2f);
    }

    private void Follow(Vector3 playerPosition)
    {
        body.linearVelocity = (playerPosition - transform.position).normalized * speed * bonusChaseSpeed;
        attackTrigger.gameObject.SetActive(true);
    }

    public void ChangeRadius(float multiplier)
    {
        chaseTrigger.radius = defaultRadius * multiplier;
        chaseTrigger.enabled = multiplier > 0;
    }
}
