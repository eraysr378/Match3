namespace Interfaces
{
    public interface IPoolableObject
    {
        public void OnSpawn();
        public void OnReturnToPool();
    }
}