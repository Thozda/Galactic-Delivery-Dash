using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using CodeMonkey.Utils;
using UnityEngine.AdaptivePerformance.VisualScripting;

public class Mission : MonoBehaviour
{
    public GameObject[] planets;
    private GameObject chosenPlanet;
    public GameObject tempPlanet;
    private Vector3 planetPos;
    public GameObject arrowRotate;
    public GameObject Button;

    // Start is called before the first frame update
    void Start()
    {
        int n = Random.Range(0, planets.Length);
        chosenPlanet = planets[n];
        //chosenPlanet = tempPlanet;
        planetPos = chosenPlanet.transform.position;
        //planetPos = tempPlanet.transform.position;
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
        if (other.gameObject == chosenPlanet)
        {
            Button.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == chosenPlanet)
        {
            Button.SetActive(false);
        }
    }
}
