using Core.Controller;
using Library.ServiceLocatorSystem;
using UnityEngine;

namespace Installers
{
    public class LoaderInstaller : BaseInstaller
    {        
        [Header("References")] 
        [SerializeField] private SceneController _sceneController;
        [SerializeField] private DataController _dataController;
        
        public override void InstallBindings()
        {
            ServiceLocator.BindInstance(_sceneController);
            ServiceLocator.BindInstance(_dataController);
        }
    }
}