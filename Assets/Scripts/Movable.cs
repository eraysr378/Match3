using System;
using UnityEngine;
using System.Collections;
using Cells;
using Pieces;
using GridRelated;

public class Movable : MonoBehaviour
{
    private Piece _piece;

    private void Awake()
    {
        _piece = GetComponent<Piece>();
    }

    public void StartMoving(Cell targetCell, float duration)
    {
        if (targetCell == null)
        {
            Debug.Log("target cell set null");
        }
        _piece.SetCell(targetCell);
        StartCoroutine(MoveToCellIE(targetCell,duration));
    }

    private IEnumerator MoveToCellIE(Cell targetCell, float duration)
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;
        Vector3 targetPosition = GridUtility.GridPositionToWorldPosition(targetCell.Row, targetCell.Col);

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }
}