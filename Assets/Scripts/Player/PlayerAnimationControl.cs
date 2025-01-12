using UnityEngine;

public class PlayerAnimationControl : MonoBehaviour
{
    private Animator _animator;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    
    public void SetAnimatorSpeed(float speed)
    {
        _animator.speed = speed;
    }
}
