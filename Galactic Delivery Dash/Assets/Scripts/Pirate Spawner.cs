using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject piratePrefab;

    private float interval = 7f;
    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnPirate(interval, piratePrefab));
    }

    private IEnumerator spawnPirate(float interval, GameObject pirate)
    {
        interval = Random.Range(15, 25);
        yield return new WaitForSeconds(interval);
        GameObject newPirate = Instantiate(pirate, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
        newPirate.GetComponent<Pirate>().player = player;
        StartCoroutine(spawnPirate(interval, pirate));
    }
}
