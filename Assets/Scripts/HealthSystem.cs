using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour, IDamageable
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    [Header("Death Settings")]
    [SerializeField] private bool destroyOnDeath = true; // Уничтожать объект при смерти

    [Header("Events")]
    public UnityEvent onDamageTaken;
    public UnityEvent onDeath;
    public UnityEvent<float> onHealthChanged; // Передаёт текущее здоровье

    void Start()
    {
        currentHealth = maxHealth;
        // Сообщаем UI о текущем здоровье при старте/респавне
        onHealthChanged?.Invoke(currentHealth);
    }

    // Реализация интерфейса IDamageable
    public void TakeDamage(float damage)
    {
        if (currentHealth <= 0) return; // Уже мёртв

        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth); // Не уходим в минус

        // Вызываем события
        onDamageTaken?.Invoke();
        onHealthChanged?.Invoke(currentHealth);

        // Проверяем смерть
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public void Die()
    {
        onDeath?.Invoke();
        Debug.Log($"{gameObject.name} died!");

        if (destroyOnDeath)
        {
            Destroy(gameObject, 0.1f);
        }
        else
        {
            // Не уничтожаем, только делаем неактивным (для игрока и баз)
            gameObject.SetActive(false);
        }
    }
    // Метод для лечения
    // Метод для лечения
    public void Heal(float amount)
    {
        currentHealth += amount;

        // Не позволяем здоровью превышать максимум
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        // Вызываем событие обновления UI
        onHealthChanged?.Invoke(currentHealth);

        Debug.Log($"{gameObject.name} healed for {amount} HP!");
    }

    // Вспомогательный метод для получения процента здоровья (0-1)
    public float GetHealthPercentage()
    {
        return currentHealth / maxHealth;
    }
}