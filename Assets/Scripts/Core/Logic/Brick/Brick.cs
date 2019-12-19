using Core.Data.Simulation;
using DefaultNamespace;
using Library.PhysicsEngine.Data;
using Library.SignalBusSystem;
using Signals;
using UnityEngine;

namespace Core.Logic.Brick
{
    public class Brick : MonoBehaviour
    {
        private Polygon _polygon;
        public Polygon Polygon
        {
            get
            {
                if (_polygon == null) _polygon = GetComponent<Polygon>();
                return _polygon;
            }
        }
        
        public int LifeAmount { get; private set; }
        
        public void Setup(int lifeAmount, Vector2 size)
        {
            LifeAmount = lifeAmount;
            Polygon.Resize(size.x / transform.localScale.x, size.y / transform.localScale.y);
            transform.localScale = size;
        }

        public void Clear()
        {
            LifeAmount = 0;
        }

        public bool IsContainsPoint(Vector2 point)
        {
            Vector2 bottomLeft = transform.position - transform.localScale / 2f;
            Vector2 topRight = transform.position + transform.localScale / 2f;

            return (bottomLeft.x < point.x && bottomLeft.y < point.y) && (point.x < topRight.x && point.y < topRight.y);
        }
        
        private void DecreaseLife(int amount)
        {
            LifeAmount -= amount;

            if (LifeAmount <= 0)
            {
                Destruct();
            }
        }

        private void Destruct()
        {
            gameObject.SetActive(false);
            
            SignalBus.Fire(new BrickDestructedSignal(this));
        }

        public void OnCustomCollisionEnter(Shape other)
        {
            if (other.gameObject.CompareTag(Tags.Ball))
            {
                DecreaseLife(1);
            }
        }
    }
}