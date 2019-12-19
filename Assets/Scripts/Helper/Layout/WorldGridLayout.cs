using System.Collections.Generic;
using UnityEngine;

namespace Helper.Layout
{
    public class WorldGridLayout : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _background;
        [SerializeField] private int _columnCount;
        [SerializeField] private int _rowCount;
        
        private readonly List<GameObject> _childList = new List<GameObject>();

        private float CellWidth => BackgroundSize.x / _columnCount;
        private float CellHeight => BackgroundSize.y / _rowCount;
        private Vector2 BackgroundSize => _background.bounds.size;
        private Vector2 LeftTopPosition => new Vector2(_background.transform.localPosition.x - BackgroundSize.x / 2f,
            _background.transform.localPosition.y + BackgroundSize.y / 2f);
                
        public void AddChild(GameObject child)
        {
            child.transform.SetParent(transform);
            child.transform.localPosition = Vector3.zero;
            child.transform.localScale = Vector3.one;
            child.transform.localEulerAngles = Vector3.zero;
            
            _childList.Add(child);
            
            RearrangeLayout();
        }

        public void RemoveChild(int index, out GameObject removedChild)
        {
            removedChild = _childList[index];
            removedChild.transform.SetParent(null);
            _childList.RemoveAt(index);
        }

        public void Clear()
        {
            for (int i = 0; i < _childList.Count; i++)
            {
                _childList[i].transform.SetParent(null);
            }
            _childList.Clear();
        }
        
        public int GetChildCount()
        {
            return _childList.Count;
        }
        
        private void RearrangeLayout()
        {
            for (int i = 0; i < _childList.Count; i++)
            {
                int columnIndex = i % _columnCount;
                int rowIndex = (i / _columnCount);
                float posX = LeftTopPosition.x + CellWidth * ((2 * columnIndex + 1) / 2f);
                float posY = LeftTopPosition.y - CellHeight * ((2 * rowIndex + 1) / 2f);
                
                _childList[i].transform.localPosition = new Vector3(posX, posY, _childList[i].transform.localPosition.z);
            }
        }
    }
}