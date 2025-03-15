using System;
using UnityEngine;
using System.Collections;
using Cells;
using GridRelated;

public class Movable : MonoBehaviour
{
    private Cell _cell;

    private void Awake()
    {
        _cell = GetComponent<Cell>();
    }

    public void StartMoving(int targetRow,int targetCol, float duration)
    {
        StartCoroutine(MoveToPositionIE(targetRow,targetCol,duration));
    }

    private IEnumerator MoveToPositionIE(int targetRow,int targetCol, float duration)
    {
        _cell.SetPosition(targetRow, targetCol);
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;
        Vector3 targetPosition = GridUtility.GridPositionToWorldPosition(targetRow, targetCol, _cell);

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }
}