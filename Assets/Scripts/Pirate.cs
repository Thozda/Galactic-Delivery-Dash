using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pirate : MonoBehaviour
{
    public Transform player;
    public GameObject[] PirateTargets;
    public Transform Target;
    private Rigidbody2D rb;
    private Vector2 movement;
    public float moveSpeed;
    private bool dead;
    public float health = 1f;
    public ParticleSystem explosionFX;
    public AudioSource kaboom;
    public GameObject BossPrefab;
    public bool IsBoss;
    public Collider2D[] Colliders;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        moveSpeed = Random.Range(2.5f, 3.9f);

        PirateTargets = GameObject.FindGameObjectsWithTag("Pirate Target");
        Target = PirateTargets[Random.Range(0, PirateTargets.Length)].transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            Vector3 direction = Target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rb.rotation = angle - 90;
            direction.Normalize();
            movement = direction;
        }
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
            //dead = true;
            //Debug.Log("boom");
            //StartCoroutine(death(other.gameObject));
        }
    }
    private IEnumerator death()
    {
        if (!dead)
        {
            int _bossChance = Random.Range(1, 31);
            if (_bossChance == 1)
            {
                GameObject[] _pirateSpawners = GameObject.FindGameObjectsWithTag("PirateSpawner");
                int _spawnLocNum = Random.Range(0, _pirateSpawners.Length);
                Instantiate(BossPrefab, _pirateSpawners[_spawnLocNum].transform.position, Quaternion.identity).GetComponent<Pirate>().IsBoss = true;
            }

            if (IsBoss)
            {
                CurrencyManager.credits += 100;
                PlayerPrefs.SetInt("credits", CurrencyManager.credits);
                PlayerPrefs.Save();
            }

            explosionFX.Play();
            kaboom.Play();
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            foreach (var i in Colliders)
            {
                i.enabled = false;
            }
            this.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            yield return new WaitForSeconds(5f);
            this.gameObject.SetActive(false);
        }
        dead = true;
    }
    public void WeaponHit()
    {
        if (PerksManager.hasBetterLasers)
        {
            health -= 0.75f;
        }
        else
        {
            health -= 0.3f;
        }
        if (health <= 0)
        {
            StartCoroutine(death());
        }
    }
    public void MissileHit()
    {
        health -= 1.5f;
        if (IsBoss)
        {
            //Debug.Log(health);
        }
        if (health <= 0)
        {
            StartCoroutine(death());
        }
    }
}
