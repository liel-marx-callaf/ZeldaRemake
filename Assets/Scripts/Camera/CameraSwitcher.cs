using System;
using System.Collections;
using UnityEngine;
using Unity.Cinemachine;
// todo: fix pixel perfect camera size and colliders
public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] private CinemachineCamera currentCamera;
    [SerializeField] private CinemachineCamera targetCamera;
    [SerializeField] private int currentIndex;
    [SerializeField] private int targetIndex;
    
    private float _transitionDuration;
    private bool _areaSwitching = false;

    private void Start()
    {
        _transitionDuration = Camera.main.GetComponent<CinemachineBrain>().DefaultBlend.Time;
    }

    private void OnEnable()
    {
        MyEvents.AreaSwitch += OnAreaSwitch;
    }
    
    private void OnDisable()
    {
        MyEvents.AreaSwitch -= OnAreaSwitch;
    }



    private IEnumerator IgnoreTriggerTemporarily()
    {
        _areaSwitching = true;
        yield return new WaitForSeconds(_transitionDuration);
        _areaSwitching = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_areaSwitching) return;
        if (other.CompareTag("Player"))
        {
            SwitchCamera();
        } 
    }

    private void SwitchCamera()
    {
        if(currentCamera.Priority == 0) return;
        currentCamera.Priority = 0; // Lower priority for the current camera
        if (targetCamera != null)
        {
            targetCamera.Priority = 10; // Higher priority for the target camera
        }
        
        MyEvents.AreaSwitch?.Invoke(targetIndex, currentIndex);
    }
        private void OnAreaSwitch(int enterIndex, int exitIndex)
        {
            if (enterIndex == currentIndex)
            {
                StartCoroutine(IgnoreTriggerTemporarily());
    
            }
        }
}