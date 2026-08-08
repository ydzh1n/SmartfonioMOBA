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
        // Проверяем кулдаун
        if (Time.time - lastAttackTime < attackCooldown) return;

        lastAttackTime = Time.time;

        // Ищем всех врагов в радиусе атаки
        Collider[] hitColliders = Physics.OverlapSphere(playerTransform.position, attackRange);

        foreach (Collider hit in hitColliders)
        {
            // Проверяем, есть ли у объекта HealthSystem и это не игрок
            HealthSystem health = hit.GetComponent<HealthSystem>();
            if (health != null && hit.gameObject != gameObject)
            {
                health.TakeDamage(attackDamage);
                Debug.Log($"Player attacked {hit.gameObject.name} for {attackDamage} damage!");
            }
        }

        // Визуальный эффект атаки
        Debug.Log("Player attacked!");
    }

    // Визуализация радиуса атаки в редакторе
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}