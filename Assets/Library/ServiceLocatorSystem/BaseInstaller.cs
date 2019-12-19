using UnityEngine;

namespace Library.ServiceLocatorSystem
{
    public abstract class BaseInstaller : MonoBehaviour
    {
        public abstract void InstallBindings();

        protected virtual void Awake()
        {
            InstallBindings();
        }
    }
}