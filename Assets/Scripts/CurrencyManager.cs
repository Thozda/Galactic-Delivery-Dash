using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrencyManager : MonoBehaviour
{
    public static int credits;
    [SerializeField] TMPro.TextMeshProUGUI[] text;
    public riskState currentState;

    public GameObject valueSelector;

    // Start is called before the first frame update
    void Start()
    {
        credits = PlayerPrefs.GetInt("credits", 1000);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var i in text)
        {
            i.text = credits.ToString();
        }

        switch (currentState)
        {
            case riskState.lowValue:
                currentState = riskState.lowValue;
                break;
            case riskState.midValue:
                currentState = riskState.midValue;
                break;
            case riskState.highValue:
                currentState = riskState.highValue;
                break;
        }
    }
    public void LowValuePackage()
    {
        TutorialCheck();
        CurrencyManager.credits -= 50;
        PlayerPrefs.SetInt("credits", credits);
        PlayerPrefs.Save();
        currentState = riskState.lowValue;
    }
    public void MidValuePackage()
    {
        TutorialCheck();
        CurrencyManager.credits -= 100;
        PlayerPrefs.SetInt("credits", credits);
        PlayerPrefs.Save();
        currentState = riskState.midValue;
    }
    public void HighValuePackage()
    {
        TutorialCheck();
        CurrencyManager.credits -= 250;
        PlayerPrefs.SetInt("credits", credits);
        PlayerPrefs.Save();
        currentState = riskState.highValue;
    }
    public void TutorialCheck()
    {
        valueSelector.SetActive(false);

        if (Tutorial.TutorialSeen)
        {
            Time.timeScale = 1;
        }
    }
}
public enum riskState { lowValue, midValue, highValue }
