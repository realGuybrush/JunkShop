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
    private List<int> junkPrefabs = new List<int>(), valuablesPrefabs = new List<int>();
    
    private float bonusRunawaySpeed, dropChance, dropIsLegendaryChance, dropChancesBonus, runningDropChance, scaredDropChance;

    protected override void Awaking()
    {
        base.Awaking();
        bonusRunawaySpeed = Random.Range(runawaySpeedMin, runawaySpeedMax);
        dropChance = defaultDropChance;
        dropIsLegendaryChance = defaultDropIsLegendaryChance;
        dropChancesBonus = 2f;
        runningDropChance = defaultDropChance * dropChancesBonus;
        SetDrops();
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

    private void SetDrops()
    {
        junkPrefabs = new List<int>();
        valuablesPrefabs = new List<int>();
        for(int i=0; i<3; i++)
            junkPrefabs.Add(WorldManager.Instance.GetRandomJunk());
        valuablesPrefabs.Add(WorldManager.Instance.GetRandomValuable());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer.Equals(playerLayer))
            SetParametersToRunMode();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer.Equals(playerLayer))
            RunAway(other.transform.position);
    }

    private void SetParametersToRunMode()
    {
        speed = defaultSpeed * bonusRunawaySpeed;
        dropChance = runningDropChance;
        dropIsLegendaryChance = defaultDropIsLegendaryChance * dropChancesBonus;
        animator.SetBool(moveHash, true);
    }

    private void TryToDrop()
    {
        if(Time.timeScale > 0)
            if (Random.Range(0f, 1f) < dropChance)
                Drop(Random.Range(0f, 1f) < dropIsLegendaryChance);
    }

    private void Drop(bool legendary)
    {
        if(legendary)
        {
            if (valuablesPrefabs != null && valuablesPrefabs.Count > 0)
                WorldManager.Instance.SpawnValuable(valuablesPrefabs[Random.Range(0, valuablesPrefabs.Count)], transform.position);
        } else
        {
            if (junkPrefabs != null && junkPrefabs.Count > 0)
                WorldManager.Instance.SpawnJunk(junkPrefabs[Random.Range(0, junkPrefabs.Count)], transform.position);
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
        walkingTime = 1f;
        body.linearVelocity = (transform.position - playerPosition).normalized * speed;
        Flip();
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
