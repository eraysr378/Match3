using System;
using System.Collections;
using Cells;
using Interfaces;
using Managers;
using Misc;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Pieces
{
    public abstract class Piece : MonoBehaviour, IPoolableObject, IPointerDownHandler, IPointerUpHandler,
        IPointerEnterHandler
    {
        public static event Action<Piece> OnAnyPointerDown;
        public static event Action<Piece> OnAnyPointerUp;
        public static event Action<Piece> OnAnyPointerEnter;
        [SerializeField] protected SpriteRenderer visual;
        public Cell CurrentCell { get; private set; }
        public int Row => CurrentCell.Row;
        public int Col => CurrentCell.Col;

        protected PieceType pieceType;

        protected Collider2D _collider2D;
        private Color _baseColor;

        protected virtual void Awake()
        {
            _collider2D = GetComponent<Collider2D>();
            _baseColor = visual.color;

        }


        public virtual void Init(Vector3 position)
        {
            transform.position = position;
            transform.localScale = Vector3.one;
        }

        public virtual void Init(Cell cell)
        {
            SetCell(cell);
            transform.SetParent(cell?.transform);
            transform.localScale = Vector3.one;
        }

        public void SetCell(Cell newCell)
        {
            CurrentCell?.SetPiece(null);
            CurrentCell = newCell;
            if (CurrentCell != null)
            {
                transform.SetParent(CurrentCell.transform);
                transform.localScale = Vector3.one;
                CurrentCell.SetPiece(this);
            }
        }

        public void SetPieceType(PieceType pieceType) => this.pieceType = pieceType;
        public PieceType GetPieceType() => pieceType;

        public void DestroyPiece()
        {
            StartCoroutine(DestroyAfter(0.15f));
        }

        protected virtual IEnumerator DestroyAfter(float seconds)
        {
            visual.color = Color.red;
            SetCell(null);
            yield return new WaitForSeconds(seconds);

            ReturnToPool();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnAnyPointerDown?.Invoke(this);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnAnyPointerUp?.Invoke(this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnAnyPointerEnter?.Invoke(this);
        }


        public virtual void OnSpawn()
        {
            visual.color = _baseColor;
            _collider2D.enabled = true;
        }

        public virtual void ReturnToPool()
        {
            EventManager.OnPieceReturnToPool?.Invoke(this);
            gameObject.SetActive(false);

        }
    }
}