using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target; // Цель (база игрока или игрок)

    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float rotationSpeed = 5f;

    [Header("Attack Settings")]
    public float attackRange = 1.5f;
    public float attackDamage = 10f;
    public float attackCooldown = 1f;

    private float lastAttackTime = 0f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Если цель не назначена, ищем базу игрока
        if (target == null)
        {
            GameObject playerBase = GameObject.Find("PlayerBase");
            if (playerBase != null)
            {
                target = playerBase.transform;
            }
        }
    }

    void Update()
    {
        if (target == null) return;

        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        // Если в радиусе атаки — атакуем
        if (distanceToTarget <= attackRange)
        {
            Attack();
        }
        else
        {
            // Иначе — двигаемся к цели
            MoveTowardsTarget();
        }
    }

    void MoveTowardsTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;

        // Поворот к цели
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Движение
        Vector3 movement = direction * moveSpeed * Time.deltaTime;
        rb.MovePosition(transform.position + movement);
    }

    void Attack()
    {
        // Проверяем кулдаун атаки
        if (Time.time - lastAttackTime < attackCooldown) return;

        lastAttackTime = Time.time;

        // Наносим урон цели
        IDamageable damageable = target.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(attackDamage);
            Debug.Log($"{gameObject.name} attacked {target.name} for {attackDamage} damage!");
        }
    }
}