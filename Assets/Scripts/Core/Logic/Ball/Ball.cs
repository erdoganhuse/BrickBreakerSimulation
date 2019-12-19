using DefaultNamespace;
using Library.CoroutineSystem;
using Library.PhysicsEngine.Core;
using Library.PhysicsEngine.Data;
using Library.SignalBusSystem;
using Signals;
using UnityEngine;

using Rigidbody = Library.PhysicsEngine.Rigidbody.Rigidbody;

namespace Core.Logic.Ball
{
    [RequireComponent(typeof(Rigidbody), typeof(Polygon))]
    public class Ball : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        public Rigidbody Rigidbody
        {
            get
            {
                if (_rigidbody == null) _rigidbody = GetComponent<Rigidbody>();
                return _rigidbody;
            }
        }

        public void Setup() { }
        
        public void SetVelocity(Vector2 velocity)
        {
            Rigidbody.SetVelocity(velocity);
        }        
    }
}