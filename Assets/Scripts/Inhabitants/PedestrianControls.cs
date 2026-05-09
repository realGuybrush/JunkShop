using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PedestrianControls : RandomWalker
{
    [SerializeField]
    private ScareTrigger scareTrigger;

    [SerializeField]
    private float runawaySpeedMin, runawaySpeedMax,
        defaultDropChance, defaultDropIsLegendaryChance;

    [SerializeField]
    private List<GameObject> junkPrefabs = new List<GameObject>(), valuablesPrefabs = new List<GameObject>();
    
    private float bonusRunawaySpeed, dropChance, dropIsLegendaryChance, dropChancesBonus;

    private void Awake()
    {
        bonusRunawaySpeed = Random.Range(runawaySpeedMin, runawaySpeedMax);
        dropChance = defaultDropChance;
        dropIsLegendaryChance = defaultDropIsLegendaryChance;
        dropChancesBonus = 2f;
        scareTrigger.OnScared += DoubleDropChance;
        scareTrigger.OnEscaped += HalveDropChance;
    }

    private void OnDestroy()
    {
        scareTrigger.OnScared -= DoubleDropChance;
        scareTrigger.OnEscaped -= HalveDropChance;
    }

    protected override void Updating()
    {
        TryToDrop();
        base.Updating();
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

    protected override void Stop()
    {
        base.Stop();
        dropChance = defaultDropChance;
        dropIsLegendaryChance = defaultDropIsLegendaryChance;
    }

    private void RunAway(Vector3 playerPosition)
    {
        speed = defaultSpeed * bonusRunawaySpeed;
        dropChance = defaultDropChance * dropChancesBonus;
        dropIsLegendaryChance = defaultDropIsLegendaryChance * dropChancesBonus;
        walkingTime = 1f;
        body.linearVelocity = (transform.position - playerPosition).normalized * speed;
    }

    private void DoubleDropChance()
    {
        dropChancesBonus *= 2;
    }

    private void HalveDropChance()
    {
        dropChancesBonus /= 2;
    }
}
