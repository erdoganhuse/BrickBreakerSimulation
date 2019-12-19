using Core.Data.Config;
using Library.CoroutineSystem;
using Library.DependencyInjection;
using Library.Patterns.Singleton;
using Library.SignalBusSystem;
using Signals;
using UnityEngine;

namespace Core.Controller
{
    public class DataController : MonoBehaviour
    {
        public Config Config { get; private set; }
        
        [SerializeField] private string _configFilePath;
        
        private void Start()
        {
            ReadConfigFile();
        }

        private void ReadConfigFile()
        {
            TextAsset configFile = Resources.Load<TextAsset>(_configFilePath);
            Config = JsonUtility.FromJson<Config>(configFile.text);
            
            CoroutineManager.DoAfterFixedUpdate(() =>
            {
                SignalBus.Fire(new GameConfigLoadedSignal());    
            });
        }

        public void SaveSimulation()
        {
            
        }
    }
}