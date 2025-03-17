using Cells;
using UnityEngine;
[RequireComponent(typeof(Movable))]
public class Fillable : MonoBehaviour
{
    private Movable _movable;

    private void Awake()
    {
        _movable = GetComponent<Movable>();
    }

    public void Fill(Cell targetCell,float duration)
    {
        if (_movable == null)
        {
            Debug.LogWarning("Trying to move a Fillable cell without Movable!");
            return;
        }
        _movable.StartMoving(targetCell,duration);
    }
}