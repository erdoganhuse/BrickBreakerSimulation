using Core.Logic.Ball;
using Library.ServiceLocatorSystem;
using Library.SignalBusSystem;
using Library.Utilities;
using Signals;
using UnityEngine;
using Utilities.Enums;

namespace Core.Controller
{
    public class BallLauncher : MonoBehaviour
    {
        public Ball CurrentBall { get; private set; }
        
        [SerializeField] private Ball _ballPrefab;
        [SerializeField] private Transform _container;

        private DataController _dataController;
        private BoardController _boardController;

        private Vector2 _launchVelocity;
        
        private void Start()
        {            
            _dataController = ServiceLocator.Get<DataController>();
            _boardController = ServiceLocator.Get<BoardController>();
            
            SignalBus.Subscribe<SimulationCreatedSignal>(OnSimulationCreated);
            SignalBus.Subscribe<SimulationStartedSignal>(OnSimulationStarted);
            SignalBus.Subscribe<SimulationLoadedSignal>(OnSimulationLoaded);
            SignalBus.Subscribe<SimulationEndedSignal>(OnSimulationEnded);
        }

        private void OnDestroy()
        {
            SignalBus.Unsubscribe<SimulationCreatedSignal>(OnSimulationCreated);
            SignalBus.Unsubscribe<SimulationStartedSignal>(OnSimulationStarted);
            SignalBus.Unsubscribe<SimulationLoadedSignal>(OnSimulationLoaded);
            SignalBus.Unsubscribe<SimulationEndedSignal>(OnSimulationEnded);
        }
        
        public void Launch(Vector2 velocity)
        {
            CurrentBall.Setup();
            CurrentBall.SetVelocity(velocity);            
        }
        
        private Ball CreateBall()
        {
            return Instantiate(_ballPrefab, _container);
        }

        private Vector2 GetVelocity()
        {
            return _dataController.Config.GameConfig.ballSpeed * Vector2Helper.GetRandomDirection();
        }

        private Vector2 GetSpawnPosition()
        {
            return _boardController.GameArea.GetPosition(Anchor.LowerCenter) +
                   Vector2.up * CurrentBall.transform.localScale.y / 2f;
        }
        
        #region Signal Listeners

        private void OnSimulationCreated(SimulationCreatedSignal simulationCreatedSignal)
        {
            if (CurrentBall == null) CurrentBall = CreateBall();
            
            CurrentBall.transform.position = GetSpawnPosition();
            CurrentBall.Rigidbody.SetVelocity(Vector2.zero);
            CurrentBall.gameObject.SetActive(true);

            _launchVelocity = GetVelocity();
        }
        
        private void OnSimulationStarted()
        {
            Launch(_launchVelocity);
        }
        
        private void OnSimulationLoaded(SimulationLoadedSignal simulationLoadedSignal)
        {
            if (CurrentBall == null) CurrentBall = CreateBall();
            
            CurrentBall.transform.position = simulationLoadedSignal.Data.BallPosition;
            CurrentBall.Rigidbody.SetVelocity(Vector2.zero);
            CurrentBall.gameObject.SetActive(true);

            _launchVelocity = simulationLoadedSignal.Data.BallSpeed;
        }

        private void OnSimulationEnded()
        {
            CurrentBall.Rigidbody.SetVelocity(Vector2.zero);
            CurrentBall.gameObject.SetActive(false);
        }
        
        #endregion
    }
}