using Core.Controller;
using Core.Controller.Simulation;
using Core.UI.Screen;
using Library.CoroutineSystem;
using Library.ServiceLocatorSystem;
using Library.SignalBusSystem;
using Signals;
using UnityEngine;

namespace Installers
{
    public class GameInstaller : BaseInstaller
    {
        [Header("References")] 
        [SerializeField] private BallLauncher _ballLauncher;
        [SerializeField] private BrickSpawner _brickSpawner;
        [SerializeField] private BoardController _boardController;
        [SerializeField] private CameraController _cameraController;
        
        [Header("UI")] 
        [SerializeField] private InGameScreen _inGameScreen;
        [SerializeField] private EndScreen _endScreen;
        
        public override void InstallBindings()
        {
            ServiceLocator.BindInstance(_ballLauncher);
            ServiceLocator.BindInstance(_brickSpawner);
            ServiceLocator.BindInstance(_boardController);
            ServiceLocator.BindInstance(_cameraController);
            
            ServiceLocator.BindInstance(_inGameScreen);
            ServiceLocator.BindInstance(_endScreen);
            
            ServiceLocator.Bind<GameController>(new GameController());
            ServiceLocator.Bind<SimulationController>(new SimulationController());
            ServiceLocator.Bind<ISimulationFinisher>(new SimulationFinishCalculator());
            ServiceLocator.Bind<UiController>(new UiController());
            
            CoroutineManager.DoAfterFixedUpdate(() =>
            {                
                SignalBus.Fire(new SceneReadySignal());
            });            
        }
    }
}