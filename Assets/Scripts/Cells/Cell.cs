using Pieces;
using UnityEngine;

namespace Cells
{
    public abstract class Cell  : MonoBehaviour
    {
        public int Row { get; private set; }
        public int Col { get; private set; }
        public Piece CurrentPiece { get; private set; }
        public void SetRow(int row) => Row = row;
        public void SetCol(int col) => Col = col;

        public void SetPosition(int row, int col)
        {
            Row = row;
            Col = col;
        }

        public void SetPiece(Piece piece)
        {
            CurrentPiece = piece;
        }

    }

}