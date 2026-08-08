using UnityEngine;

public class HealingZone : MonoBehaviour
{
    [Header("Healing Settings")]
    public float healAmount = 10f;      // Сколько HP восстанавливается за тик
    public float healInterval = 1f;     // Как часто происходит лечение (в секундах)

    private float lastHealTime = 0f;
    private Transform playerTransform;

    void Start()
    {
        // Находим игрока при старте
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    // OnTriggerStay вызывается каждый кадр, пока объект находится внутри триггера
    void OnTriggerStay(Collider other)
    {
        // Проверяем, что внутри зоны находится именно игрок
        if (other.transform == playerTransform)
        {
            // Проверяем интервал лечения
            if (Time.time - lastHealTime >= healInterval)
            {
                HealthSystem health = playerTransform.GetComponent<HealthSystem>();

                // Лечим, только если здоровье не полное
                if (health != null && health.GetCurrentHealth() < health.GetMaxHealth())
                {
                    health.Heal(healAmount);
                    lastHealTime = Time.time;
                }
            }
        }
    }
}