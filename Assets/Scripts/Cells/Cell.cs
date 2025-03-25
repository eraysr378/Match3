using System;
using Pieces;
using UnityEngine;

namespace Cells
{
    public abstract class Cell  : MonoBehaviour
    {
        public int Row { get; private set; }
        public int Col { get; private set; }
        public Piece CurrentPiece { get; private set; }

        [SerializeField] private SpriteRenderer spriteRenderer;

        public void SetPosition(int row, int col)
        {
            Row = row;
            Col = col;
        }

        public void SetPiece(Piece piece)
        {
            CurrentPiece = piece;
        }

        // private void Update()
        // {
        //     if (CurrentPiece == null)
        //     {
        //         spriteRenderer.color = new Color(1, 1, 1, 0.8f); 
        //     }
        //     else
        //     {
        //         spriteRenderer.color = new Color(1, 1, 1, 0.2f); 
        //     }
        // }
    }

}