using System;
using UnityEngine;
using UnityEngine.Events;

namespace Library.PhysicsEngine.Data
{
    [Serializable]
    public class CollisionEvent : UnityEvent<Shape>{}
    
    public abstract class Shape : MonoBehaviour
    {
        public CollisionEvent OnCollisionEnter;

        public abstract void Initialize();

        public abstract void SetOrient( float radians );

        public abstract Type GetType();
    }
}