using System;
using UnityEngine;

public class EnemyAnimatorController : MonoBehaviour
{ 
    private BoxCollider2D _boxCollider2D;

    private void OnEnable()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }
    
    public void ChangeBoxColliderHeight(float newHeight)
    {
        if(_boxCollider2D == null) return;
        _boxCollider2D.size = new Vector2(_boxCollider2D.size.x, newHeight);
    }
    
    public void ChangeBoxColliderYOffset(float yOffset)
    {
        if(_boxCollider2D == null) return;
        _boxCollider2D.offset = new Vector2(_boxCollider2D.offset.x, yOffset);
    }
    
    public void ChangeBoxColliderWidth(float newWidth)
    {
        if(_boxCollider2D == null) return;
        _boxCollider2D.size = new Vector2(newWidth, _boxCollider2D.size.y);
        // _boxCollider2D.offset = new Vector2(xOffset, _boxCollider2D.offset.y);
    }
    
    public void ChangeBoxColliderXOffset(float xOffset)
    {  
        if(_boxCollider2D == null) return;
        _boxCollider2D.offset = new Vector2(xOffset, _boxCollider2D.offset.y);
    }
}
