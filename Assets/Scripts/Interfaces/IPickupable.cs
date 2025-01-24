using Pool;

namespace Interfaces
{
    public interface IPickupable
    {
        public void Despawn();
        public void Pickup();
        public void SetDespawnTime(float time);
    }
}

