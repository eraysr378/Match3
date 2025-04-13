using GridRelated;
using MatchSystem;
using Pieces;
using SwapSystem;
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
            FillManager.OnAllFillsCompleted += OnFillCompleted;
            SwapValidator.OnSwapWillCauseMatch += WouldSwapCauseMatch;
            EventManager.OnMatchCheckRequested += OnMatchCheckRequested;
        }
        
        public void OnDisable()
        {
            GridInitializer.OnGridInitialized -= Initialize;
            FillManager.OnAllFillsCompleted -= OnFillCompleted;
            SwapValidator.OnSwapWillCauseMatch -= WouldSwapCauseMatch;
            EventManager.OnMatchCheckRequested -= OnMatchCheckRequested;
        }

        private void Initialize(Grid grid)
        {
            _grid = grid;
            _matchFinder = new MatchFinder(grid);
            _matchHandler = new MatchHandler();
            _swapFinder = new SwapFinder(grid, _matchFinder);
            
        }
        private void OnMatchCheckRequested(Piece piece)
        {
            HandleMatches(piece);
        }

        private void OnFillCompleted()
        {
            if (!_matchHandler.IsHandlerActive)
            {
                FindAndHandleAllMatches();
            }
        }
        private bool WouldSwapCauseMatch(Piece a, Piece b)
        {
            return _matchFinder.WouldSwapCauseMatch(a, b);
        }
        public bool TryGetMatchFormingSwap(out (Piece,Piece)? pieces)
        {
            _swapFinder.TryFindValidSwapForMatch(out pieces);
            return pieces != null;
        }
        

        private void HandleMatches(params Piece[] pieces)
        {
            var matches = _matchFinder.FindMatches(pieces);

            if (matches.Count == 0)
                return ;

            _matchHandler.HandleMatches(matches);
        }

        private void FindAndHandleAllMatches()
        {
            var allPieces = GridManager.Instance.GetAllPieces();
            var matches = _matchFinder.FindMatches(allPieces);
            if (matches.Count > 0)
            {
                _matchHandler.HandleMatches(matches);
            }
            else
            {
                // Debug.Log("No matches found");
            }
   
        }
    }
}