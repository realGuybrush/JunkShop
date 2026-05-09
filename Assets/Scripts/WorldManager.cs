using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public static WorldManager Instance;

    [SerializeField]
    private List<PolicemanControls> policemen;

    [SerializeField]
    private TextMeshProUGUI karmaText, earningsText, junkText, valuableText;

    private float earnings, junkHoard, valuableHoard;
    private int karma;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        foreach (var policeman in policemen)
            policeman.OnAttack += HalfKarmaAndEarnings;
    }

    private void OnDestroy()
    {
        foreach (var policeman in policemen)
            policeman.OnAttack -= HalfKarmaAndEarnings;
    }

    private void HalfKarmaAndEarnings()
    {
        if (karma < -1)
            karma /= 2;
        else
            karma = 0;
        int earningLoss = (int) earnings / 2;
        earnings -= Mathf.Max(earningLoss, earnings);
        int valuablesLoss = (int) valuableHoard / 2;
        valuableHoard -= Mathf.Max(valuablesLoss, valuableHoard);
        UpdateKarma(0);
        UpdateTexts();
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
    }

    public void Sell(bool valuable)
    {
        if (valuable)
        {
            earnings += valuableHoard;
            if(valuableHoard > 0)
                UpdateKarma(-(int)valuableHoard);
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
        UpdateKarma((int)valuableHoard / 3);
        valuableHoard = 0;
        UpdateTexts();
    }

    public void LowerKarmaForChasing()
    {
        UpdateKarma(-5);
    }
}
