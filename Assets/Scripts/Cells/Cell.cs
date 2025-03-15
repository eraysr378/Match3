using System;
using System.Collections;
using GridRelated;
using Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cells
{
    public abstract class Cell : MonoBehaviour,IPointerDownHandler, IPointerUpHandler,IPointerEnterHandler,IPointerClickHandler
    {
        [SerializeField] protected SpriteRenderer visual;
        public Collider2D Collider => _collider;

        protected CellType cellType;
        public int Row => _row;
        public int Col => _col;
        private int _col;
        private int _row;
        private Collider2D _collider;
        private Coroutine _moveCoroutine;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

        public virtual void Init(int row, int col,Vector3 position,float elementSize,Transform parent)
        {
            _row = row;
            _col = col;
            transform.position = position;
            transform.localScale = Vector3.one * elementSize;
            transform.SetParent(parent);
        }

        public void SetPosition(int row, int col)
        {
            _row = row;
            _col = col;
        }
        public void SetRow(int row) => _row = row;
        public void SetCol(int col)  => _col = col;
        public void SetCellType(CellType cellType) => this.cellType = cellType;
        public CellType GetCellType() => cellType;
        // public void UpdatePosition(int newRow, int newCol, float duration = 2f)
        // {
        //     if(_moveCoroutine != null)
        //         StopCoroutine(_moveCoroutine);
        //     _moveCoroutine = StartCoroutine(MoveToPosition(newRow,newCol, duration));
        // }
        // private IEnumerator MoveToPosition(int targetRow,int targetCol, float duration)
        // {
        //     _row = targetRow;
        //     _col = targetCol;
        //     Vector3 startPosition = transform.position;
        //     Vector3 targetPosition = GridUtility.GridPositionToWorldPosition(targetRow,targetCol,this);
        //     float elapsedTime = 0f;
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
        public void DestroyCell()
        {
            visual.color = Color.red;
            EventManager.OnCellDestroyed?.Invoke(this);

            StartCoroutine(DestroyAfter(0.15f));
        }

        private IEnumerator DestroyAfter(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            Destroy(gameObject);
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            EventManager.OnPointerDownCell?.Invoke(this);
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            EventManager.OnPointerUpCell?.Invoke(this);
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            EventManager.OnPointerEnterCell?.Invoke(this);
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            EventManager.OnPointerClickedCell?.Invoke(this);

        }
    }
}
