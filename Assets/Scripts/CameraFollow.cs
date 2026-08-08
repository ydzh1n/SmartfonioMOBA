using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target; // Объект, за которым следует камера

    [Header("Offset Settings")]
    public Vector3 offset = new Vector3(0, 15, -10); // Смещение камеры относительно игрока

    [Header("Smooth Settings")]
    public float smoothSpeed = 5f; // Плавность следования

    [Header("Look Settings")]
    public bool lookAtTarget = true; // Камера смотрит на игрока

    void LateUpdate()
    {
        if (target != null)
        {
            // Вычисляем целевую позицию камеры
            Vector3 desiredPosition = target.position + offset;

            // Плавное перемещение камеры
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

            transform.position = smoothedPosition;

            // Камера смотрит на игрока
            if (lookAtTarget)
            {
                transform.LookAt(target);
            }
        }
    }
}