using Library.SignalBusSystem;
using Signals;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities.Constants;

namespace Core.Controller
{
    public class SceneController : MonoBehaviour
    {
        private void Awake()
        {
            SignalBus.Subscribe<GameConfigLoadedSignal>(OnGameConfigLoaded);
        }

        private void OnDestroy()
        {
            SignalBus.Unsubscribe<GameConfigLoadedSignal>(OnGameConfigLoaded);            
        }

        private void LoadGameScene()
        {
            SceneManager.LoadSceneAsync(SceneNames.Game, LoadSceneMode.Additive);
            SceneManager.sceneLoaded += SceneManager_OnSceneLoaded;
        }

        #region Event Listeners
        
        private void OnGameConfigLoaded()
        {
            LoadGameScene();
        }

        private void SceneManager_OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            SceneManager.sceneLoaded -= SceneManager_OnSceneLoaded;

            SceneManager.SetActiveScene(scene);
        }
        
        #endregion        
    }
}