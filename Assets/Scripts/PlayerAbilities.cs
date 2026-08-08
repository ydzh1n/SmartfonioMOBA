using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerAbilities : MonoBehaviour
{
    [Header("Teleport Ability")]
    public KeyCode teleportKey = KeyCode.T;
    public float teleportCooldown = 30f; // Кулдаун телепорта (секунды)
    public float teleportPreparationTime = 3f; // Время подготовки (без движения и урона)
    private float lastTeleportTime = -30f;
    private bool isTeleporting = false;
    private float teleportStartTime = 0f;
    private Vector3 spawnPosition;

    [Header("PowerSlash Ability")]
    public KeyCode powerSlashKey = KeyCode.Q;
    public float powerSlashCooldown = 20f; // Кулдаун PowerSlash
    public float powerSlashDuration = 5f; // Длительность эффекта
    private float lastPowerSlashTime = -20f;
    private bool isPowerSlashActive = false;
    private float powerSlashEndTime = 0f;

    [Header("References")]
    public PlayerAttack playerAttack;
    public PlayerMovement playerMovement;
    public HealthSystem healthSystem;

    [Header("UI (Optional)")]
    public Slider teleportCooldownSlider;
    public Slider powerSlashCooldownSlider;
    public TextMeshProUGUI teleportTimerText;
    public TextMeshProUGUI powerSlashTimerText;
    public TextMeshProUGUI teleportStatusText;

    private float baseAttackRange;
    private float baseAttackDamage;

    void Start()
    {
        // Сохраняем начальную позицию (точку спавна)
        spawnPosition = transform.position;

        // Сохраняем базовые значения атаки
        if (playerAttack != null)
        {
            baseAttackRange = playerAttack.attackRange;
            baseAttackDamage = playerAttack.attackDamage;
        }

        Debug.Log("PlayerAbilities initialized!");
    }

    void Update()
    {
        // Проверяем активные эффекты
        CheckActiveEffects();

        // Телепорт
        if (Input.GetKeyDown(teleportKey) && CanUseTeleport())
        {
            StartTeleport();
        }

        // PowerSlash
        if (Input.GetKeyDown(powerSlashKey) && CanUsePowerSlash())
        {
            ActivatePowerSlash();
        }

        // Обновляем UI
        UpdateUI();
    }

    // ============ ТЕЛЕПОРТ ============

    bool CanUseTeleport()
    {
        if (isTeleporting) return false;
        if (Time.time - lastTeleportTime < teleportCooldown) return false;
        if (healthSystem != null && healthSystem.GetCurrentHealth() <= 0) return false;

        return true;
    }

    void StartTeleport()
    {
        isTeleporting = true;
        teleportStartTime = Time.time;
        Debug.Log("Teleport started! Stay still and don't take damage for 3 seconds...");

        if (teleportStatusText != null)
        {
            teleportStatusText.gameObject.SetActive(true);
            teleportStatusText.text = "Teleporting...";
        }
    }

    void CompleteTeleport()
    {
        // Телепортируем игрока на spawn
        transform.position = spawnPosition;

        lastTeleportTime = Time.time;
        isTeleporting = false;

        Debug.Log("Teleport completed!");

        if (teleportStatusText != null)
        {
            teleportStatusText.gameObject.SetActive(false);
        }
    }

    void CancelTeleport()
    {
        isTeleporting = false;
        Debug.Log("Teleport cancelled!");

        if (teleportStatusText != null)
        {
            teleportStatusText.text = "Teleport cancelled!";
            Invoke(nameof(HideTeleportStatus), 1f);
        }
    }

    void HideTeleportStatus()
    {
        if (teleportStatusText != null)
        {
            teleportStatusText.gameObject.SetActive(false);
        }
    }

    // ============ POWERSLASH ============

    bool CanUsePowerSlash()
    {
        if (isPowerSlashActive) return false;
        if (Time.time - lastPowerSlashTime < powerSlashCooldown) return false;
        if (healthSystem != null && healthSystem.GetCurrentHealth() <= 0) return false;

        return true;
    }

    void ActivatePowerSlash()
    {
        isPowerSlashActive = true;
        powerSlashEndTime = Time.time + powerSlashDuration;
        lastPowerSlashTime = Time.time;

        // Увеличиваем урон и радиус в 2 раза
        if (playerAttack != null)
        {
            playerAttack.attackRange = baseAttackRange * 2f;
            playerAttack.attackDamage = baseAttackDamage * 2f;
        }

        Debug.Log("PowerSlash activated! Double damage and range for 5 seconds!");
    }

    void DeactivatePowerSlash()
    {
        isPowerSlashActive = false;

        // Возвращаем нормальные значения
        if (playerAttack != null)
        {
            playerAttack.attackRange = baseAttackRange;
            playerAttack.attackDamage = baseAttackDamage;
        }

        Debug.Log("PowerSlash ended.");
    }

    // ============ ПРОВЕРКИ ============

    void CheckActiveEffects()
    {
        // Проверяем телепорт
        if (isTeleporting)
        {
            float timeSinceStart = Time.time - teleportStartTime;

            // Проверяем, двигался ли игрок
            if (playerMovement != null && Time.time - playerMovement.LastMoveTime < 0.5f)
            {
                CancelTeleport();
                return;
            }

            // Проверяем, получал ли игрок урон
            if (healthSystem != null && Time.time - healthSystem.LastDamageTime < 1f)
            {
                CancelTeleport();
                return;
            }

            // Если прошло 3 секунды - телепортируем
            if (timeSinceStart >= teleportPreparationTime)
            {
                CompleteTeleport();
            }
        }

        // Проверяем PowerSlash
        if (isPowerSlashActive && Time.time >= powerSlashEndTime)
        {
            DeactivatePowerSlash();
        }
    }

    // ============ UI ============

    void UpdateUI()
    {
        // Телепорт UI
        if (teleportCooldownSlider != null)
        {
            float cooldownRemaining = teleportCooldown - (Time.time - lastTeleportTime);
            if (cooldownRemaining > 0)
            {
                // Slider использует value (0-1), а не fillAmount
                teleportCooldownSlider.value = cooldownRemaining / teleportCooldown;
            }
            else
            {
                teleportCooldownSlider.value = 0;
            }
        }

        if (teleportTimerText != null)
        {
            float cooldownRemaining = teleportCooldown - (Time.time - lastTeleportTime);
            if (cooldownRemaining > 0)
            {
                teleportTimerText.text = $"T: {Mathf.CeilToInt(cooldownRemaining)}s";
            }
            else
            {
                teleportTimerText.text = "T: Ready";
            }
        }

        // PowerSlash UI
        if (powerSlashCooldownSlider != null)
        {
            float cooldownRemaining = powerSlashCooldown - (Time.time - lastPowerSlashTime);
            if (cooldownRemaining > 0)
            {
                powerSlashCooldownSlider.value = cooldownRemaining / powerSlashCooldown;
            }
            else
            {
                powerSlashCooldownSlider.value = 0;
            }
        }

        if (powerSlashTimerText != null)
        {
            float cooldownRemaining = powerSlashCooldown - (Time.time - lastPowerSlashTime);
            if (cooldownRemaining > 0)
            {
                powerSlashTimerText.text = $"Q: {Mathf.CeilToInt(cooldownRemaining)}s";
            }
            else
            {
                powerSlashTimerText.text = "Q: Ready";
            }
        }
    }
}