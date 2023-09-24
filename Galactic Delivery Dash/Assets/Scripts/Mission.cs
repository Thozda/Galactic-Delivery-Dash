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
    private GameObject chosenPlanet;
    public GameObject chosenPlanetRigged;
    private GameObject origionalPlanet;
    private GameObject finalPlanet;
    private Vector3 planetPos;
    public GameObject arrowRotate;
    public GameObject collectButton;
    public GameObject deliverButton;

    // Start is called before the first frame update
    void Start()
    {
        int n = Random.Range(0, planets.Length);
        chosenPlanet = planets[n];
        origionalPlanet = chosenPlanet;
        //origionalPlanet = chosenPlanetRigged;
        planetPos = chosenPlanet.transform.position;
        //planetPos = chosenPlanetRigged.transform.position;
        print(chosenPlanet.name);
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
        if (other.gameObject == origionalPlanet)
        {
            collectButton.SetActive(true);
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

    public void collected()
    {
        int n = Random.Range(0, planets.Length);
        chosenPlanet = planets[n];
        finalPlanet = chosenPlanet;
        planetPos = chosenPlanet.transform.position;
        //print(chosenPlanet.name);
    }

    public void Delivered()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
