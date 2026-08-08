using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerRespawn : MonoBehaviour
{
    [Header("Respawn Settings")]
    public float respawnDelay = 5f;
    public float invincibilityTime = 2f;

    [Header("UI")]
    public GameObject respawnScreen;
    public TextMeshProUGUI respawnTimerText;

    private HealthSystem healthSystem;
    private Rigidbody rb;
    private PlayerMovement movement;
    private PlayerAttack attack;
    private Vector3 spawnPosition;
    private bool isDead = false;
    private float respawnTimeRemaining;
    private bool isRespawning = false;
    public bool IsDead => isDead;

    void Start()
    {
        spawnPosition = transform.position;

        healthSystem = GetComponent<HealthSystem>();
        rb = GetComponent<Rigidbody>();
        movement = GetComponent<PlayerMovement>();
        attack = GetComponent<PlayerAttack>();

        if (healthSystem != null)
        {
            healthSystem.onDeath.AddListener(OnPlayerDeath);
        }
    }

    void OnPlayerDeath()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("Player died!");

        if (movement != null) movement.enabled = false;
        if (attack != null) attack.enabled = false;
        if (rb != null) rb.isKinematic = true;

        // Вместо деактивации объекта - только скрываем Renderer
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.enabled = false;
        }

        // Показываем экран респавна
        if (respawnScreen != null)
        {
            respawnScreen.SetActive(true);
        }

        // Запускаем таймер
        respawnTimeRemaining = respawnDelay;
        isRespawning = true;

        // Корутина теперь будет работать, т.к. объект активен
        StartCoroutine(UpdateRespawnTimer());

        Invoke(nameof(Respawn), respawnDelay);
    }

    IEnumerator UpdateRespawnTimer()
    {
        Debug.Log("UpdateRespawnTimer started!");

        while (isRespawning && respawnTimeRemaining > 0)
        {
            if (respawnTimerText != null)
            {
                int secondsLeft = Mathf.CeilToInt(respawnTimeRemaining);
                respawnTimerText.text = "Respawning in " + secondsLeft + "...";
                //Debug.Log("Timer: " + secondsLeft);
            }

            yield return new WaitForSeconds(0.1f);
            respawnTimeRemaining -= 0.1f;
        }

        Debug.Log("UpdateRespawnTimer finished!");
    }

    void Respawn()
    {
        Debug.Log("Player respawned!");

        // Включаем Renderer вместо активации объекта
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.enabled = true;
        }

        transform.position = spawnPosition;

        if (healthSystem != null)
        {
            healthSystem.Heal(healthSystem.GetMaxHealth());
        }

        if (movement != null) movement.enabled = true;
        if (attack != null) attack.enabled = true;
        if (rb != null) rb.isKinematic = false;

        if (respawnScreen != null)
        {
            respawnScreen.SetActive(false);
        }

        isRespawning = false;
        isDead = false;

        Debug.Log("Player is back in game!");
    }
}