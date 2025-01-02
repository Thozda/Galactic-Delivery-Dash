using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerksManager : MonoBehaviour
{
    public static bool hasShorterMissileCooldown;
    public static bool hasBetterArmour;
    public static bool hasBetterLasers;
    public static bool hasUltraArmour;

    public Button shorterMissileCooldownButton;
    public Button betterArmourButton;
    public Button betterLasersButton;
    public Button ultraArmourButton;

    public GameObject ShorterMissileCooldownLocked;
    public GameObject BetterArmourLocked;
    public GameObject BetterLasersLocked;
    public GameObject UltraArmourLocked;

    public GameObject ShorterMissileCooldownUnlocked;
    public GameObject BetterArmourUnlocked;
    public GameObject BetterLasersUnlocked;
    public GameObject UltraArmourUnlocked;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("hasShorterMissileCooldown", 0) == 0)
        {
            hasShorterMissileCooldown = false;
        }
        else
        {
            hasShorterMissileCooldown = true;
        }

        if (PlayerPrefs.GetInt("hasBetterArmour", 0) == 0)
        {
            hasBetterArmour = false;
        }
        else
        {
            hasBetterArmour = true;
        }

        if (PlayerPrefs.GetInt("hasBetterLasers", 0) == 0)
        {
            hasBetterLasers = false;
        }
        else
        {
            hasBetterLasers = true;
        }

        if (PlayerPrefs.GetInt("hasUltraArmour", 0) == 0)
        {
            hasUltraArmour = false;
        }
        else
        {
            hasUltraArmour = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PerksManager.hasShorterMissileCooldown == true)
        {
            shorterMissileCooldownButton.interactable = false;
            ShorterMissileCooldownLocked.SetActive(false);
            ShorterMissileCooldownUnlocked.SetActive(true);
        }
        if (PerksManager.hasBetterArmour == true)
        {
            betterArmourButton.interactable = false;
            BetterArmourLocked.SetActive(false);
            BetterArmourUnlocked.SetActive(true);
        }
        if (PerksManager.hasBetterLasers == true)
        {
            betterLasersButton.interactable = false;
            BetterLasersLocked.SetActive(false);
            BetterLasersUnlocked.SetActive(true);
        }
        if (PerksManager.hasUltraArmour == true)
        {
            ultraArmourButton.interactable = false;
            UltraArmourLocked.SetActive(false);
            UltraArmourUnlocked.SetActive(true);
        }
    }
    public void ShorterMissileCooldown()
    {
        if (CurrencyManager.credits >= 2500)
        {
            PerksManager.hasShorterMissileCooldown = true;
            CurrencyManager.credits -= 2500;

            PlayerPrefs.SetInt("hasShorterMissileCooldown", 1);
            PlayerPrefs.SetInt("credits", CurrencyManager.credits);
            PlayerPrefs.Save();
        }
    }
    public void BetterArmour()
    {
        if (CurrencyManager.credits >= 5000)
        {
            PerksManager.hasBetterArmour = true;
            CurrencyManager.credits -= 5000;

            PlayerPrefs.SetInt("hasBetterArmour", 1);
            PlayerPrefs.SetInt("credits", CurrencyManager.credits);
            PlayerPrefs.Save();
        }
    }
    public void BetterLasers()
    {
        if (CurrencyManager.credits >= 7500)
        {
            PerksManager.hasBetterLasers = true;
            CurrencyManager.credits -= 7500;

            PlayerPrefs.SetInt("hasBetterLasers", 1);
            PlayerPrefs.SetInt("credits", CurrencyManager.credits);
            PlayerPrefs.Save();
        }
    }
    public void UltraArmour()
    {
        if (CurrencyManager.credits >= 10000)
        {
            PerksManager.hasUltraArmour = true;
            CurrencyManager.credits -= 10000;

            PlayerPrefs.SetInt("hasUltraArmour", 1);
            PlayerPrefs.SetInt("credits", CurrencyManager.credits);
            PlayerPrefs.Save();
        }
    }
}
