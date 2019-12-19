using Library.SignalBusSystem;
using Signals;
using UnityEngine;

namespace Core.Controller
{
    public class GameController
    {
        public GameController()
        {
            SignalBus.Subscribe<SceneReadySignal>(OnSceneReady);
        }

        ~GameController()
        {
            SignalBus.Unsubscribe<SceneReadySignal>(OnSceneReady);
        }

        private void StartGame()
        {
            Application.targetFrameRate = 60;
            Input.multiTouchEnabled = false;
            Time.timeScale = 1f;
            
            SignalBus.Fire(new GameStartedSignal());
        }

        #region Signal Listeners

        private void OnSceneReady()
        {
            StartGame();
        }        

        #endregion
    }
}
