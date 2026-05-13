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
    
    private float defaultRadius, chaseSpeed;
    
    public event Action OnAttack = delegate { };

    protected override void Awaking()
    {
        base.Awaking();
        defaultRadius = chaseTrigger.radius;
        chaseSpeed = Random.Range(chaseSpeedMin, chaseSpeedMax) * speed;
        attackTrigger.OnReached += Attack;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer.Equals(playerLayer))
        {
            attackTrigger.gameObject.SetActive(true);
            animator.SetBool("Move", true);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer.Equals(playerLayer))
            Follow(other.transform.position);
    }

    private void Attack()
    {
        attackTrigger.gameObject.SetActive(false);
        chaseTrigger.enabled = false;
        animator.SetBool("Move", false);
        animator.SetBool("Attack", true);
        OnAttack?.Invoke();
        StartCoroutine("Wait");
    }

    private IEnumerator Wait()
    {
        body.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(2f);
        animator.SetBool("Attack", false);
    }

    private void Follow(Vector3 playerPosition)
    {
        body.linearVelocity = (playerPosition - transform.position).normalized * chaseSpeed;
        animator.SetBool("Move", true);
        Flip();
    }

    public void ChangeRadius(float multiplier)
    {
        chaseTrigger.radius = Mathf.Min(defaultRadius * multiplier, 50f);
        chaseTrigger.enabled = multiplier > 0;
    }
}
