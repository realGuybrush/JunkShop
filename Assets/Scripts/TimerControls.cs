using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerControls : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;
    
    [SerializeField]
    private GameObject theEndPanel;
    
    [SerializeField]
    private Image readyText, goText;

    [SerializeField]
    private float maxTime;

    [SerializeField]
    private TextMeshProUGUI timerText, karmaText, earningsText;

    private void Update()
    {
        if (readyText.gameObject.activeSelf)
        {
            readyText.color = new Color(readyText.color.r, readyText.color.g, readyText.color.b, readyText.color.a - Time.deltaTime);
            if (readyText.color.a <= 0f)
            {
                readyText.gameObject.SetActive(false);
                goText.gameObject.SetActive(true);
                StartCoroutine("TurnOffText");
                audioSource.Play();
            }
        } else if (!goText.gameObject.activeSelf)
        {
            if(maxTime > 0)
            {
                maxTime -= Time.deltaTime;
                timerText.text = (int) maxTime / 60 + ":" + (int)(maxTime % 60);
            } else
            {
                TheEnd();
            }
        }
    }

    private IEnumerator TurnOffText()
    {
        yield return new WaitForSeconds(0.5f);
        goText.gameObject.SetActive(false);
    }

    private void TheEnd()
    {
        Time.timeScale = 0;
        theEndPanel.SetActive(true);
        karmaText.text = WorldManager.Instance.Karma.ToString();
        earningsText.text = WorldManager.Instance.Earnings + "$";
    }
}
