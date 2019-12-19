using System;
using Library.PhysicsEngine.Data;
using Library.PhysicsEngine.Utilities;
using UnityEngine;

namespace Library.PhysicsEngine.Rigidbody
{
    public class Rigidbody : MonoBehaviour
    {
        public Vector2 Velocity { get; private set; }
        public Vector2 Force { get; private set; }
        public float AngularVelocity { get; private set; }
        public float Torque { get; private set; }
        
        private Shape _shape;
        public Shape Shape
        {
            get
            {
                if (_shape == null) _shape = GetComponent<Shape>();
                return _shape;
            }
        }

        public float InverseMass => Math.Abs(Mass) < Mathf.Epsilon ? 0f : 1f / Mass;
        public float InverseInertia => Math.Abs(Inertia) < Mathf.Epsilon ? 0f : 1f / Inertia;

        public float GravityScale = 1f;
        public float Mass = 1f;
        public float Inertia = 1f;
        public float StaticFriction = 0.5f;
        public float DynamicFriction = 0.3f;
        public float Restitution = 0.2f;

        private void OnEnable()
        {
            Core.PhysicsEngine.Add(this);
        }

        private void OnDisable()
        {
            Core.PhysicsEngine.Remove(this);
        }

        public void SetVelocity(Vector2 velocity)
        {
            Velocity = velocity;
        }

        public void SetAngularVelocity(float angularVelocity)
        {
            AngularVelocity = angularVelocity;
        }

        public void SetTorque(float torque)
        {
            Torque = torque;
        }

        public void SetForce(Vector2 force)
        {
            Force = force;
        }
        
        public void SetOrient(float radians)
        {
            Shape.SetOrient(radians);
        }
        
        public void ApplyForce(Vector2 force)
        {
            Force += force;
        }
        
        public void ApplyImpulse(Vector2 impulse, Vector2 contact)
        {
            Velocity += (InverseMass * impulse);
            AngularVelocity += InverseInertia * Vector2Ext.Cross(contact, impulse);
        }
        
        public void SetStatic()
        {
            Inertia = 0f;
            Mass = 0f;
        }
    }
}