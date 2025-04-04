using System.Collections;
using Cells;
using Interfaces;
using Managers;
using Misc;
using UnityEngine;

namespace Pieces
{
    public abstract class Piece : MonoBehaviour, IPoolableObject
    {

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

        public void DestroyPieceInstantly()
        {
            SetCell(null);
            Debug.Log("Destroy " + gameObject.name);
            ReturnToPool();
            
        }
        public void DestroyPieceAfter(float seconds)
        {
            
            StartCoroutine(DestroyAfter(seconds));
        }

        protected virtual IEnumerator DestroyAfter(float seconds)
        {
            visual.color = Color.red;
            SetCell(null);
            yield return new WaitForSeconds(seconds);
            Debug.Log("Destroy " + gameObject.name);
            ReturnToPool();
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