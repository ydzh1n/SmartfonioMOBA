using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [Header("Respawn Settings")]
    public float respawnDelay = 5f; // Задержка перед респавном
    public float invincibilityTime = 2f; // Время неуязвимости после респавна

    private HealthSystem healthSystem;
    private Rigidbody rb;
    private PlayerMovement movement;
    private PlayerAttack attack;
    private Vector3 spawnPosition;
    private bool isDead = false;

    void Start()
    {
        // Сохраняем начальную позицию
        spawnPosition = transform.position;

        // Получаем компоненты
        healthSystem = GetComponent<HealthSystem>();
        rb = GetComponent<Rigidbody>();
        movement = GetComponent<PlayerMovement>();
        attack = GetComponent<PlayerAttack>();

        // Подписываемся на смерть
        if (healthSystem != null)
        {
            healthSystem.onDeath.AddListener(OnPlayerDeath);
        }
    }

    void OnPlayerDeath()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("Player died! Respawning in " + respawnDelay + " seconds...");

        // Отключаем компоненты
        if (movement != null) movement.enabled = false;
        if (attack != null) attack.enabled = false;

        // Скрываем игрока
        gameObject.SetActive(false);

        // Запускаем таймер респавна
        Invoke(nameof(Respawn), respawnDelay);
    }

    void Respawn()
    {
        Debug.Log("Player respawned!");

        // Включаем игрока
        gameObject.SetActive(true);

        // Возвращаем на начальную позицию
        transform.position = spawnPosition;

        // Восстанавливаем здоровье
        if (healthSystem != null)
        {
            healthSystem.Heal(healthSystem.GetMaxHealth());
        }

        // Включаем компоненты
        if (movement != null) movement.enabled = true;
        if (attack != null) attack.enabled = true;

        isDead = false;

        Debug.Log("Player is back in game!");
    }
}