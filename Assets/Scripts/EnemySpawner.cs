using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject enemyPrefab; // Префаб врага
    public float spawnInterval = 5f; // Интервал между спавнами (секунды)
    public int enemiesPerWave = 3; // Количество врагов в волне

    [Header("Spawn Area")]
    public Vector2 spawnAreaSize = new Vector2(5, 5); // Область спавна

    private float lastSpawnTime = 0f;

    void Update()
    {
        // Проверяем время для спавна
        if (Time.time - lastSpawnTime >= spawnInterval)
        {
            SpawnWave();
            lastSpawnTime = Time.time;
        }
    }

    void SpawnWave()
    {
        for (int i = 0; i < enemiesPerWave; i++)
        {
            // Случайная позиция в области спавна
            Vector3 randomOffset = new Vector3(
                Random.Range(-spawnAreaSize.x, spawnAreaSize.x),
                0,
                Random.Range(-spawnAreaSize.y, spawnAreaSize.y)
            );

            Vector3 spawnPosition = transform.position + randomOffset;

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
        }

        Debug.Log($"Spawned wave of {enemiesPerWave} enemies!");
    }
}