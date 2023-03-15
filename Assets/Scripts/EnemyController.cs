using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Rigidbody enemyRB;
    GameObject player;
    public float speed;
    public float enemyAttackRadius = 8;

    //Boss variables
    public bool isBoss = false;
    public float spawnInterval;
    private float nextSpawn;
    public int miniEnemySpawnCount;
    private SpawnManager spawnManager;

    void Start()
    {
        enemyRB = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");

        if (isBoss) spawnManager = FindObjectOfType<SpawnManager>();

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lookDir = (player.transform.position - transform.position).normalized;
        enemyRB.AddForce(lookDir * speed);

        if (isBoss)
        {
            if (Time.time > nextSpawn)
            { 
                nextSpawn = Time.time + spawnInterval;
                spawnManager.SpawnMiniEnemy(miniEnemySpawnCount);
            }
        }

        if (transform.position.y < -1) speed = 0;
        if (transform.position.y < -20) Destroy(this.gameObject);
    }
}
