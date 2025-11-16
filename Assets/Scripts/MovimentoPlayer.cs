using UnityEngine;

// Esta linha garante que o script SÓ funcione
// se o objeto tiver um CharacterController
[RequireComponent(typeof(CharacterController))]
public class MovimentoPlayer : MonoBehaviour
{
    // --- Variáveis de Movimento ---
    public float velocidade = 8.0f;          // Velocidade de andar
    public float gravidade = -20.0f;       // Força da gravidade
    
    // --- Variáveis da Câmera (Mouse) ---
    public float sensibilidadeMouse = 2.0f;
    private Camera cam;
    private float rotacaoVertical = 0.0f;

    // --- Componentes ---
    private CharacterController controller;
    private Vector3 velocidadeVertical; // Guarda a velocidade da gravidade/pulo

    
    void Start()
    {
        // Pega os componentes que estão no mesmo objeto do script
        controller = GetComponent<CharacterController>();
        
        // Pega a câmera que está "filha" deste objeto
        cam = GetComponentInChildren<Camera>();

        // Trava o cursor do mouse no centro da tela e o esconde
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    
    void Update()
    {
        // --- ROTAÇÃO (MOUSE) ---

        // Pega a movimentação do mouse
        float mouseX = Input.GetAxis("Mouse X") * sensibilidadeMouse;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidadeMouse;

        // Gira o JOGADOR INTEIRO para os lados (esquerda/direita)
        transform.Rotate(0, mouseX, 0);

        // Gira apenas a CÂMERA para cima/baixo
        rotacaoVertical -= mouseY;
        rotacaoVertical = Mathf.Clamp(rotacaoVertical, -90f, 90f); // Limita para não dar "looping"
        cam.transform.localRotation = Quaternion.Euler(rotacaoVertical, 0, 0);


        // --- MOVIMENTO (TECLADO) ---

        // Pega o input do teclado (W/S = "Vertical", A/D = "Horizontal")
        float moveFrente = Input.GetAxis("Vertical");
        float moveLado = Input.GetAxis("Horizontal");

        // Calcula o vetor de direção baseado na rotação do jogador
        Vector3 direcao = new Vector3(moveLado, 0, moveFrente);
        direcao = transform.TransformDirection(direcao); // Converte de local para global

        // Aplica a velocidade ao movimento
        Vector3 movimento = direcao * velocidade;

        
        // --- GRAVIDADE ---

        // Se o jogador está no chão, reseta a velocidade vertical
        if (controller.isGrounded)
        {
            velocidadeVertical.y = -2.0f; // Uma pequena força para baixo
        }
        else
        {
            // Se está no ar, aplica a gravidade
            velocidadeVertical.y += gravidade * Time.deltaTime;
        }

        // Aplica o movimento (W,A,S,D) e a gravidade
        controller.Move((movimento + velocidadeVertical) * Time.deltaTime);
    }
}