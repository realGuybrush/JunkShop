using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class WorldManager : MonoBehaviour
{
    public static WorldManager Instance;

    [SerializeField]
    private AudioSource pickUpJunk, pickUpValuable;
    
    [SerializeField]
    private List<Transform> doors;

    [SerializeField]
    private List<Item> junkPrefabs, valuablesPrefabs;
    
    [SerializeField]
    private GameObject walkerPrefab;

    [SerializeField]
    private int maxSlackersAmount, maxPolicemenAmount, maxItemsAmount;

    [SerializeField]
    private float halfMapWidth, halfMapHeight, chanceToSpawnWalker, chanceToSpawnItem, valItemPercentage;

    [SerializeField]
    private GameObject pedestrianPrefab;

    [SerializeField]
    private PolicemanSpawningHusk policemanPrefab;

    [SerializeField]
    private List<PolicemanControls> policemen;

    [SerializeField]
    private TextMeshProUGUI karmaText, earningsText, junkText, valuableText;

    private float earnings, junkHoard, valuableHoard;
    private int karma;

    private List<Item> items = new List<Item>();

    private void Awake()
    {
        Time.timeScale = 1;
        if (Instance == null)
            Instance = this;
        InitSlackingPeople();
    }

    private void Update()
    {
        if(Time.timeScale > 0)
        {
            TrySpawnWalker();
            TrySpawnItem();
        }
    }

    private void OnDestroy()
    {
        foreach (var policeman in policemen)
            policeman.OnAttack -= HalfKarmaAndEarnings;
    }

    private void InitSlackingPeople()
    {
        int slackerAmount = Random.Range(1, maxSlackersAmount + 1),
            policemenAmount = Random.Range(1, maxPolicemenAmount + 1);
        //do I need to remember pedestrians? do I need to call them from here?
        //todo: refactor this class and its connections to other classes
        for (int i = 0; i < slackerAmount; i++)
            Instantiate(pedestrianPrefab, RandomCoordinates(), new Quaternion());
        for (int i = 0; i < policemenAmount; i++)
        {
            policemen.Add(Instantiate(policemanPrefab, 
                new Vector3(Random.Range(-halfMapWidth, halfMapWidth),
                    Random.Range(-halfMapHeight, halfMapHeight)),
                new Quaternion()).PolicemanControls);
            policemen[^1].OnAttack += HalfKarmaAndEarnings;
        }
    }

    private Vector3 RandomCoordinates()
    {
        Vector3 output = new Vector3(Random.Range(-halfMapWidth, halfMapWidth),
            Random.Range(-halfMapHeight, halfMapHeight));
        while (output.magnitude > halfMapWidth)
            output = new Vector3(Random.Range(-halfMapWidth, halfMapWidth),
            Random.Range(-halfMapHeight, halfMapHeight));
        return output;
    }

    private void HalfKarmaAndEarnings()
    {
        if (karma < -4)
            karma -= karma / 5;
        else
            karma = 0;
        earnings -= earnings / 5;
        if (earnings < 0.01f) earnings = 0;
        int valuablesLoss = (int) valuableHoard / 2;
        valuableHoard -= Mathf.Max(valuablesLoss, valuableHoard);
        UpdateKarma(0);
        UpdateTexts();
    }

    private void TrySpawnWalker()
    {
        if (Random.Range(0f, 1f) < chanceToSpawnWalker)
            Instantiate(walkerPrefab, RandomDoor(Vector3.zero) * 0.98f, new Quaternion());
    }

    public Vector3 RandomDoor(Vector3 exclude)
    {
        int indx1 = Random.Range(0, doors.Count);
        while ((doors[indx1].position - exclude).magnitude < 0.01f)
            indx1 = Random.Range(0, doors.Count);
        return doors[indx1].position;
    }
    

    private void TrySpawnItem()
    {
        if (Random.Range(0f, 1f) < chanceToSpawnItem)
            if (Random.Range(0f, 1f) < valItemPercentage)
                SpawnValuable(Random.Range(0, valuablesPrefabs.Count), RandomCoordinates());
            else
                SpawnJunk(Random.Range(0, junkPrefabs.Count), RandomCoordinates());
    }

    public void SpawnJunk(int index, Vector3 position)
    {
        //if(items.Count < maxItemsAmount)
            items.Add(Instantiate(junkPrefabs[index],position, new Quaternion()));
    }
    
    public void SpawnValuable(int index, Vector3 position)
    {
        //if(items.Count < maxItemsAmount)
            items.Add(Instantiate(valuablesPrefabs[index], position, new Quaternion()));
    }

    public void AddToSack(bool valuable, float cost)
    {
        if (valuable)
        {
            valuableHoard += cost;
            pickUpValuable.Play();
        } else
        {
            junkHoard += cost;
            pickUpJunk.Play();
        }
        UpdateTexts();
    }

    private void UpdateTexts()
    {
        earningsText?.SetText(earnings.ToString());
        junkText?.SetText(junkHoard.ToString());
        valuableText?.SetText(valuableHoard.ToString());
    }

    public void Sell(bool valuable)
    {
        if (valuable)
        {
            earnings += valuableHoard;
            if(valuableHoard > 0)
                UpdateKarma(-(int)valuableHoard / 10);
            valuableHoard = 0;
        } else
        {
            earnings += junkHoard;
            junkHoard = 0;
        }
        UpdateTexts();
    }

    private void UpdateKarma(int delta)
    {
        karma += delta;
        foreach (var policeman in policemen)
            policeman.ChangeRadius(-karma);
        karmaText?.SetText(karma.ToString());
    }

    public void Return()
    {
        if (valuableHoard <= 0) return;
        UpdateKarma((int)valuableHoard / 33);
        valuableHoard = 0;
        UpdateTexts();
    }

    public void LowerKarmaForChasing()
    {
        UpdateKarma(-5);
    }

    public int GetRandomJunk()
    {
        return Random.Range(0, junkPrefabs.Count);
    }
    
    public int GetRandomValuable()
    {
        return Random.Range(0, valuablesPrefabs.Count);
    }

    public int Karma => karma;
    public float Earnings => earnings;
}
