using Interfaces;
using Pieces;
using Utils;

namespace SwapSystem
{
    public class SwapCommand : ISwapCommand
    {
        private readonly Piece _first;
        private readonly Piece _second;
        private readonly PositionSwapper _swapper;

        public SwapCommand(Piece first, Piece second, PositionSwapper swapper)
        {
            _first = first;
            _second = second;
            _swapper = swapper;
        }

        public void Execute()
        {
            _swapper.SwapPositions(_first.gameObject, _second.gameObject);
        }

        public void Undo()
        {
            _swapper.SwapPositions(_first.gameObject, _second.gameObject);
        }
    }
}