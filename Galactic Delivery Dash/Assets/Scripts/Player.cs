using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public MovementJoystick movementJoystick;
    public float playerSpeed;
    public float rotationSpeed;
    private Rigidbody2D rb;
    public float health = 1f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(movementJoystick.joystickVec.y != 0)
        {
            rb.velocity = new Vector2(movementJoystick.joystickVec.x * playerSpeed, movementJoystick.joystickVec.y * playerSpeed);

            transform.rotation = Quaternion.LookRotation(transform.forward, rb.velocity);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Pirate")
        {
            health = health - 0.2f;
            if (health <= 0f)
            {
                Debug.Log("Dead");
            }
        }
        if (other.tag == "Planet")
        {
            health = health - 0.1f;
            if (health <= 0f)
            {
                Debug.Log("Dead");
            }
        }
    }
}
