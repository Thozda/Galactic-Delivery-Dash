using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public CanvasGroup UpgradesPanel;
    public CanvasGroup SkinsPanel;
    public CanvasGroup CreditsPanel;
    public LevelPlayAds AdManager;
    private bool _MissionStarted;
    public TMPro.TextMeshProUGUI AdButtonText;

    private void Start()
    {
        _MissionStarted = true;
        UpdateAdButtonText();
    }

    public void PlayGame()
    {
        _MissionStarted = true;
        UpdateAdButtonText();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Store()
    {
        UpgradesPanel.alpha = 1.0f;
        UpgradesPanel.blocksRaycasts = true;
    }

    public void Back()
    {
        UpgradesPanel.alpha = 0f;
        UpgradesPanel.blocksRaycasts = false;
    }

    public void Skins()
    {
        SkinsPanel.alpha = 1.0f;
        SkinsPanel.blocksRaycasts = true;
    }

    public void SkinsBack()
    {
        SkinsPanel.alpha = 0f;
        SkinsPanel.blocksRaycasts = false;
    }

    public void Credits()
    {
        CreditsPanel.alpha = 1.0f;
        CreditsPanel.blocksRaycasts = true;
    }

    public void FreeCreditsAd()
    {
        if (_MissionStarted)
        {
            AdManager.ShowRewardedAd("100Credits");

            _MissionStarted = false;
            UpdateAdButtonText();
        }
    }

    public void CreditsBack()
    {
        CreditsPanel.alpha = 0f;
        CreditsPanel.blocksRaycasts = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void DeletePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

    private void UpdateAdButtonText()
    {
        if (_MissionStarted)
        {
            AdButtonText.text = ("Watch Ad");
        }
        else
        {
            AdButtonText.text = ("Try Again Soon");
        }
    }
}
