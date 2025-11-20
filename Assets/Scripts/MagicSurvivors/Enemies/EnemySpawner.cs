using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagicSurvivors.Data;
using MagicSurvivors.Core;

namespace MagicSurvivors.Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Enemy Database")]
        [SerializeField] private List<EnemyData> basicEnemies = new List<EnemyData>();
        [SerializeField] private List<EnemyData> eliteEnemies = new List<EnemyData>();
        [SerializeField] private EnemyData miniBoss1Data;
        [SerializeField] private EnemyData miniBoss2Data;
        [SerializeField] private EnemyData miniBoss3Data;
        [SerializeField] private EnemyData finalBossData;
        
        [Header("Spawn Settings")]
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private float baseSpawnInterval = 2f;
        [SerializeField] private int maxEnemyCount = 50;
        [SerializeField] private float spawnRangeMin = 10f;
        [SerializeField] private float spawnRangeMax = 15f;
        
        private Transform playerTransform;
        private int currentEnemyCount = 0;
        private Coroutine spawnCoroutine;
        
        private void Start()
        {
            FindPlayer();
            
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameStart += StartSpawning;
                GameManager.Instance.OnGameEnd += StopSpawning;
                GameManager.Instance.OnMiniBoss1Time += SpawnMiniBoss1;
                GameManager.Instance.OnMiniBoss2Time += SpawnMiniBoss2;
                GameManager.Instance.OnMiniBoss3Time += SpawnMiniBoss3;
                GameManager.Instance.OnFinalBossTime += SpawnFinalBoss;
            }
        }
        
        private void OnDestroy()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameStart -= StartSpawning;
                GameManager.Instance.OnGameEnd -= StopSpawning;
                GameManager.Instance.OnMiniBoss1Time -= SpawnMiniBoss1;
                GameManager.Instance.OnMiniBoss2Time -= SpawnMiniBoss2;
                GameManager.Instance.OnMiniBoss3Time -= SpawnMiniBoss3;
                GameManager.Instance.OnFinalBossTime -= SpawnFinalBoss;
            }
        }
        
        private void FindPlayer()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
        }
        
        private void StartSpawning()
        {
            if (spawnCoroutine == null)
            {
                spawnCoroutine = StartCoroutine(SpawnEnemyCoroutine());
            }
        }
        
        private void StopSpawning()
        {
            if (spawnCoroutine != null)
            {
                StopCoroutine(spawnCoroutine);
                spawnCoroutine = null;
            }
        }
        
        private IEnumerator SpawnEnemyCoroutine()
        {
            while (true)
            {
                float densityMultiplier = GameManager.Instance != null ? 
                    GameManager.Instance.GetSpawnDensityMultiplier() : 1f;
                
                float adjustedInterval = baseSpawnInterval / densityMultiplier;
                yield return new WaitForSeconds(adjustedInterval);
                
                if (currentEnemyCount < maxEnemyCount)
                {
                    SpawnRandomEnemy();
                }
            }
        }
        
        private void SpawnRandomEnemy()
        {
            EnemyData enemyData = SelectRandomEnemyData();
            if (enemyData != null)
            {
                SpawnEnemy(enemyData);
            }
        }
        
        private EnemyData SelectRandomEnemyData()
        {
            float eliteChance = 0.1f;
            
            if (Random.value < eliteChance && eliteEnemies.Count > 0)
            {
                return eliteEnemies[Random.Range(0, eliteEnemies.Count)];
            }
            else if (basicEnemies.Count > 0)
            {
                return basicEnemies[Random.Range(0, basicEnemies.Count)];
            }
            
            return null;
        }
        
        private void SpawnEnemy(EnemyData enemyData)
        {
            Vector3 spawnPosition = GetValidSpawnPosition();
            
            if (spawnPosition != Vector3.zero && enemyPrefab != null)
            {
                GameObject enemyObj = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                Enemy enemy = enemyObj.GetComponent<Enemy>();
                
                if (enemy != null)
                {
                    enemy.InitializeWithData(enemyData);
                    enemy.OnEnemyDeath += OnEnemyDeath;
                    currentEnemyCount++;
                }
            }
        }
        
        private Vector3 GetValidSpawnPosition()
        {
            if (playerTransform == null) return Vector3.zero;
            
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            float distance = Random.Range(spawnRangeMin, spawnRangeMax);
            
            Vector3 offset = new Vector3(
                Mathf.Cos(angle) * distance,
                Mathf.Sin(angle) * distance,
                0f
            );
            
            return playerTransform.position + offset;
        }
        
        private void OnEnemyDeath(Enemy enemy)
        {
            currentEnemyCount--;
            enemy.OnEnemyDeath -= OnEnemyDeath;
        }
        
        private void SpawnMiniBoss1(float time)
        {
            if (miniBoss1Data != null)
            {
                SpawnEnemy(miniBoss1Data);
                Debug.Log("EnemySpawner: Spawned Mini-Boss 1 (Flame Ogre)");
            }
        }
        
        private void SpawnMiniBoss2(float time)
        {
            if (miniBoss2Data != null)
            {
                SpawnEnemy(miniBoss2Data);
                Debug.Log("EnemySpawner: Spawned Mini-Boss 2 (Wind Harpy)");
            }
        }
        
        private void SpawnMiniBoss3(float time)
        {
            if (miniBoss3Data != null)
            {
                SpawnEnemy(miniBoss3Data);
                Debug.Log("EnemySpawner: Spawned Mini-Boss 3 (Ice Golem)");
            }
        }
        
        private void SpawnFinalBoss(float time)
        {
            if (finalBossData != null)
            {
                SpawnEnemy(finalBossData);
                Debug.Log("EnemySpawner: Spawned Final Boss (Ancient Dragon)");
            }
        }
    }
}
