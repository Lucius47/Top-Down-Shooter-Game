﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public event System.Action<int> OnNewWave;

    Wave currentWave;
    int currentWaveNumber;

    int enemiesRemainingToSpawn;
    float nextSpawnTime;
    int enemiesRemainingAlive;
    
    public Enemy enemy;
    public Wave[] waves;

    LivingEntity playerEntity;
    Transform playerT;

    MapGenerator map;

    float timeBetweenCampingChecks = 2;
    float nextCampCheckTime;
    float campThresholdDistance = 1.5f;

    Vector3 campPositionOld;
    bool isCamping;
    bool isDisabled;


    void Start()
    {
        playerEntity = FindObjectOfType<Player>();
        playerT = playerEntity.transform;

        nextCampCheckTime = timeBetweenCampingChecks + Time.time;
        campPositionOld = playerT.position;

        playerEntity.OnDeath += OnPlayerDeath;

        map = FindObjectOfType<MapGenerator>();
        NextWave();
    }
    void Update()
    {

        if (!isDisabled)
        {
            if (Time.time > nextCampCheckTime)
            {
                nextCampCheckTime = Time.time + timeBetweenCampingChecks;

                isCamping = (Vector3.Distance(playerT.position, campPositionOld) < campThresholdDistance);
                campPositionOld = playerT.position;
            }

            if (enemiesRemainingToSpawn > 0 && Time.time > nextSpawnTime)
            {
                enemiesRemainingToSpawn--;
                nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;

                StartCoroutine(SpawnEnemy());
            }
        }
    }

    IEnumerator SpawnEnemy ()
    {
        float spawnDelay = 1;
        float tileFlashSpeed = 4;

        Transform spawnTile = map.GetRandomOpenTile();
        
        if (isCamping)
        {
            spawnTile = map.GetTileFromPosition(playerT.position);
        }

        Material tileMaterial = spawnTile.GetComponent<Renderer>().material;
        Color initialColour = tileMaterial.color;
        Color flashColour = Color.red;
        float spawnTimer = 0;

        while (spawnTimer < spawnDelay)
        {
            tileMaterial.color = Color.Lerp(initialColour, flashColour, Mathf.PingPong(spawnTimer * tileFlashSpeed, 1));

            spawnTimer += Time.deltaTime;
            yield return null;
        }

        Enemy spawnedEnemy = Instantiate(enemy, spawnTile.position - Vector3.up, Quaternion.identity) as Enemy;
            spawnedEnemy.OnDeath += OnEnemyDeath;
    }


    
    void OnPlayerDeath()
    {
        isDisabled = true;
    }
    
    void OnEnemyDeath()
    {
        enemiesRemainingAlive--;
        if (enemiesRemainingAlive == 0)
        {
            NextWave();
        }
    }

    void ResetPlayerPosition ()
    {
        playerT.position = map.GetTileFromPosition(Vector3.zero).position + Vector3.up * 3;
    }

    void NextWave()
    {
        currentWaveNumber++;

        if (currentWaveNumber - 1 < waves.Length)
        {
            currentWave = waves[currentWaveNumber - 1];
            enemiesRemainingToSpawn = currentWave.enemyCount;
            enemiesRemainingAlive = enemiesRemainingToSpawn;
            OnNewWave?.Invoke(currentWaveNumber);
            ResetPlayerPosition();
        }
    }

    [System.Serializable]
    public class Wave
    {
        public int enemyCount;
        public float timeBetweenSpawns;
    }
}