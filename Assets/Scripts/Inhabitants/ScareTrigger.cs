using System;
using UnityEngine;

public class ScareTrigger : MonoBehaviour
{
    [SerializeField]
    private AudioSource wah;
    
    [SerializeField]
    private GameObject scaredText;

    private bool tooClose = false;

    private float chaseTimer;
    
    public event Action OnScared = delegate { };
    public event Action OnEscaped = delegate { };

    private void Update()
    {
        if (tooClose)
        {
            if (chaseTimer > 0)
                chaseTimer -= Time.deltaTime;
            else
            {
                chaseTimer = 1f;
                WorldManager.Instance.LowerKarmaForChasing();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.layer.Equals(LayerMask.NameToLayer("Player"))) return;
        wah.Play();
        scaredText.SetActive(true);
        tooClose = true;
        chaseTimer = 1f;
        WorldManager.Instance.LowerKarmaForChasing();
        OnScared?.Invoke();
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.gameObject.layer.Equals(LayerMask.NameToLayer("Player"))) return;
        scaredText.SetActive(false);
        tooClose = false;
        chaseTimer = 0f;
        OnEscaped?.Invoke();
    }
}
