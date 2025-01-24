using Pool;

namespace Interfaces
{
    public interface IPickupable : IPoolable
    {
        public void Despawn();
        public void Pickup();
        public void SetDespawnTime(float time);
    }
}

