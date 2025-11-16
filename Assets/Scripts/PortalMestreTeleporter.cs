using UnityEngine;

public class PortalMestreTeleporter : MonoBehaviour
{
    // CADA portal terá dois destinos:
    public Transform destination_LadoFrente;
    public Transform destination_LadoTras;
    
    // NOVO: Limites para a altura da câmara
    public float minCameraHeight = 0.2f;
    public float maxCameraHeight = 1.8f; // Assumindo 1.8 como a altura normal

    
    private bool isPlayerOverlapping = false;
    private Transform playerTransform;
    private float previousZPosition;

    void Update()
    {
        if (isPlayerOverlapping && playerTransform != null)
        {
            float currentZPosition = transform.InverseTransformPoint(playerTransform.position).z;

            // Se o jogador atravessar da FRENTE (Z > 0) para TRÁS (Z < 0)
            if (previousZPosition > 0 && currentZPosition < 0)
            {
                // Teleporta e avisa que é para "encolher" (true)
                TeleportPlayer(destination_LadoFrente, true); 
            }
            // Se o jogador atravessar de TRÁS (Z < 0) para FRENTE (Z > 0)
            else if (previousZPosition < 0 && currentZPosition > 0)
            {
                // Teleporta e avisa que é para "crescer" (false)
                TeleportPlayer(destination_LadoTras, false);
            }
            
            previousZPosition = currentZPosition;
        }
    }

    // A função TeleportPlayer agora aceita um booleano "isShrinking"
    private void TeleportPlayer(Transform destinationPortal, bool isShrinking)
    {
        // Se o destino não for válido (não estiver ligado), não faz nada.
        if (destinationPortal == null) return;

        Transform player = playerTransform;

        Vector3 localPos = transform.InverseTransformPoint(player.position);
        localPos = new Vector3(-localPos.x, localPos.y, -localPos.z);
        Vector3 newGlobalPos = destinationPortal.TransformPoint(localPos);
        
        Quaternion rotationDifference = destinationPortal.rotation * Quaternion.Inverse(transform.rotation) * Quaternion.Euler(0, 180, 0);
        Quaternion newRotation = rotationDifference * player.rotation;
        
        CharacterController controller = player.GetComponent<CharacterController>();
        controller.enabled = false;
        
        player.position = newGlobalPos;
        player.rotation = newRotation;
        
        controller.enabled = true;

        // --- INÍCIO DA NOVA LÓGICA DE TAMANHO ---
        // Acha a câmara que é "filha" do player
        Camera playerCam = player.GetComponentInChildren<Camera>();
        if (playerCam != null)
        {
            float currentHeight = playerCam.transform.localPosition.y;
            float newHeight;

            if (isShrinking)
            {
                // Diminui a altura pela metade
                newHeight = currentHeight / 2f;
                // Aplica o limite MÍNIMO
                if (newHeight < minCameraHeight)
                {
                    newHeight = minCameraHeight;
                }
            }
            else // está a crescer
            {
                // Dobra a altura
                newHeight = currentHeight * 2f;
                // Aplica o limite MÁXIMO  
                if (newHeight > maxCameraHeight)
                {
                    newHeight = maxCameraHeight;
                }
            }
            
            // Altera a POSIÇÃO Y LOCAL da câmara
            Vector3 camLocalPos = playerCam.transform.localPosition;
            camLocalPos.y = newHeight;
            playerCam.transform.localPosition = camLocalPos;
        }
        // --- FIM DA NOVA LÓGICA DE TAMANHO ---
        
        // Desarma este portal para evitar loops
        isPlayerOverlapping = false;

        // Pega o script do portal de destino e "reinicia" o estado dele
        PortalMestreTeleporter destinationScript = destinationPortal.GetComponent<PortalMestreTeleporter>();
        if (destinationScript != null)
        {
            destinationScript.ResetPortalState(destinationPortal.InverseTransformPoint(player.position).z);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        CharacterController controller = other.GetComponent<CharacterController>();
        if (controller != null)
        {
            playerTransform = other.transform;
            previousZPosition = transform.InverseTransformPoint(playerTransform.position).z;
            isPlayerOverlapping = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        CharacterController controller = other.GetComponent<CharacterController>();
        if (controller != null)
        {
            isPlayerOverlapping = false;
            playerTransform = null;
        }
    }

    // Método para o portal de destino "resetar" seu estado quando o jogador chega
    public void ResetPortalState(float playerLocalZ)
    {
        isPlayerOverlapping = true; 
        previousZPosition = playerLocalZ;
    }
}