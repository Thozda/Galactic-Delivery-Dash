using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public CanvasGroup StorePanel;

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Store()
    {
        StorePanel.alpha = 1.0f;
        StorePanel.blocksRaycasts = true;
    }

    public void Back()
    {
        StorePanel.alpha = 0f;
        StorePanel.blocksRaycasts = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
