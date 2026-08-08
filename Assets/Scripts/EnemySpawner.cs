using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject enemyPrefab; // Префаб врага
    public float spawnInterval = 5f; // Интервал между спавнами (секунды)
    public int enemiesPerWave = 3; // Количество врагов в волне

    [Header("Spawn Area")]
    public Vector2 spawnAreaSize = new Vector2(5, 5); // Область спавна

    [Header("Player Safety")]
    public float minDistanceFromPlayer = 8f; // Минимальное расстояние от игрока

    private float lastSpawnTime = 0f;
    private Transform playerTransform;

    void Start()
    {
        // Находим игрока
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            Debug.Log("EnemySpawner: Player found!");
        }
        else
        {
            Debug.LogWarning("EnemySpawner: Player not found! Spawning without distance check.");
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
        int maxAttempts = enemiesPerWave * 5; // Максимум попыток (чтобы не зациклиться)
        int attempts = 0;

        while (spawnedCount < enemiesPerWave && attempts < maxAttempts)
        {
            attempts++;

            // Генерируем случайную позицию в области спавна
            Vector3 randomOffset = new Vector3(
                Random.Range(-spawnAreaSize.x, spawnAreaSize.x),
                0,
                Random.Range(-spawnAreaSize.y, spawnAreaSize.y)
            );

            Vector3 spawnPosition = transform.position + randomOffset;

            // Проверяем расстояние до игрока
            if (playerTransform != null)
            {
                float distanceToPlayer = Vector3.Distance(spawnPosition, playerTransform.position);

                // Если слишком близко к игроку — пробуем другую позицию
                if (distanceToPlayer < minDistanceFromPlayer)
                {
                    Debug.Log($"Attempt {attempts}: Too close to player ({distanceToPlayer:F1}m), retrying...");
                    continue;
                }
            }

            // Создаём врага
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

            // Настраиваем цель врага (база игрока)
            EnemyAI ai = enemy.GetComponent<EnemyAI>();
            if (ai != null)
            {
                GameObject playerBase = GameObject.Find("PlayerBase");
                if (playerBase != null)
                {
                    ai.target = playerBase.transform;
                }
            }

            spawnedCount++;
            Debug.Log($"Spawned enemy {spawnedCount}/{enemiesPerWave} at {spawnPosition}");
        }

        if (spawnedCount < enemiesPerWave)
        {
            Debug.LogWarning($"Could only spawn {spawnedCount}/{enemiesPerWave} enemies (max attempts reached)");
        }
        else
        {
            Debug.Log($"Spawned wave of {enemiesPerWave} enemies!");
        }
    }
}