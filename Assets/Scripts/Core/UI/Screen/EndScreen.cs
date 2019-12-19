using System.Collections;
using System.Globalization;
using Core.Controller.Simulation;
using Library.ServiceLocatorSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI.Screen
{
    public class EndScreen : BaseScreen
    {
        [SerializeField] private Text _destructionPercentageText;
        [SerializeField] private Text _simulationDurationText;

        private SimulationController _simulationController;
        
        private void Start()
        {
            _simulationController = ServiceLocator.Get<SimulationController>();
        }
        
        public void Setup(float destructionPercentage, float simulationDuration)
        {
            _destructionPercentageText.text = $"Destruction Percentage: {100 * (1f - destructionPercentage)}";
            _simulationDurationText.text = $"Simulation Duration: {simulationDuration:F1}";
        }

        #region UI Event Listeners

        public void OnPlayAgainButtonClicked()
        {
            Hide();
            HideListener(() =>
            {
                _simulationController.Create();
            });
        }

        #endregion
    }
}