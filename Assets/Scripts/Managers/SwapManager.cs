using System;
using System.Collections;
using Cells;
using Pieces;
using UnityEngine;
using Grid = GridRelated.Grid;

namespace Managers
{
    public class SwapManager : MonoBehaviour
    {
        private bool _canSwap = true;
        private Grid _grid;
        private Piece _swappedFirstPiece;
        private Piece _swappedSecondPiece;

        public void Initialize(Grid grid)
        {
            _grid = grid;
        }
        
        public void Swap(Piece firstPiece, Piece secondPiece)
        {
            _swappedFirstPiece = firstPiece;
            _swappedSecondPiece = secondPiece;
            StartCoroutine(SwapIE(firstPiece, secondPiece, false));
        }

        public void RevertSwap()
        {
            StartCoroutine(SwapIE(_swappedFirstPiece, _swappedSecondPiece, true));
            _swappedFirstPiece = null;
            _swappedSecondPiece = null;
        }

        private IEnumerator SwapIE(Piece firstPiece, Piece secondPiece, bool isReverting)
        {
            _canSwap = false;
            yield return StartCoroutine(AnimateSwapIE(firstPiece, secondPiece));

            Cell saveCell = firstPiece.CurrentCell;
            firstPiece.SetCell(secondPiece.CurrentCell);
            secondPiece.SetCell(saveCell);

            firstPiece.CurrentCell.SetPiece(firstPiece);
            secondPiece.CurrentCell.SetPiece(secondPiece);

            _canSwap = true;
            if (!isReverting)
            {
                EventManager.OnSwapCompleted?.Invoke(_swappedFirstPiece, _swappedSecondPiece);
            }
        }

        private IEnumerator AnimateSwapIE(Piece firstPiece, Piece secondPiece)
        {
            float duration = 0.25f;
            Vector3 startPos1 = firstPiece.transform.position;
            Vector3 startPos2 = secondPiece.transform.position;
            float time = 0;

            while (time < duration)
            {
                time += Time.deltaTime;
                float t = time / duration;
                firstPiece.transform.position = Vector3.Lerp(startPos1, startPos2, t);
                secondPiece.transform.position = Vector3.Lerp(startPos2, startPos1, t);
                yield return null;
            }

            firstPiece.transform.position = startPos2;
            secondPiece.transform.position = startPos1;
        }

        public bool CanSwap()
        {
            return _canSwap;
        }
    }
}