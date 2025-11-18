using UnityEngine;
// Precisamos destas duas novas linhas para aceder ao estado do dispositivo VR:
using System.Collections.Generic;
using UnityEngine.XR;

public class VRSwitcher : MonoBehaviour
{
    [Header("Objetos do Player")]
    public GameObject playerPC;
    public GameObject playerVR;

    void Awake()
    {
        // Garante que ambos começam desativados
        if (playerPC != null) playerPC.SetActive(true);
        if (playerVR != null) playerVR.SetActive(false);

        // --- LÓGICA DE DETEÇÃO NOVA E MELHORADA ---
        // Vamos verificar se existe um dispositivo HMD (Head-Mounted Display) ativo
        var xrDevices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeadMounted, xrDevices);
        
        if (xrDevices.Count > 0)
        {
            // VR ESTÁ ATIVO (Encontrou um óculos)
            Debug.Log("VR HMD detectado. Ativando Player_VR.");
            if (playerVR != null) playerVR.SetActive(true);
        }
        else
        {
            // VR NÃO ESTÁ ATIVO (Nenhum óculos encontrado)
            Debug.Log("Nenhum VR HMD detectado. Ativando Player_PC.");
            if (playerPC != null) playerPC.SetActive(true);
        }
    }
}