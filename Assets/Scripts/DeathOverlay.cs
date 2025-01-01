using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathOverlay : MonoBehaviour
{
    public LevelPlayAds AdManager;

    public void WatchAd()
    {
        AdManager.ShowRewardedAd("Revive");
    }
    public void Continue()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }
}
