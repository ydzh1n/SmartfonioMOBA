using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target;

    [Header("Offset Settings")]
    public Vector3 offset = new Vector3(0, 20, -15);

    [Header("Smooth Settings")]
    public float smoothSpeed = 10f;

    [Header("Camera Boundaries")]
    public float cameraMinX = -32f;
    public float cameraMaxX = 32f;
    public float cameraMinZ = -32f;
    public float cameraMaxZ = 32f;

    void Start()
    {
        // Сразу устанавливаем камеру на правильную позицию
        if (target != null)
        {
            Vector3 startPosition = target.position + offset;
            startPosition.x = Mathf.Clamp(startPosition.x, cameraMinX, cameraMaxX);
            startPosition.z = Mathf.Clamp(startPosition.z, cameraMinZ, cameraMaxZ);
            transform.position = startPosition;
        }
    }

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;

            // Ограничиваем камеру границами
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, cameraMinX, cameraMaxX);
            desiredPosition.z = Mathf.Clamp(desiredPosition.z, cameraMinZ, cameraMaxZ);

            // Плавное перемещение
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        }
    }
}