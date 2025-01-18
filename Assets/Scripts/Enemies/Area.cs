using Unity.Cinemachine;
using UnityEngine;


[CreateAssetMenu(fileName = "NewArea", menuName = "Game/Area")]
public class Area : ScriptableObject
{
    public string areaName;
    public CinemachineCamera associatedCamera;
    public Collider2D cameraEdgeCollider;
    public EnemyType[] enemyTypes;
    
    
}