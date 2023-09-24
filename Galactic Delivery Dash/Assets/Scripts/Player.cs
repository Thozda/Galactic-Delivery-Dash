using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public MovementJoystick movementJoystick;
    public float playerSpeed;
    public float rotationSpeed;
    private Rigidbody2D rb;
    public float health = 1f;
    public Text healthDisplay;

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

        healthDisplay.text = health.ToString();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Pirate")
        {
            health = health - 0.3f;
            if (health <= 0f)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
            }
        }
        if (other.tag == "Planet")
        {
            health = health - 0.1f;
            if (health <= 0f)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
            }
        }
    }
}
