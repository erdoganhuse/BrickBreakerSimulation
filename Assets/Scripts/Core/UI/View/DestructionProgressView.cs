using Library.SignalBusSystem;
using Signals;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI.View
{
    public class DestructionProgressView : MonoBehaviour
    {
        [SerializeField] private Slider _destructionSlider;
        [SerializeField] private Text _percentageText;

        private int _destructedBrickCount; 
        private int _totalBrickCount;
        
        private void Awake()
        {
            SignalBus.Subscribe<SimulationStartedSignal>(OnSimulationStarted);    
            SignalBus.Subscribe<SimulationEndedSignal>(OnSimulationEnded);
            SignalBus.Subscribe<BrickDestructedSignal>(OnBrickDestructed);
        }

        private void OnDestroy()
        {
            SignalBus.Unsubscribe<SimulationStartedSignal>(OnSimulationStarted);    
            SignalBus.Unsubscribe<SimulationEndedSignal>(OnSimulationEnded);
            SignalBus.Unsubscribe<BrickDestructedSignal>(OnBrickDestructed);
        }

        private void Start()
        {
            Hide();
        }
        
        private void Show()
        {
            gameObject.SetActive(true);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }

        private void UpdateUi(float destructionProgress)
        {
            _destructionSlider.value = destructionProgress;
            _percentageText.text = $"%{(int)(destructionProgress * 100)}";
        }
        
        #region Signal Listeners
        
        private void OnSimulationStarted(SimulationStartedSignal simulationStartedSignal)
        {
            _destructedBrickCount = simulationStartedSignal.DestructedBrickCount;
            _totalBrickCount = simulationStartedSignal.TotalBrickCount;
            
             UpdateUi((float)(_totalBrickCount - _destructedBrickCount) / _totalBrickCount);

            Show();
        }

        private void OnSimulationEnded()
        {
            Hide();
        }

        private void OnBrickDestructed()
        {
            _destructedBrickCount++;
            UpdateUi((float)(_totalBrickCount - _destructedBrickCount) / _totalBrickCount);
        }
                
        #endregion
    }
}