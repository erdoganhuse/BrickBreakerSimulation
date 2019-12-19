using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Core.Data.Board;
using Core.Data.Simulation;
using Core.Logic.Brick;
using Library.Extensions;
using Library.PhysicsEngine.Data;
using Library.ServiceLocatorSystem;
using Library.SignalBusSystem;
using Signals;
using UnityEngine;

namespace Core.Controller
{
    public class BoardController : MonoBehaviour
    {
        [SerializeField] private int _brickCountInRow;
        [SerializeField] private float _gapBetweenBricks;
        [SerializeField] private Transform _gameAreaSprite;
        [SerializeField] private Polygon _leftBorder;
        [SerializeField] private Polygon _rightBorder;
        [SerializeField] private Polygon _bottomBorder;
        [SerializeField] private Polygon _topBorder;
        
        public int TotalBrickCount { get; private set; }
        public GameArea GameArea { get; private set; }
        
        private DataController _dataController;
        private BrickSpawner _brickSpawner;
        
        private void Awake()
        {
            _dataController = ServiceLocator.Get<DataController>();
            _brickSpawner = ServiceLocator.Get<BrickSpawner>();
        }

        private void Start()
        {
            SignalBus.Subscribe<SimulationCreatedSignal>(OnSimulationCreated);
            SignalBus.Subscribe<SimulationLoadedSignal>(OnSimulationLoaded);
            SignalBus.Subscribe<SimulationEndedSignal>(OnSimulationEnded);
            
            CreateGameArea();
        }

        private void OnDestroy()
        {
            SignalBus.Unsubscribe<SimulationCreatedSignal>(OnSimulationCreated);
            SignalBus.Unsubscribe<SimulationLoadedSignal>(OnSimulationLoaded);            
            SignalBus.Unsubscribe<SimulationEndedSignal>(OnSimulationEnded);
        }

        private void Clear()
        {
            _brickSpawner.Clear();
        }
        
        public void CreateGameArea()
        {
            Vector2 areaSize = new Vector2(_dataController.Config.GameConfig.gameAreaWidth,
                _dataController.Config.GameConfig.gameAreaHeight);
            
            CreateGameArea(areaSize);
        }
        
        public void CreateGameArea(Vector2 size)
        {
            Vector2 center = new Vector2(0f, 0f);
            
            GameArea = new GameArea(center, size);
            _gameAreaSprite.localPosition = ((Vector3)GameArea.Center).WithZ(2f);
            _gameAreaSprite.localScale = GameArea.Size;

            RearrangeBorders();
            
            SignalBus.Fire(new GameAreaCreatedSignal(GameArea));
        }
        
        public void CreateBoard(List<BrickData> bricks)
        {
            List<Brick> spawnedBricks = _brickSpawner.SpawnMultiple(bricks.Count());
            for (int i = 0; i < spawnedBricks.Count; i++)
            {
                spawnedBricks[i].Setup(bricks[i].Life, bricks[i].Size);
                spawnedBricks[i].transform.localPosition = bricks[i].Position;
                spawnedBricks[i].gameObject.SetActive(true);
            }
        }

        private void RearrangeBorders()
        {
            Vector2 leftBorderDestScale = _leftBorder.transform.localScale.WithY(GameArea.Size.y);
            _leftBorder.Resize(leftBorderDestScale.x / _leftBorder.transform.localScale.x,
                leftBorderDestScale.y / _leftBorder.transform.localScale.y);            
            _leftBorder.transform.position = (Vector2)_gameAreaSprite.position + new Vector2(-GameArea.Size.x/2f, 0f);
            _leftBorder.transform.localScale = _leftBorder.transform.localScale.WithY(GameArea.Size.y);
            
            Vector2 rightBorderDestScale = _rightBorder.transform.localScale.WithY(GameArea.Size.y);
            _rightBorder.Resize(rightBorderDestScale.x / _rightBorder.transform.localScale.x,
                rightBorderDestScale.y / _rightBorder.transform.localScale.y);
            _rightBorder.transform.position = (Vector2)_gameAreaSprite.position + new Vector2(GameArea.Size.x/2f, 0f);
            _rightBorder.transform.localScale = _rightBorder.transform.localScale.WithY(GameArea.Size.y);
            
            Vector2 bottomBorderDestScale = _bottomBorder.transform.localScale.WithX(GameArea.Size.x);
            _bottomBorder.Resize(bottomBorderDestScale.x / _bottomBorder.transform.localScale.x,
                bottomBorderDestScale.y / _bottomBorder.transform.localScale.y);
            _bottomBorder.transform.position = (Vector2)_gameAreaSprite.position + new Vector2(0f, -GameArea.Size.y/2f);
            _bottomBorder.transform.localScale = _bottomBorder.transform.localScale.WithX(GameArea.Size.x);
            
            Vector2 topBorderDestScale = _topBorder.transform.localScale.WithX(GameArea.Size.x);
            _topBorder.Resize(topBorderDestScale.x / _topBorder.transform.localScale.x,
                topBorderDestScale.y / _topBorder.transform.localScale.y);
            _topBorder.transform.position = (Vector2)_gameAreaSprite.position + new Vector2(0f, GameArea.Size.y/2f);
            _topBorder.transform.localScale = _topBorder.transform.localScale.WithX(GameArea.Size.x);
        }

        #region Signal Listeners

        private void OnSimulationCreated(SimulationCreatedSignal simulationCreatedSignal)
        {
            Clear();

            CreateGameArea();
            
            TotalBrickCount = _dataController.Config.GameConfig.numBricksToSpawn;            
            float length = GameArea.GetSingleSquareSize(_brickCountInRow, _gapBetweenBricks);
            Vector2 brickSize = new Vector2(length, length);
            Vector2[] brickPositions = GameArea.GetGridPointsFromUpperLeft(TotalBrickCount, _brickCountInRow, brickSize, _gapBetweenBricks);

            List<BrickData> bricks = new List<BrickData>();
            for (int i = 0; i < TotalBrickCount; i++)
            {
                int life = Random.Range(_dataController.Config.GameConfig.minBrickLife, _dataController.Config.GameConfig.maxBrickLife + 1);
                bricks.Add(new BrickData(){Life = life, Size = brickSize, Position = brickPositions[i]});
            }

            CreateBoard(bricks);
        }

        private void OnSimulationLoaded(SimulationLoadedSignal simulationLoadedSignal)
        {
            Clear();

            TotalBrickCount = simulationLoadedSignal.Data.TotalBrickCount;
            CreateGameArea(simulationLoadedSignal.Data.BoardSize);
            CreateBoard(simulationLoadedSignal.Data.Bricks);
        }
        
        private void OnSimulationEnded()
        {
            Clear();
        }

        #endregion
    }
}