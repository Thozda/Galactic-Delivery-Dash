using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pirate : MonoBehaviour
{
    public Transform player;
    private Rigidbody2D rb;
    private Vector2 movement;
    public float moveSpeed;
    private bool dead;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        moveSpeed = Random.Range(2.5f, 3.9f);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.rotation = angle - 90;
        direction.Normalize();
        movement = direction;
    }

    private void FixedUpdate()
    {
        moveCharacter(movement);
    }

    void moveCharacter(Vector2 direction)
    {
        if (!dead)
        {
            rb.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Pirate")
        {
            dead = true;
            Debug.Log("boom");
            StartCoroutine(death(other.gameObject));
        }
    }
    private IEnumerator death(GameObject other)
    {
        yield return new WaitForSeconds(5);
        other.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
