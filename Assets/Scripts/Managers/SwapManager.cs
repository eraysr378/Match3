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
        public bool CanSwap => _canSwap;
        private bool _canSwap = true;
        private Piece _swappedFirstPiece;
        private Piece _swappedSecondPiece;
        
        
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
            yield return AnimateSwapIE(firstPiece, secondPiece);
            
            Cell firstCell = firstPiece.CurrentCell;
            Cell secondCell = secondPiece.CurrentCell;
            secondPiece.SetCell(null);
            firstPiece.SetCell(secondCell);
            secondPiece.SetCell(firstCell);
            
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
        
    }
}