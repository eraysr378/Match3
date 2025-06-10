using BuildSystem;
using MatchSystem;
using Pieces;
using SwapSystem;
using TileRelated;
using UnityEngine;
using Grid = GridRelated.Grid;

namespace Managers
{
    public class MatchManager : MonoBehaviour
    {
        [SerializeField]private GridStabilizationChecker gridStabilizationChecker;

        private MatchFinder _matchFinder;
        private MatchHandler _matchHandler;
        private SwapFinder _swapFinder;
        public void OnEnable()
        {
            GridBuilder.OnCellsCreated += Initialize;
            SwapValidator.OnSwapWillCauseMatch += WouldSwapCauseMatch;
            EventManager.OnMatchCheckRequested += OnMatchCheckRequested;
            gridStabilizationChecker.OnRowStabilized += HandleRowStabilized;

        }
        
        public void OnDisable()
        {
            GridBuilder.OnCellsCreated -= Initialize;
            SwapValidator.OnSwapWillCauseMatch -= WouldSwapCauseMatch;
            EventManager.OnMatchCheckRequested -= OnMatchCheckRequested;
            gridStabilizationChecker.OnRowStabilized -= HandleRowStabilized;
        }
    
        private void Initialize(Grid grid)
        {
            _matchFinder = new MatchFinder(grid);
            _matchHandler = new MatchHandler();
            _swapFinder = new SwapFinder(grid, _matchFinder);
        }
        private void OnMatchCheckRequested(Piece piece)
        {
            HandleMatches(piece);
        }
        private void HandleRowStabilized(int row)
        {
            var pieces = GridManager.Instance.GetPiecesInRow(row).ToArray();
            HandleMatches(pieces);
        }
        private bool WouldSwapCauseMatch(Piece a, Piece b)
        {
            return _matchFinder.WouldSwapCauseMatch(a, b);
        }
        // public bool TryGetMatchFormingSwap(out (Piece,Piece)? pieces)
        // {
        //     _swapFinder.TryFindValidSwapForMatch(out pieces);
        //     return pieces != null;
        // }
        //

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
            
            var matches = _matchFinder.FindMatches(allPieces.ToArray());
            if (matches.Count > 0)
            {
                _matchHandler.HandleMatches(matches);
            }
            else
            {
                // Debug.Log("No matches found");
            }
   
        }

        public bool IsBusy()
        {
            return  _matchHandler.IsBusy();
        }
    }
}