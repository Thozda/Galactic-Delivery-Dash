using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using CodeMonkey.Utils;
using UnityEngine.AdaptivePerformance.VisualScripting;
using UnityEngine.SceneManagement;

public class Mission : MonoBehaviour
{
    public GameObject[] planets;
    public GameObject chosenPlanet;
    public GameObject chosenPlanetRigged;
    public GameObject origionalPlanet;
    public GameObject finalPlanet;
    private Vector3 planetPos;
    public GameObject arrowRotate;
    public GameObject collectButton;
    public GameObject deliverButton;
    public CurrencyManager currencyManagerRef;
    public AudioSource collectionAudio;
    private bool isCollected;
    private bool _isPlayingTutorial;
    public GameObject TutorialCollect;
    public GameObject TutorialDeliver;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
        _isPlayingTutorial = !Tutorial.TutorialSeen;
        PlanetChooser(TutorialCollect, true);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 toPosition = planetPos;
        Vector3 fromPosition = transform.position;
        fromPosition.z = 0f;
        Vector3 dir = (toPosition - fromPosition).normalized;
        float angle = UtilsClass.GetAngleFromVectorFloat(dir);
        arrowRotate.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == finalPlanet)
        {
            deliverButton.SetActive(true);
        }
        if (!isCollected)
        {
            if (other.gameObject == origionalPlanet)
            {
                collectButton.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == finalPlanet)
        {
            deliverButton.SetActive(false);
        }
        if (other.gameObject == origionalPlanet)
        {
            collectButton.SetActive(false);
        }
    }

    private void PlanetChooser(GameObject _planet, bool _firstPlanet)
    {
        if (_firstPlanet)
        {
            if (_isPlayingTutorial)
            {
                origionalPlanet = _planet;
                planetPos = _planet.transform.position;
            }
            else
            {
                int n = Random.Range(0, planets.Length);
                chosenPlanet = planets[n];
                origionalPlanet = chosenPlanet;
                planetPos = chosenPlanet.transform.position;
            }
        }
        else
        {
            if (_isPlayingTutorial)
            {
                finalPlanet = _planet;
                planetPos = _planet.transform.position;
            }
            else
            {
                int n = Random.Range(0, planets.Length);
                chosenPlanet = planets[n];
                finalPlanet = chosenPlanet;
                planetPos = chosenPlanet.transform.position;
            }
        }
        
    }

    public void collected()
    {
        PlanetChooser(TutorialDeliver, false);
        collectionAudio.Play();
        isCollected = true;
        collectButton.SetActive(false);
        Tutorial.TutorialSeen = true;
        PlayerPrefs.SetInt("TutorialSeen", 1);
        PlayerPrefs.Save();
    }

    public void Delivered()
    {
        switch (currencyManagerRef.currentState)
        {
            case riskState.lowValue:
                currencyManagerRef.currentState = riskState.lowValue;
                CurrencyManager.credits += 100;
                PlayerPrefs.SetInt("credits", CurrencyManager.credits);
                PlayerPrefs.Save();
                break;
            case riskState.midValue:
                currencyManagerRef.currentState = riskState.midValue;
                CurrencyManager.credits += 200;
                PlayerPrefs.SetInt("credits", CurrencyManager.credits);
                PlayerPrefs.Save();
                break;
            case riskState.highValue:
                currencyManagerRef.currentState = riskState.highValue;
                CurrencyManager.credits += 500;
                PlayerPrefs.SetInt("credits", CurrencyManager.credits);
                PlayerPrefs.Save();
                break;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
