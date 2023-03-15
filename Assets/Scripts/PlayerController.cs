using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rb;
    private GameObject _centerPoint;
    private SpawnManager _spawnManager;
    private GameManager _gameManager;

    [Header("Player Controls")]
    public float speed = 5.0f;
    public int lives = 3; 
    private Light _light;
    private Vector3 startPos;

    [HideInInspector] public PowerUpType currentPowerUp = PowerUpType.None;

    [Header("Power Ups")]
    public GameObject powerUpIndicator;

    [HideInInspector] public bool hasPowerUp = false;

    [Header("Pushback PowerUp")] 
    public float powerUpStrength = 15f;
    public float powerUpTimer = 7f;

    [Header("Rocket PowerUp")]
    public GameObject rocketPrefab;
    private GameObject tmpRocket;
    private Coroutine powerUpCountDown;

    [Header("Smash PowerUp")]
    public float hangTime = 0.3f;
    public float smashSpeed = 20f;
    public float explosionForce = 300f;
    public float explosionRadius = 15f;
    bool smashing = false;
    float floorY;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _light = GetComponent<Light>();
        _spawnManager = FindObjectOfType<SpawnManager>();
        _gameManager= FindObjectOfType<GameManager>();
        _centerPoint = GameObject.Find("CenterPoint");
        startPos = transform.position;
    }

    void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");
        float SidesInput = Input.GetAxis("Horizontal");
        _rb.AddForce(_centerPoint.transform.forward * speed * forwardInput);
        _rb.AddForce(_centerPoint.transform.right * speed * SidesInput);

        powerUpIndicator.transform.position = transform.position + new Vector3(0,-0.5f,0);

        if (transform.position.y < -10)
        {
            _gameManager.lives--;
            transform.position = startPos;
        }

        if (currentPowerUp == PowerUpType.Rockets && Input.GetKeyDown(KeyCode.F)) LaunchRockets();

        if (currentPowerUp == PowerUpType.Smash && Input.GetKeyDown(KeyCode.Space) && !smashing)
        {
            smashing = true;
            StartCoroutine(Smash());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerUp") && !hasPowerUp)
        {
            hasPowerUp = true;
            currentPowerUp = other.gameObject.GetComponent<PowerUpsBase>().powerUpType;
            _light.enabled = true;
            powerUpIndicator.SetActive(true);
            Destroy(other.gameObject);
            StartCoroutine(PowerUpCountDownRoutine());

            if (powerUpCountDown != null)
            { 
                StopCoroutine(powerUpCountDown);
            }
            powerUpCountDown = StartCoroutine(PowerUpCountDownRoutine());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && currentPowerUp == PowerUpType.Pushback && hasPowerUp)
        {
            Rigidbody enemyRB = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position);

            Debug.Log("Collided with " + collision.gameObject.name + " with PowerUp set to " + currentPowerUp.ToString());
            enemyRB.AddForce(awayFromPlayer * powerUpStrength, ForceMode.Impulse);
        }
    }

    void LaunchRockets()
    {
        foreach (var enemy in FindObjectsOfType<EnemyController>())
        {
            tmpRocket = Instantiate(rocketPrefab, (transform.position - new Vector3( 0, 0, 0)) + Vector3.up, Quaternion.identity);
            tmpRocket.GetComponent<RocketBehaviour>().Fire(enemy.transform);
        }
    }

    IEnumerator PowerUpCountDownRoutine()
    {
        yield return new WaitForSeconds(powerUpTimer);
        hasPowerUp = false;
        currentPowerUp = PowerUpType.None;
        _light.enabled = false;
        powerUpIndicator.SetActive(false);
    }

    IEnumerator Smash()
    { 
        var enemies = FindObjectsOfType<EnemyController>();
        floorY = transform.position.y;
        float jumptime = Time.time + hangTime;

        while (Time.time < jumptime)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, smashSpeed);
            yield return null;
        }

        while (transform.position.y > floorY)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, -smashSpeed * 2);
            yield return null;
        }

        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
                enemies[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRadius, 0.0f, ForceMode.Impulse);
        }

        smashing = false;

    }

}
