using UnityEngine;
using Unity.Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] private CinemachineCamera targetCamera;
    [SerializeField] private SpriteRenderer targetSprite; // The sprite to match the camera size to
    [SerializeField] private float cameraAspect = 1.77f; // Example aspect ratio (16:9)

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
            targetCamera.Priority = 10; // Higher priority for the target camera
            SetCameraSizeToSprite(targetCamera, targetSprite); // Set the camera size to match the sprite
        }

        if (Camera.main != null)
        {
            Camera.main.aspect = cameraAspect; // Set the camera aspect ratio
        }
    }

    private void SetCameraSizeToSprite(CinemachineCamera targetCam, SpriteRenderer sprite)
    {
        if (sprite != null)
        {
            float spriteHeight = sprite.bounds.size.y;
            float orthographicSize = spriteHeight / 2f;

            targetCam.Lens.OrthographicSize = orthographicSize;
        }
    }
}