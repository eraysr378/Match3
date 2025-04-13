namespace Interfaces
{
    public interface ISwapCommand
    {
        void Execute();
        void Undo();
    }
}