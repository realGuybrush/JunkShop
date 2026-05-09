using TMPro;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public static WorldManager Instance;

    [SerializeField]
    private TextMeshProUGUI karmaText, earningsText, junkText, valuableText;

    private float earnings, junkHoard, valuableHoard;
    private int karma;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void AddToSack(bool valuable, float cost)
    {
        if (valuable)
            valuableHoard += cost;
        else
            junkHoard += cost;
        UpdateTexts();
    }

    private void UpdateTexts()
    {
        earningsText?.SetText(earnings.ToString());
        junkText?.SetText(junkHoard.ToString());
        valuableText?.SetText(valuableHoard.ToString());
        karmaText?.SetText(karma.ToString());
    }

    public void Sell(bool valuable)
    {
        if (valuable)
        {
            earnings += valuableHoard;
            if(valuableHoard > 0)
                karma -= Mathf.Max((int)valuableHoard / 10, 1);
            valuableHoard = 0;
        } else
        {
            earnings += junkHoard;
            junkHoard = 0;
        }
        UpdateTexts();
    }

    public void Return()
    {
        if (valuableHoard <= 0) return;
        karma += Mathf.Max((int)valuableHoard / 33, 1);
        valuableHoard = 0;
        UpdateTexts();
    }
}
