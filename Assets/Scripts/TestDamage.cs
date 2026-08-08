using UnityEngine;

public class TestDamage : MonoBehaviour
{
    public float damage = 10f;

    void Update()
    {
        // Нажми T чтобы нанести урон ближайшему IDamageable
        if (Input.GetKeyDown(KeyCode.T))
        {
            // Находим все объекты с HealthSystem
            HealthSystem[] healthSystems = FindObjectsOfType<HealthSystem>();

            foreach (HealthSystem hs in healthSystems)
            {
                hs.TakeDamage(damage);
                Debug.Log($"{hs.gameObject.name}: {hs.GetCurrentHealth()} / {hs.GetMaxHealth()}");
            }
        }
    }
}