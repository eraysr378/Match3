using Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cells
{
    public abstract class Cell : MonoBehaviour,IPointerDownHandler, IPointerUpHandler,IPointerEnterHandler,IPointerClickHandler
    {
        [SerializeField] protected SpriteRenderer visual;
        protected CellType cellType;
        public int Row => _row;
        public int Col => _col;
        private int _col;
        private int _row;
        private bool _isChecked= false;

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
        public bool IsChecked() => _isChecked;
        public void Check() => _isChecked = true;
        public void Uncheck() => _isChecked = false;

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
