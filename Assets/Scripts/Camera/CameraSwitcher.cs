using UnityEngine;
using Unity.Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] private CinemachineCamera targetCamera;
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SwitchCamera();
        } 
    }

    private void SwitchCamera()
    {
        CinemachineCamera[] cameras = Object.FindObjectsByType<CinemachineCamera>(FindObjectsSortMode.None);
        if (cameras != null)
        {
            foreach (var cam in cameras)
            {
                if (cam != null)
                {
                    cam.Priority = 0; // Lower priority for all cameras
                }
            }
        }

        if (targetCamera != null)
        {
            targetCamera.Priority = 10; // Higher priority for the target camera}
        }
    }
    
}