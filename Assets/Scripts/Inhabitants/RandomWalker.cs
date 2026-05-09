using UnityEngine;
using Random = UnityEngine.Random;

public class RandomWalker : MonoBehaviour
{
    [SerializeField]
    protected Rigidbody2D body;

    [SerializeField]
    protected float defaultSpeed;
    
    [SerializeField]
    private float maxWalkingTime, chanceToStartWalking;
    
    protected float walkingTime, speed;
    void Update()
    {
        Updating();
    }

    protected virtual void Updating()
    {
        TryToStartWalkRandom();
        UpdateTimer();
    }

    private void TryToStartWalkRandom()
    {
        if (Random.Range(0f, 1f) < chanceToStartWalking && !(walkingTime > 0))
        {
            walkingTime = Random.Range(0f, maxWalkingTime);
            body.linearVelocity = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * speed;
        }
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
    }
}
