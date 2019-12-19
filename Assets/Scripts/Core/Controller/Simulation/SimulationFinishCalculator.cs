using Library.CoroutineSystem;
using Library.ServiceLocatorSystem;
using Library.SignalBusSystem;
using Signals;
using UnityEngine;

namespace Core.Controller.Simulation
{
    public class SimulationFinishCalculator : ISimulationFinisher
    {
        private readonly SimulationController _simulationController;
        private readonly DataController _dataController;
        
        private int _remainingBrickCount;
        private float _noHitsTimeout;
        private Coroutine _noHitsTimeoutCoroutine;
        
        public SimulationFinishCalculator()
        {
            _simulationController = ServiceLocator.Get<SimulationController>();
            _dataController = ServiceLocator.Get<DataController>();
            
            SignalBus.Subscribe<SimulationStartedSignal>(OnSimulationStarted);
            SignalBus.Subscribe<SimulationEndedSignal>(OnSimulationEnded);
            SignalBus.Subscribe<BrickDestructedSignal>(OnBrickDestructed);

            _noHitsTimeout = _dataController.Config.GameConfig.noHitsTimeout;
        }

        ~SimulationFinishCalculator()
        {
            SignalBus.Unsubscribe<SimulationStartedSignal>(OnSimulationStarted);
            SignalBus.Unsubscribe<SimulationEndedSignal>(OnSimulationEnded);
            SignalBus.Unsubscribe<BrickDestructedSignal>(OnBrickDestructed);
        }

        public void ControlStatus()
        {
            if (_remainingBrickCount <= 0)
            {
                _simulationController.End(true);
            }
        }

        private void StartNoHitsTimer()
        {
            _noHitsTimeoutCoroutine = CoroutineManager.DoAfterGivenTime(_noHitsTimeout, () =>
            {
                SignalBus.Fire(new BallLifetimeEndedSignal());
                _simulationController.End(true);
            });
        }

        private void StopNoHitsTimer()
        {
            CoroutineManager.StopChildCoroutine(_noHitsTimeoutCoroutine);
        }

        #region Signal Listeners

        private void OnSimulationStarted(SimulationStartedSignal simulationStartedSignal)
        {
            _remainingBrickCount = simulationStartedSignal.TotalBrickCount - simulationStartedSignal.DestructedBrickCount;
            
            StopNoHitsTimer();
            StartNoHitsTimer();
        }
        
        private void OnSimulationEnded()
        {
            StopNoHitsTimer();
        }
        
        private void OnBrickDestructed()
        {
            _remainingBrickCount--;

            CoroutineManager.DoAfterFixedUpdate(ControlStatus);
        }
        
        #endregion
    }
}