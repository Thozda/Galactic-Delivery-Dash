using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    private Rigidbody2D rb;
    public float MissileSpeed;
    public GameObject MissileDirection;
    public GameObject ExplosionFX;
    private AudioSource ExplosionSound;

    private HashSet<GameObject> piratesHit = new HashSet<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = GameObject.FindGameObjectWithTag("PlayerRotator").transform.rotation;

        MissileDirection = GameObject.FindGameObjectWithTag("MissileDirection");
        rb = GetComponent<Rigidbody2D>();
        ExplosionSound = GameObject.FindGameObjectWithTag("ExplosionAudio").GetComponent<AudioSource>();

        rb.AddForce(MissileDirection.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Pirate" ||  collision.tag == "Planet")
        {
            this.gameObject.SetActive(false);
            Instantiate(ExplosionFX, transform.position, Quaternion.identity);
            ExplosionSound.Play();

            if (collision.tag == "Pirate")
            {
                Collider2D[] _piratesInRange = Physics2D.OverlapCircleAll(transform.position, 3);
                foreach (var i in _piratesInRange)
                {
                    if (i is CircleCollider2D && i.gameObject.GetComponent<Pirate>() != null)
                    {
                        GameObject pirate = i.gameObject;
                        if (!piratesHit.Contains(pirate))
                        {
                            piratesHit.Add(pirate);
                            pirate.GetComponent<Pirate>().MissileHit();
                        }
                    }
                }
            }
        }
    }
}
