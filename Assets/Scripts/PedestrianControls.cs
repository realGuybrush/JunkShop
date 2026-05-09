using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PedestrianControls : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D body;

    [SerializeField]
    private float defaultSpeed, runawaySpeedMin, runawaySpeedMax, maxWalkingTime, chanceToStartWalking,
        defaultDropChance, defaultDropIsLegendaryChance;

    [SerializeField]
    private List<GameObject> junkPrefabs = new List<GameObject>(), valuablesPrefabs = new List<GameObject>();
    
    private float walkingTime, speed, bonusRunawaySpeed, dropChance, dropIsLegendaryChance;

    private Vector2 walkDirection;

    private void Awake()
    {
        bonusRunawaySpeed = Random.Range(runawaySpeedMin, runawaySpeedMax);
        dropChance = defaultDropChance;
        dropIsLegendaryChance = defaultDropIsLegendaryChance;
    }

    private void Update()
    {
        TryToDrop();
        TryToStartWalkRandom();
        UpdateTimer();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
            RunAway(other.transform.position);
    }

    private void TryToDrop()
    {
        if (Random.Range(0f, 1f) < dropChance)
            Drop(Random.Range(0f, 1f) < dropIsLegendaryChance);
    }

    private void Drop(bool legendary)
    {
        if(legendary)
        {
            if (valuablesPrefabs != null && valuablesPrefabs.Count > 0)
                Instantiate(valuablesPrefabs[Random.Range(0, valuablesPrefabs.Count)], transform.position, transform.rotation);
        } else
        {
            if (junkPrefabs != null && junkPrefabs.Count > 0)
                Instantiate(junkPrefabs[Random.Range(0, junkPrefabs.Count)], transform.position, transform.rotation);
        }
    }

    private void TryToStartWalkRandom()
    {
        if (Random.Range(0f, 1f) < chanceToStartWalking && !(walkingTime > 0))
        {
            walkingTime = Random.Range(0f, maxWalkingTime);
            walkDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            body.linearVelocity = walkDirection * speed;
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

    private void Stop()
    {
        body.linearVelocity = Vector2.zero;
        speed = defaultSpeed;
        dropChance = defaultDropChance;
        dropIsLegendaryChance = defaultDropIsLegendaryChance;
    }

    private void RunAway(Vector3 playerPosition)
    {
        speed = defaultSpeed * bonusRunawaySpeed;
        dropChance = defaultDropChance * 2;
        dropIsLegendaryChance = defaultDropIsLegendaryChance * 2;
        walkingTime = 1f;
        body.linearVelocity = (transform.position - playerPosition).normalized * speed;
    }
}
