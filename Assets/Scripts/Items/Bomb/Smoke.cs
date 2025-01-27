using Pool;
using UnityEngine;

public class Smoke : MonoBehaviour, IPoolable
{
    public void OnSmokeAnimationEnd()
    {
        SmokePool.Instance.Return(this);
    }

    public void Reset()
    {
    }
}