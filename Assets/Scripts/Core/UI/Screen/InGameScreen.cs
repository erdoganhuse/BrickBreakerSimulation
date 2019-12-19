using Core.Controller.Simulation;
using Library.ServiceLocatorSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI.Screen
{
    public class InGameScreen : BaseScreen
    {
        [SerializeField] private Text _speedText;
        
        private SimulationController _simulationController;
        
        private void Start()
        {
            _simulationController = ServiceLocator.Get<SimulationController>();
            
            _speedText.text = $"SPEED: {Time.timeScale}";
        }
        
        #region UI Event Listeners

        public void OnStartButtonClicked()
        {
            _simulationController.Start();
        }
        
        public void OnNewButtonClicked()
        {  
            _simulationController.Create();
        }

        public void OnSaveButtonClicked()
        {
            _simulationController.Save();
        }
        
        public void OnLoadButtonClicked()
        {   
            _simulationController.Load();
        }

        public void OnSpeedUpButtonClicked()
        {
            Time.timeScale = Time.timeScale + 0.25f;            
            _speedText.text = $"SPEED: {Time.timeScale}";
        }

        public void OnSpeedDownButtonClicked()
        {
            Time.timeScale = Mathf.Max(0f, Time.timeScale - 0.25f);
            _speedText.text = $"SPEED: {Time.timeScale}";            
        }

        #endregion
    }
}