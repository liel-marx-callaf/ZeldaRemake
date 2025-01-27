using Pool;
using UnityEngine;

public class BombPool : MonoPool<Bomb>
{
    // Inherits everything from MonoPool<Bomb>.
    // Make sure you attach this to a GameObject in your scene 
    // and assign the prefab & initial size in the Inspector.
}