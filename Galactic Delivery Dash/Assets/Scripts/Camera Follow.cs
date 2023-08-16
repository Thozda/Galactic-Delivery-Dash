using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    private float targetX;
    private float targetY;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        targetX = player.transform.position.x;
        targetY = player.transform.position.y;
        transform.position = new Vector3 (targetX, targetY, -10);
    }
}
