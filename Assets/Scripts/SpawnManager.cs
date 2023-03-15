using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    public GameObject[] enemyPrefabs;
    public GameObject[] powerUpPrefabs;
    public float _spawnRange = 9.0f;
    public int enemyCount;
    //private int powerUpsToDestroy;
    PlayerController playerController;

    //Boss variables
    public GameObject bossPrefab;
    public GameObject[] miniEnemyPrefabs;
    public int bossRound = 3;


    public int waveNumber = 1;

    // Start is called before the first frame update
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        SpawnEnemyWave(waveNumber);
        SpawnPowerUp(waveNumber);
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = FindObjectsOfType<EnemyController>().Length;
        if (enemyCount == 0)
        {
            playerController.hasPowerUp = false;
            waveNumber++;
            if (waveNumber % bossRound == 0)
            {
                SpawnBossWave(waveNumber);
                SpawnPowerUp(waveNumber / 2);
            } 
            else 
            {
                SpawnEnemyWave(waveNumber);
                SpawnPowerUp(waveNumber / 2);
            }
        } 
    }

    private Vector3 GenerateSpawnPosition()
    {
        float spwanPosX = Random.Range(-_spawnRange, _spawnRange);
        float spwanPosZ = Random.Range(-_spawnRange, _spawnRange);
        Vector3 randomPos = new Vector3(spwanPosX, 0, spwanPosZ);
        
        return randomPos;
    }

    void SpawnBossWave(int currentRound)
    {
        int miniEnemiesToSpawn;

        //We don't want to divide by zero!
        if (bossRound != 0) miniEnemiesToSpawn = currentRound / bossRound;
        else miniEnemiesToSpawn = 1;

        var boss = Instantiate(bossPrefab, GenerateSpawnPosition(), bossPrefab.transform.rotation);
        boss.GetComponent<EnemyController>().miniEnemySpawnCount = miniEnemiesToSpawn;

    }

    public void SpawnMiniEnemy(int amount)
    {
        for (int i = 0; i < amount; i++)
        { 
            int randomMini = Random.Range(0, miniEnemyPrefabs.Length);
            Instantiate(miniEnemyPrefabs[randomMini], GenerateSpawnPosition(), miniEnemyPrefabs[randomMini].transform.rotation);
        }
    }

    public void SpawnEnemyWave(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            int randomEnemy = Random.Range(0,enemyPrefabs.Length);
            Instantiate(enemyPrefabs[randomEnemy], GenerateSpawnPosition(), enemyPrefabs[randomEnemy].transform.rotation);
        }
    }

    public void SpawnPowerUp(int powerUpsToSpawn)
    {
        for (int i = 0; i < powerUpsToSpawn; i++)
        {
            int randomPowerup = Random.Range(0, powerUpPrefabs.Length);
            Instantiate(powerUpPrefabs[randomPowerup], GenerateSpawnPosition(), powerUpPrefabs[randomPowerup].transform.rotation);
        }
            


    }
}
