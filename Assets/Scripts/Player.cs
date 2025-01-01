using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Xml;
using UnityEngine.Rendering.PostProcessing;

public class Player : MonoBehaviour
{
    public MovementJoystick movementJoystick;
    public float playerSpeed;
    public float rotationSpeed;
    private Rigidbody2D rb;
    public float health = 1f;
    public Image HealthBarFont;
    public bool isShooting;
    public Transform turretDir;
    public Vector3 turretDir2D;
    public AudioSource turretHitAudio;
    public AudioSource turretMissAudio;
    public AudioSource ThrustAudio;
    public AudioSource DamageAudio;
    public LineRenderer laserRenderer;
    public Transform laserPosObj;
    public Vector2 laserPos;
    public float laserDuration;
    public GameObject laserTarget;
    public Vector2 targetPos2D;
    private bool _IsMoving;
    public GameObject PlayerRotator;
    private float _DoubleClickDelay;
    private float lastTapTime;
    private bool waitingForDoubleTap = false;
    public LayerMask uiLayerMask;
    public ParticleSystem ThrustParticles;
    public ParticleSystem DamageSparks;
    public GameObject MissilePrefab;
    public float MissileCooldown;
    public Image MissileButton;
    public Sprite[] MissileButtonSprites;
    private int _MissileButtonSpriteSelector;
    private float _MissileButtonFlashingTimer = 0;
    public GameObject HealthRegenTimer;
    private float _LastUpdate;
    public Animator DamageAnimator;
    public Sprite[] PlayerShips;
    public SpriteRenderer SpriteRenderer;
    public float Intensity = 0;
    public PostProcessVolume Volume;
    private Vignette _Vignette;
    public GameObject DeathOverlay;
    private bool _DiedAlready;
    public Material RedLaser;
    public Material GreenLaser;
    private float _LastDamageRealTime;
    private float _LastUpdateRealTime;

    // Start is called before the first frame update
    void Start()
    {
        playerSpeed = 4f;

        if (PerksManager.hasUltraArmour == true)
        {
            health = 3f;
        }
        else if (PerksManager.hasBetterArmour == true)
        {
            health = 2f;
        }
        else
        {
            health = 1f;
        }

        SpriteRenderer.sprite = PlayerShips[SkinManager.SkinSelected];
        rb = GetComponent<Rigidbody2D>();
        _DoubleClickDelay = 0.25f;
        lastTapTime = 0f;
        _LastDamageRealTime = -30f;
        Volume.profile.TryGetSettings<Vignette>(out _Vignette);
        _Vignette.enabled.Override(false);
        _DiedAlready = false;
    }

    private void Update()
    {
        //health bar
        if (PerksManager.hasUltraArmour == true)
        {
            HealthBarFont.fillAmount = Mathf.Lerp(HealthBarFont.fillAmount, health / 3, Time.deltaTime * 2);
        }
        else if (PerksManager.hasBetterArmour == true)
        {
            HealthBarFont.fillAmount = Mathf.Lerp(HealthBarFont.fillAmount, health / 2, Time.deltaTime * 2);
        }
        else
        {
            HealthBarFont.fillAmount = Mathf.Lerp(HealthBarFont.fillAmount, health, Time.deltaTime * 2);
        }

        //Movement
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 tapPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            tapPosition.z = 0;

            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                return;
            }

            //Single tap to rotate
            RotateTowards(tapPosition);

            // Handle double-tap for movement
            if (Time.time - lastTapTime <= _DoubleClickDelay)
            {
                MoveInDirection((tapPosition - transform.position).normalized);
                ThrustParticles.Play();
                ThrustAudio.Play();
            }

            lastTapTime = Time.time;
        }

        if (MissileCooldown > 0)
        {
            MissileCooldown -= Time.deltaTime;

            _MissileButtonFlashingTimer += Time.deltaTime;
            if (_MissileButtonFlashingTimer >= 0.5)
            {
                //Debug.Log("Flashing");
                _MissileButtonFlashingTimer = 0;
                if (_MissileButtonSpriteSelector == 0)
                {
                    _MissileButtonSpriteSelector = 1;
                    MissileButton.sprite = MissileButtonSprites[1];
                    //Debug.Log("Flashing off");
                }
                else
                {
                    _MissileButtonSpriteSelector = 0;
                    MissileButton.sprite = MissileButtonSprites[0];
                    //Debug.Log("Flashing on");
                }
            }
        }
        else
        {
            MissileButton.sprite = MissileButtonSprites[0];
        }
    }

    void FixedUpdate()
    {
        if (Time.realtimeSinceStartup - _LastDamageRealTime <= 30)
        {
            HealthRegenTimer.SetActive(true);
            if (Time.realtimeSinceStartup - _LastUpdateRealTime >= 1)
            {
                _LastUpdateRealTime = Time.realtimeSinceStartup;
                HealthRegenTimer.GetComponent<Image>().fillAmount =
                    Mathf.Clamp01((30 - (Time.realtimeSinceStartup - _LastDamageRealTime)) / 30);
            }
        }
        else
        {
            HealthRegenTimer.SetActive(false);
            if (PerksManager.hasUltraArmour)
            {
                health = Mathf.Clamp(health + 0.00035f, 0, 3);
            }
            else if (PerksManager.hasBetterArmour)
            {
                health = Mathf.Clamp(health + 0.00035f, 0, 2);
            }
            else
            {
                health = Mathf.Clamp01(health + 0.00035f);
            }
        }
    }

    public void ReviveLastDamageTimeUpdate()
    {
        _LastDamageRealTime = Time.realtimeSinceStartup - 31;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Pirate")
        {
            TakeDamage(0.2f);
        }

        if (other.tag == "Planet")
        {
            TakeDamage(0.1f);
        }
    }

    private void TakeDamage(float _damageAmount)
    {
        health = health - _damageAmount;
        if (health < 0.1f)
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);

            Death();
        }
        //LastDamageTime = Time.time;
        _LastDamageRealTime = Time.realtimeSinceStartup;
        _LastUpdate = Time.time;
        DamageSparks.Play();
        DamageAudio.Play();

        StartCoroutine(DamageFlash());
    }
    IEnumerator DamageFlash()
    {
        Intensity = 0.7f;
        _Vignette.enabled.Override(true);
        _Vignette.intensity.Override(0.7f);

        yield return new WaitForSeconds(0.1f);

        while (Intensity > 0)
        {
            Intensity -= 0.1f;
            if (Intensity < 0) Intensity = 0;
            _Vignette.intensity.Override(Intensity);
            yield return new WaitForSeconds(0.05f);
        }

        _Vignette.enabled.Override(false);
        yield break;
    }

    private void Death()
    {
        Time.timeScale = 0;

        if (_DiedAlready)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
        }
        else
        {
            _DiedAlready = true;
            DeathOverlay.SetActive(true);
        }
    }

    public void LaserFire()
    {
        RaycastHit2D hit;
        Vector2 rayStart = new Vector2(PlayerRotator.transform.position.x, PlayerRotator.transform.position.y - 0.65f);
        hit = Physics2D.Raycast(transform.position, -(PlayerRotator.transform.up));

        if (hit.collider != null && hit.collider.CompareTag("Pirate"))
        {
            laserTarget = hit.collider.gameObject;
            targetPos2D = new Vector2(laserTarget.transform.position.x, laserTarget.transform.position.y);
            hit.collider.gameObject.GetComponent<Pirate>().WeaponHit();
            turretHitAudio.Play();
            laserRenderer.SetPosition(0, laserPosObj.position);
            laserRenderer.SetPosition(1, laserTarget.transform.position);
        }
        else
        {
            turretMissAudio.Play();

            // Convert local position to global position relative to laserPosObj
            Vector3 turretDirGlobal = laserPosObj.TransformPoint(turretDir.localPosition);

            Vector2 endPosition = new Vector2(turretDirGlobal.x, turretDirGlobal.y);
            laserRenderer.SetPosition(0, laserPosObj.position);
            laserRenderer.SetPosition(1, endPosition);
        }

        StartCoroutine(ShootLaser());
    }

    IEnumerator ShootLaser()
    {
        if (PerksManager.hasBetterLasers)
        {
            laserRenderer.material = GreenLaser;
        }
        else
        {
            laserRenderer.material = RedLaser;
        }
        laserRenderer.enabled = true;
        yield return new WaitForSeconds(laserDuration);
        laserRenderer.enabled = false;
    }


    public void MissileFire()
    {
        if (MissileCooldown <= 0)
        {
            Instantiate(MissilePrefab, PlayerRotator.transform.position, Quaternion.identity);
            if (PerksManager.hasShorterMissileCooldown)
            {
                MissileCooldown = 3;
            }
            else
            {
                MissileCooldown = 8;
            }
            _MissileButtonSpriteSelector = 1;
            MissileButton.sprite = MissileButtonSprites[1];
        }
    }

    private void RotateTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        PlayerRotator.transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }

    private void MoveInDirection(Vector3 direction)
    {
        //direction = direction.normalized;
        rb.velocity = direction * playerSpeed;
    }
}
