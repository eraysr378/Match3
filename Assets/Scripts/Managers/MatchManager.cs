using GridRelated;
using MatchSystem;
using Pieces;
using UnityEngine;
using Grid = GridRelated.Grid;

namespace Managers
{
    public class MatchManager : MonoBehaviour
    {
        private Grid _grid;
        private MatchFinder _matchFinder;
        private MatchHandler _matchHandler;
        private SwapFinder _swapFinder;

      

        public void OnEnable()
        {
            GridInitializer.OnGridInitialized += Initialize;
            FillManager.OnFillCompleted += OnFillCompleted;
        }

        public void OnDisable()
        {
            GridInitializer.OnGridInitialized -= Initialize;
            FillManager.OnFillCompleted -= OnFillCompleted;
        }
        private void Initialize(Grid grid)
        {
            _grid = grid;
            _matchFinder = new MatchFinder(grid);
            _matchHandler = new MatchHandler();
            _swapFinder = new SwapFinder(grid, _matchFinder);
        }

        private void OnFillCompleted()
        {
            FindAndHandleAllMatches();
        }

        public bool TryGetMatchFormingSwap(out (Piece,Piece)? pieces)
        {
            _swapFinder.TryFindValidSwapForMatch(out pieces);
            return pieces != null;
        }
        public bool TryHandleMatches(params Piece[] pieces)
        {
            var matches = _matchFinder.FindMatches(pieces);

            if (matches.Count == 0)
                return false;

            _matchHandler.HandleMatches(matches);
            return true;
        }

        private void FindAndHandleAllMatches()
        {
            var allPieces = _grid.GetAllPieces();
            var matches = _matchFinder.FindMatches(allPieces);

            if (matches.Count > 0)
            {
                _matchHandler.HandleMatches(matches);
            }
        }
    }
}