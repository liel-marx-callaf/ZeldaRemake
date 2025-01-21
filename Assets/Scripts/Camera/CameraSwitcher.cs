using UnityEngine;
using Unity.Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] private CinemachineCamera currentCamera;
    [SerializeField] private CinemachineCamera targetCamera;
    // [SerializeField] private CinemachineCamera currentCamera;
    [SerializeField] private int currentIndex;
    [SerializeField] private int targetIndex;
    // [SerializeField] private int currentIndex;
    // [SerializeField] private CinemachineCamera[] cameras;
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SwitchCamera();
        } 
    }

    private void SwitchCamera()
    {
        // CinemachineCamera[] cameras = FindObjectsByType<CinemachineCamera>(FindObjectsSortMode.None);
        // if (cameras != null)
        // {
        //     foreach (var cam in cameras)
        //     {
        //         if (cam != null)
        //         {
        //             cam.Priority = 0; // Lower priority for all cameras
        //         }
        //     }
        // }
        currentCamera.Priority = 0; // Lower priority for the current camera

        if (targetCamera != null)
        {
            targetCamera.Priority = 10; // Higher priority for the target camera}
        }
        
        MyEvents.AreaSwitch?.Invoke(targetIndex, currentIndex);
    }
    
}