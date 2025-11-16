using UnityEngine;

public class PortalPerspectiveManager : MonoBehaviour
{
    // Arraste a câmera do seu Player para aqui
    public Camera playerCamera;

    // Arraste as 3 Câmeras dos Portais para aqui
    public Camera cameraPortalA;
    public Camera cameraPortalB;
    public Camera cameraPortalC;

    // Arraste os 3 Sensores dos Portais para aqui
    public Transform sensorPortalA;
    public Transform sensorPortalB;
    public Transform sensorPortalC;

    // Roda depois de tudo
    void LateUpdate()
    {
        // 1. Atualiza a Câmera A (Verde)
        // (Ela precisa de saber a posição do player relativa ao portal B e C)
        UpdatePortalCamera(cameraPortalA, sensorPortalB, sensorPortalA); // Mostra no B
        UpdatePortalCamera(cameraPortalA, sensorPortalC, sensorPortalA); // Mostra no C

        // 2. Atualiza a Câmera B (Laranja)
        // (Ela precisa de saber a posição do player relativa ao portal A e C)
        UpdatePortalCamera(cameraPortalB, sensorPortalA, sensorPortalB); // Mostra no A
        UpdatePortalCamera(cameraPortalB, sensorPortalC, sensorPortalB); // Mostra no C

        // 3. Atualiza a Câmera C (Azul)
        // (Ela precisa de saber a posição do player relativa ao portal A e B)
        UpdatePortalCamera(cameraPortalC, sensorPortalA, sensorPortalC); // Mostra no A
        UpdatePortalCamera(cameraPortalC, sensorPortalB, sensorPortalC); // Mostra no B
    }

    // A função mágica que calcula a perspetiva
    void UpdatePortalCamera(Camera portalCamera, Transform entrancePortal, Transform exitPortal)
    {
        if (playerCamera == null || portalCamera == null || entrancePortal == null || exitPortal == null) return;

        // --- Posição ---
        Vector3 playerOffset = entrancePortal.InverseTransformPoint(playerCamera.transform.position);
        playerOffset = new Vector3(-playerOffset.x, playerOffset.y, -playerOffset.z);
        portalCamera.transform.position = exitPortal.TransformPoint(playerOffset);

        // --- Rotação ---
        Quaternion rotationDifference = exitPortal.rotation * Quaternion.Inverse(entrancePortal.rotation) * Quaternion.Euler(0, 180, 0);
        portalCamera.transform.rotation = rotationDifference * playerCamera.transform.rotation;
    }
}