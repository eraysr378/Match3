namespace Interfaces
{
    public interface IActivatable
    {
        public void Activate();
        public bool IsActivated { get; }
    }
}