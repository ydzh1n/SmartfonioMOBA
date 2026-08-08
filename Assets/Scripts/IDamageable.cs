using UnityEngine;

// Интерфейс для всех объектов, которые могут получать урон
public interface IDamageable
{
    // Нанести урон
    void TakeDamage(float damage);

    // Получить текущее здоровье
    float GetCurrentHealth();

    // Получить максимальное здоровье
    float GetMaxHealth();

    // Событие смерти
    void Die();
}