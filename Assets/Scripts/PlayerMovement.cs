using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed = 6f;
    public float rotationSpeed = 10f;
    public float LastMoveTime { get; private set; } = 0f; // Время последнего движения

    [Header("Boundaries")]
    public float minX = -50f;
    public float maxX = 50f;
    public float minZ = -50f;
    public float maxZ = 50f;

    private Rigidbody rb;
    private Vector3 moveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        moveDirection = new Vector3(moveX, 0, moveZ).normalized;
    }

    void FixedUpdate()
    {
        if (moveDirection.magnitude > 0)
        {
            LastMoveTime = Time.time;
            // Поворот
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

            // Движение
            Vector3 movement = moveDirection * moveSpeed * Time.fixedDeltaTime;
            Vector3 newPosition = transform.position + movement;

            // Ограничиваем позицию границами карты
            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
            newPosition.z = Mathf.Clamp(newPosition.z, minZ, maxZ);

            rb.MovePosition(newPosition);
        }
        else
        {
            // Остановка
            rb.linearVelocity = Vector3.zero;
        }
    }
}