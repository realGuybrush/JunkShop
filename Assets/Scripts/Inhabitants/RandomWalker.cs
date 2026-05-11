using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomWalker : MonoBehaviour
{
    [SerializeField]
    protected Rigidbody2D body;

    [SerializeField]
    protected GameObject images;

    [SerializeField]
    protected Animator animator;

    [SerializeField]
    protected float defaultSpeed;
    
    [SerializeField]
    private float maxWalkingTime, chanceToStartWalking;
    
    protected float walkingTime, speed;

    private void Awake()
    {
        Awaking();
    }

    void Update()
    {
        Updating();
    }

    protected virtual void Awaking()
    {
        speed = defaultSpeed;
    }

    protected virtual void Updating()
    {
        DefaultWalkingPattern();
        UpdateTimer();
    }

    protected virtual void DefaultWalkingPattern()
    {
        if (Random.Range(0f, 1f) < chanceToStartWalking && !(walkingTime > 0))
        {
            walkingTime = Random.Range(0f, maxWalkingTime);
            body.linearVelocity = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * speed;
            animator.SetBool("Move", true);
            Flip();
        }
    }
    
    protected void Flip()
    {
        //todo: refactor this class and all children,
        //make movement through following point
        //and put this method there;
        //also somehow prevent it from happening every time
        if (body.linearVelocity.x > 0)
            images.transform.eulerAngles = Vector3.zero;
        if (body.linearVelocity.x < 0)
            images.transform.eulerAngles = new Vector3(0, 180, 0);
    }

    private void UpdateTimer()
    {
        if (walkingTime > 0)
        {
            walkingTime -= Time.deltaTime;
            if (walkingTime <= 0)
                Stop();
        }
    }

    protected virtual void Stop()
    {
        body.linearVelocity = Vector2.zero;
        speed = defaultSpeed;
        animator.SetBool("Move", false);
    }
}
