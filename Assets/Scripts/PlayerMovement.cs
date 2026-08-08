using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed = 6f;
    public float rotationSpeed = 10f;

    private Rigidbody rb;
    private Vector3 moveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Получаем ввод с клавиатуры
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        moveDirection = new Vector3(moveX, 0, moveZ).normalized;
    }

    void FixedUpdate()
    {
        if (moveDirection.magnitude > 0)
        {
            // Поворот в направлении движения
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

            // Перемещение
            Vector3 movement = moveDirection * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(transform.position + movement);
        }
        else
        {
            // Останавливаем игрока, если нет ввода
            rb.linearVelocity = Vector3.zero;
        }
    }
}