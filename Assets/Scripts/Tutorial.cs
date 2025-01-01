using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public static bool TutorialSeen;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("TutorialSeen", 0) == 0)
        {
            TutorialSeen = false;
        }
        else
        {
            TutorialSeen = true;
        }

        if (TutorialSeen)
        {
            this.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TutorialButton()
    {
        this.gameObject.SetActive(false);
        Time.timeScale = 1.0f;
    }
}
