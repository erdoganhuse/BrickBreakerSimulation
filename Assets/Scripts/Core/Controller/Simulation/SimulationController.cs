using System.Collections.Generic;
using Core.Data.Simulation;
using Library.ServiceLocatorSystem;
using Library.SignalBusSystem;
using Signals;
using UnityEngine;

namespace Core.Controller.Simulation
{
    public class SimulationController
    {
        private const string SimulationSaveKey = "SimulationSaveKey";

        public float Duration { get; private set; }
        public bool IsDuringSimulation { get; private set; }
        public bool IsSimulationReady { get; private set; }
        
        private readonly BallLauncher _ballLauncher;
        private readonly BrickSpawner _brickSpawner;
        private readonly BoardController _boardController;

        private float _startTime;
        
        public SimulationController()
        {
            _ballLauncher = ServiceLocator.Get<BallLauncher>();
            _brickSpawner = ServiceLocator.Get<BrickSpawner>();
            _boardController = ServiceLocator.Get<BoardController>();
        }

        ~SimulationController() { }
        
        public void Create()
        {
            if (IsDuringSimulation) End();
            
            IsSimulationReady = true;
            
            SignalBus.Fire(new SimulationCreatedSignal());
        }

        public void Start()
        {
            if (!IsSimulationReady)
            {
                Debug.Log("Create or Load a Simulation First!!!");
                return;
            }

            if (IsDuringSimulation)
            {
                Debug.Log("Already playing Simulation!!!");
                return;
            }
            
            SignalBus.Fire(new SimulationStartedSignal(
                _boardController.TotalBrickCount - _brickSpawner.ActiveBricks.Count, 
                _boardController.TotalBrickCount));
            
            IsDuringSimulation = true;
            _startTime = Time.time;
        }

        public void End(bool isCompleted = false)
        {
            IsDuringSimulation = false;
            IsSimulationReady = false;
            Duration = Time.time - _startTime;
            
            SignalBus.Fire(new SimulationEndedSignal(isCompleted, GetDestructionPercentage(), Duration));                
        }
        
        public void Load()
        {
            if (!HasSavedSimulation())
            {
                Debug.Log("There is no Simulation to Load!!!");
                return;
            }
            
            if(IsDuringSimulation) End();
            
            SimulationData data;
            LoadSimulationData(out data);

            IsSimulationReady = true;
            
            SignalBus.Fire(new SimulationLoadedSignal(data));
        }

        public void Save()
        {
            if (!IsDuringSimulation)
            {
                Debug.Log("There is no Simulation to Save!!!");
                return;
            }
            
            SaveSimulationData(GetCurrentStateData());            
            SignalBus.Fire(new SimulationSavedSignal());
        }

        private bool HasSavedSimulation()
        {
            return PlayerPrefs.HasKey(SimulationSaveKey);
        }

        private SimulationData GetCurrentStateData()
        {
            SimulationData data = new SimulationData();
            data.BallPosition = _ballLauncher.CurrentBall.transform.position;
            data.BallSpeed = _ballLauncher.CurrentBall.Rigidbody.Velocity;

            data.BoardSize = _boardController.GameArea.Size;
            data.BoardCenter = _boardController.GameArea.Center;

            data.TotalBrickCount = _boardController.TotalBrickCount;
            data.Bricks = new List<BrickData>(_brickSpawner.ActiveBricks.Count);

            for (int i = 0; i < _brickSpawner.ActiveBricks.Count; i++)
            {
                data.Bricks.Add((BrickData)_brickSpawner.ActiveBricks[i]);
            }
            
            return data;
        }

        private void LoadSimulationData(out SimulationData data)
        {
            data = new SimulationData();
            data = JsonUtility.FromJson<SimulationData>(PlayerPrefs.GetString(SimulationSaveKey));
        }
        
        private void SaveSimulationData(SimulationData data)
        {
            string dataJsonString = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(SimulationSaveKey, dataJsonString);
        }

        private float GetDestructionPercentage()
        {
            return (float)_brickSpawner.ActiveBricks.Count / _boardController.TotalBrickCount;
        }
    }
}