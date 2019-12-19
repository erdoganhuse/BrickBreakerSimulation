using Core.Controller.Simulation;
using Library.ServiceLocatorSystem;
using Library.SignalBusSystem;
using Signals;
using UnityEngine;

namespace Sandbox
{
    public class TestScript : MonoBehaviour
    {
        private SimulationController _simulationController;
        
        private void Start()
        {
            _simulationController = ServiceLocator.Get<SimulationController>();
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Debug.Log("Subscribe OnBrickDestroyed");
                SignalBus.Subscribe<BrickDestructedSignal>(OnBrickDestroyed);                
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                Debug.Log("Unsubscribe OnBrickDestroyed");
                SignalBus.Unsubscribe<BrickDestructedSignal>(OnBrickDestroyed); 
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Fire OnBrickDestroyed");
                SignalBus.Fire(new BrickDestructedSignal(null)); 
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                _simulationController.End();   
            }            
        }
        
        private void OnBrickDestroyed(BrickDestructedSignal brickDestructedSignal)
        {
            Debug.Log("OnBrickDestroyed");
        }
    }
}