using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackRange = 2f;
    public float attackDamage = 25f;
    public float attackCooldown = 0.5f;

    [Header("Input")]
    public KeyCode attackKey = KeyCode.Space;

    private float lastAttackTime = 0f;
    private Transform playerTransform;

    void Start()
    {
        playerTransform = transform;
    }

    void Update()
    {
        // Атака по нажатию пробела
        if (Input.GetKeyDown(attackKey))
        {
            Attack();
        }
    }

    void Attack()
    {
        if (Time.time - lastAttackTime < attackCooldown) return;

        lastAttackTime = Time.time;

        Collider[] hitColliders = Physics.OverlapSphere(playerTransform.position, attackRange);

        foreach (Collider hit in hitColliders)
        {
            HealthSystem health = hit.GetComponent<HealthSystem>();
            if (health != null && hit.gameObject != gameObject)
            {
                float damage = attackDamage;
                health.TakeDamage(damage);

                // Добавляем урон в статистику
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.AddDamageDealt(damage);

                    // Проверяем, умер ли враг
                    if (health.GetCurrentHealth() <= 0)
                    {
                        GameManager.Instance.AddKill();
                    }
                }

                Debug.Log($"Player attacked {hit.gameObject.name} for {damage} damage!");
            }
        }
    }

    // Визуализация радиуса атаки в редакторе
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}