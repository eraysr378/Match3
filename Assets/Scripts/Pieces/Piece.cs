using Cells;
using Interfaces;
using Managers;
using Misc;
using UnityEngine;
namespace Pieces
{
    public abstract class Piece : MonoBehaviour, IPoolableObject
    {
    
        public Cell CurrentCell { get; private set; }
        public int Row => CurrentCell.Row;
        public int Col => CurrentCell.Col;
        protected bool isBeingDestroyed = false;
         [SerializeField]private PieceType pieceType;


        private Collider2D _collider2D;

        protected virtual void Awake()
        {
            _collider2D = GetComponent<Collider2D>();
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

        public PieceType GetPieceType() => pieceType;

        public void DestroyPieceInstantly()
        {
            SetCell(null);
            OnReturnToPool();
        }
        
        public virtual void OnSpawn()
        {
            _collider2D.enabled = true;
            isBeingDestroyed = false;
        }

        public virtual void OnReturnToPool()
        {
            EventManager.OnPieceReturnToPool?.Invoke(this);
        }
    }
}