using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject enemyPrefab;
    public float spawnInterval = 5f;
    public int enemiesPerWave = 3;

    [Header("Spawn Area")]
    public Vector2 spawnAreaSize = new Vector2(5, 5);

    [Header("Player Safety")]
    public float minDistanceFromPlayer = 8f;

    private float lastSpawnTime = 0f;
    private Transform playerTransform;
    private Transform playerBaseTransform; // Кэшируем ссылку на базу

    void Start()
    {
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        // Находим базу ОДИН раз при старте
        GameObject playerBase = GameObject.Find("PlayerBase");
        if (playerBase != null)
        {
            playerBaseTransform = playerBase.transform;
        }
        else
        {
            Debug.LogWarning("EnemySpawner: PlayerBase not found!");
        }
    }

    void Update()
    {
        // Если игра закончилась — не спавним
        if (GameManager.Instance != null && !GameManager.Instance.IsGameActive)
        {
            return;
        }

        if (Time.time - lastSpawnTime >= spawnInterval)
        {
            SpawnWave();
            lastSpawnTime = Time.time;
        }
    }

    void SpawnWave()
    {
        int spawnedCount = 0;
        int maxAttempts = enemiesPerWave * 5;
        int attempts = 0;

        while (spawnedCount < enemiesPerWave && attempts < maxAttempts)
        {
            attempts++;

            Vector3 randomOffset = new Vector3(
                Random.Range(-spawnAreaSize.x, spawnAreaSize.x),
                0,
                Random.Range(-spawnAreaSize.y, spawnAreaSize.y)
            );

            Vector3 spawnPosition = transform.position + randomOffset;

            if (playerTransform != null)
            {
                float distanceToPlayer = Vector3.Distance(spawnPosition, playerTransform.position);

                if (distanceToPlayer < minDistanceFromPlayer)
                {
                    // Просто пропускаем итерацию, без спама в консоль
                    continue;
                }
            }

            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

            EnemyAI ai = enemy.GetComponent<EnemyAI>();
            if (ai != null && playerBaseTransform != null)
            {
                // Используем уже найденную ссылку, а не ищем заново
                ai.target = playerBaseTransform;
            }

            spawnedCount++;
        }
    }
}