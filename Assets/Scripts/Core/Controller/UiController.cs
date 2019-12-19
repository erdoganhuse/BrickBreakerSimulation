using Core.UI.Screen;
using Library.ServiceLocatorSystem;
using Library.SignalBusSystem;
using Signals;

namespace Core.Controller
{
    public class UiController
    {
        private readonly InGameScreen _inGameScreen;
        private readonly EndScreen _endScreen;
        
        public UiController()
        {
            _inGameScreen = ServiceLocator.Get<InGameScreen>();
            _endScreen = ServiceLocator.Get<EndScreen>();
            
            SignalBus.Subscribe<GameStartedSignal>(OnGameStarted);
            SignalBus.Subscribe<SimulationEndedSignal>(OnSimulationEnded);
        }

        ~UiController()
        {
            SignalBus.Unsubscribe<GameStartedSignal>(OnGameStarted);
            SignalBus.Unsubscribe<SimulationEndedSignal>(OnSimulationEnded);            
        }

        #region Signal Listeners
        
        private void OnGameStarted()
        {
            _inGameScreen.Show();
        }

        private void OnSimulationEnded(SimulationEndedSignal simulationEndedSignal)
        {
            if (simulationEndedSignal.IsCompleted)
            {
                _endScreen.Setup(simulationEndedSignal.DestructionPercentage, simulationEndedSignal.SimulationDuration);
                _endScreen.Show();
            }
        }        

        #endregion
    }
}