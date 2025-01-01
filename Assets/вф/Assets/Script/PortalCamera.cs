using UnityEngine;

public class PortalCamera : MonoBehaviour
{
    public Transform playerCam;     // Камера игрока
    public Transform portal;        // Текущий портал
    public Transform otherPortal;   // Другой портал
    public bool useNegativeY;       // Использовать инверсию по Y

    void LateUpdate()
    {
        // Смещение позиции камеры игрока относительно другого портала
        Vector3 playerOffsetFromOtherPortal = playerCam.position - otherPortal.position;

        if (!useNegativeY)
        {
            // Обычная трансформация позиции
            transform.position = portal.position + playerOffsetFromOtherPortal;
        }
        else
        {
            // Инверсия по оси Y
            transform.position = new Vector3(
                portal.position.x,
                -portal.position.y,
                portal.position.z)
                - new Vector3(
                    playerOffsetFromOtherPortal.x,
                    -playerOffsetFromOtherPortal.y,
                    playerOffsetFromOtherPortal.z
                );
        }

        // Разница в угле между порталом и другим порталом
        Quaternion rotationDiff = Quaternion.Inverse(otherPortal.rotation) * portal.rotation;

        // Обновляем направление камеры
        Vector3 newCamDirection = rotationDiff * playerCam.forward;

        // Устанавливаем новую ориентацию камеры
        transform.rotation = Quaternion.LookRotation(newCamDirection, Vector3.up);
    }
}
