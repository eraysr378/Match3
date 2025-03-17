using System.Collections;
using Cells;
using Interfaces;
using Managers;
using Misc;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Pieces
{
    public abstract class Piece : MonoBehaviour,IPoolableObject,IPointerDownHandler, IPointerUpHandler,IPointerEnterHandler,IPointerClickHandler
    {
        [SerializeField] protected SpriteRenderer visual;
        public Collider2D Collider => _collider;
        public Cell CurrentCell { get; private set; }
        public int Row => CurrentCell.Row;
        public int Col => CurrentCell.Col;

        protected PieceType pieceType;
        
        private Collider2D _collider;
        private Color _baseColor;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

        private void Start()
        {
            _baseColor = visual.color;
        }

        public virtual void Init(Vector3 position,float elementSize,Transform parent,Cell cell = null)
        {
            CurrentCell = cell;
            transform.position = position;
            transform.localScale = Vector3.one * elementSize;
            transform.SetParent(parent);
        }
  

        public void SetCell(Cell newCell)
        {
            CurrentCell?.SetPiece(null);
            CurrentCell = newCell;
            CurrentCell?.SetPiece(this);
        }
        
        public void SetPieceType(PieceType pieceType) => this.pieceType = pieceType;
        public PieceType GetPieceType() => pieceType;

        public void DestroyPiece()
        {
            visual.color = Color.red;
            SetCell(null);
            StartCoroutine(DestroyAfter(0.15f));
        }

        private IEnumerator DestroyAfter(float seconds)
        {
            yield return new WaitForSeconds(seconds);

            ReturnToPool();
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
        public virtual void OnSpawn()
        {
            visual.color = _baseColor;
        }
        
        public void ReturnToPool()
        {
            gameObject.SetActive(false);
            EventManager.OnCellReturnToPool?.Invoke(this);
        }
    }
}
