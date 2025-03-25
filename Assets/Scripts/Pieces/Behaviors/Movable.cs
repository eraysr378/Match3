using System;
using UnityEngine;
using System.Collections;
using Cells;
using Pieces;
using GridRelated;

public class Movable : MonoBehaviour
{
    [SerializeField] private bool isMoving;
    public event Action OnTargetReached;
    private Coroutine _moveCoroutine;
    
    public void StartMoving(Vector3 targetPosition, float duration)
    {
        if (_moveCoroutine != null)
        {
            StopCoroutine(_moveCoroutine);
        }
        _moveCoroutine = StartCoroutine(MoveToPositionIE(targetPosition,duration));
    }
    private IEnumerator MoveToPositionIE(Vector3 targetPosition, float duration)
    {
        isMoving = true;
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        OnTargetReached?.Invoke();
        isMoving = false;

    }
    // public void StartMoving(Cell targetCell, float duration)
    // {
    //     if (targetCell == null)
    //     {
    //         Debug.Log("target cell set null");
    //     }
    //     _piece.SetCell(targetCell);
    //     StartCoroutine(MoveToCellIE(targetCell,duration));
    // }
    //
    // private IEnumerator MoveToCellIE(Cell targetCell, float duration)
    // {
    //     Vector3 startPosition = transform.position;
    //     float elapsedTime = 0f;
    //     Vector3 targetPosition = GridUtility.GridPositionToWorldPosition(targetCell.Row, targetCell.Col);
    //
    //     while (elapsedTime < duration)
    //     {
    //         transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
    //         elapsedTime += Time.deltaTime;
    //         yield return null;
    //     }
    //
    //     transform.position = targetPosition;
    // }

}