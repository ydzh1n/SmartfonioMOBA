using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target; // Основная цель (база игрока)

    [Header("Aggro Settings")]
    public float aggroRange = 3f; // Радиус, в котором враг замечает игрока
    private Transform playerTransform; // Ссылка на игрока

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

        // Находим игрока
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

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
        // Если игра закончилась — отключаем AI
        if (GameManager.Instance != null && !GameManager.Instance.IsGameActive)
        {
            enabled = false;
            return;
        }

        if (target == null) return;

        // Проверяем, есть ли игрок рядом (агро-радиус)
        bool playerInRange = false;
        if (playerTransform != null && playerTransform.gameObject.activeSelf)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            if (distanceToPlayer <= aggroRange)
            {
                playerInRange = true;
            }
        }

        // Выбираем цель: игрок (если рядом) или база
        Transform currentTarget = playerInRange ? playerTransform : target;

        if (currentTarget == null) return;

        float distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);

        if (distanceToTarget <= attackRange)
        {
            Attack(currentTarget);
        }
        else
        {
            MoveTowardsTarget(currentTarget);
        }
    }

    void MoveTowardsTarget(Transform currentTarget)
    {
        Vector3 direction = (currentTarget.position - transform.position).normalized;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        Vector3 movement = direction * moveSpeed * Time.deltaTime;
        rb.MovePosition(transform.position + movement);
    }

    void Attack(Transform currentTarget)
    {
        if (Time.time - lastAttackTime < attackCooldown) return;

        lastAttackTime = Time.time;

        // Наносим урон цели
        IDamageable damageable = currentTarget.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(attackDamage);
            //Debug.Log($"{gameObject.name} attacked {currentTarget.name} for {attackDamage} damage!");
        }
    }
}