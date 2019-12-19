using System.Collections.Generic;
using Core.Logic.Brick;
using Library.Patterns.ObjectPool;
using Library.ServiceLocatorSystem;
using Library.SignalBusSystem;
using Signals;
using UnityEngine;

namespace Core.Controller
{
    public class BrickSpawner : MonoBehaviour
    {
        private const int InitialPoolCapacity = 10;

        public List<Brick> ActiveBricks => _activeBricks;

        [SerializeField] private Brick _brickPrefab;
        [SerializeField] private Transform _container;

        private DataController _dataController;
        
        private GenericPool<Brick> _brickPool;
        private List<Brick> _activeBricks;

        private void Awake()
        {
            SignalBus.Subscribe<BrickDestructedSignal>(OnBrickDestructed);
        }

        private void OnDestroy()
        {
            SignalBus.Unsubscribe<BrickDestructedSignal>(OnBrickDestructed);            
        }
        
        private void Start()
        {
            _activeBricks = new List<Brick>();
            _brickPool = new GenericPool<Brick>(_brickPrefab, InitialPoolCapacity, _container);
        }

        public void Clear()
        {
            for (int i = 0; i < _activeBricks.Count; i++)
            {
                _brickPool.Recycle(_activeBricks[i]);
            }
            
            _activeBricks.Clear();
        }
        
        public List<Brick> SpawnMultiple(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                _activeBricks.Add(_brickPool.Spawn(_container));
            }

            return _activeBricks;
        }
        
        #region Signal Listeners

        private void OnBrickDestructed(BrickDestructedSignal brickDestructedSignal)
        {
            _activeBricks.Remove(brickDestructedSignal.Brick);
            _brickPool.Recycle(brickDestructedSignal.Brick);
        }

        #endregion
    }
}