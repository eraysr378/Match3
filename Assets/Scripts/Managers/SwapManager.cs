using System;
using System.Collections;
using Cells;
using UnityEngine;
using Grid = GridRelated.Grid;
namespace Managers
{
    public class SwapManager : MonoBehaviour
    {
        private bool _canSwap = true;
        private Grid _grid;
        private Cell _swappedFirstCell;
        private Cell _swappedSecondCell;
    
        public void Initialize(Grid grid)
        {
            _grid = grid;
        }



        public void Swap(Cell firstCell, Cell secondCell)
        {
            _swappedFirstCell = firstCell;
            _swappedSecondCell = secondCell;
            StartCoroutine(SwapIE(firstCell, secondCell,false));
        }
        public void RevertSwap()
        {
            StartCoroutine(SwapIE(_swappedFirstCell,_swappedSecondCell ,true));
            _swappedFirstCell = null;
            _swappedSecondCell = null;
        }
        private IEnumerator SwapIE(Cell firstCell, Cell secondCell,bool isReverting)
        {
            _canSwap = false;
            yield return StartCoroutine(AnimateSwapIE(firstCell, secondCell));
            _grid.SwapCells(firstCell, secondCell);
            
            int saveRow = firstCell.Row;
            int saveCol = firstCell.Col;
            firstCell.SetPosition(secondCell.Row,secondCell.Col);
            secondCell.SetPosition(saveRow,saveCol);
            
            _canSwap = true;
            if (!isReverting)
            {
                EventManager.OnSwapCompleted?.Invoke(_swappedFirstCell,_swappedSecondCell);
            }
        }

        private IEnumerator AnimateSwapIE(Cell firstCell, Cell secondCell)
        {
            float duration = 0.25f;
            Vector3 startPos1 = firstCell.transform.position;
            Vector3 startPos2 = secondCell.transform.position;
            float time = 0;

            while (time < duration)
            {
                time += Time.deltaTime;
                float t = time / duration;
                firstCell.transform.position = Vector3.Lerp(startPos1, startPos2, t);
                secondCell.transform.position = Vector3.Lerp(startPos2, startPos1, t);
                yield return null;
            }

            firstCell.transform.position = startPos2;
            secondCell.transform.position = startPos1;
        }

        public bool CanSwap()
        {
            return _canSwap;
        }
    }
    

}
