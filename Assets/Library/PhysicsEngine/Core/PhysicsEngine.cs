using System;
using System.Collections;
using System.Collections.Generic;
using Library.CoroutineSystem;
using Library.Extensions;
using Library.PhysicsEngine.Data;
using UnityEngine;

namespace Library.PhysicsEngine.Core
{
    public static class PhysicsEngine
    {
        public static int Iterations = 4;
        public static float DeltaTime = Time.deltaTime;
        public static Vector2 Gravity = new Vector2(0f, -9.8f);
        
        private static Coroutine _loopCoroutine;
        private static readonly List<Rigidbody.Rigidbody> Rigidbodies = new List<Rigidbody.Rigidbody>();
        private static readonly List<Manifold> Contacts = new List<Manifold>();
        
        private static Manifold[] _previousFrameContacts;
        
        private static bool _isRunning;

        private static void Init()
        {
            _loopCoroutine = CoroutineManager.StartChildCoroutine(Loop());
            _isRunning = true;
        }

        private static void End()
        {
            CoroutineManager.StopChildCoroutine(_loopCoroutine);
            _isRunning = false;
        }

        public static void Clear()
        {
            Rigidbodies.Clear();
            Contacts.Clear();
        }   
        
        private static IEnumerator Loop()
        {
            while (true)
            {
                Step();

                DeltaTime = Time.fixedDeltaTime * Time.timeScale;
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }
        }

        private static void Step()
        {
            Contacts.Clear();
            
            for (int i = 0; i < Rigidbodies.Count; ++i)
            {
                Rigidbody.Rigidbody a = Rigidbodies[i];
                for (int j = i + 1; j < Rigidbodies.Count; ++j)
                {
                    Rigidbody.Rigidbody b = Rigidbodies[j];

                    if (Math.Abs(a.InverseMass) < Mathf.Epsilon && Math.Abs(b.InverseMass) < Mathf.Epsilon)
                    {
                        continue;
                    }

                    Manifold manifold = new Manifold(ref a, ref b);
                    manifold.Solve();

                    if (manifold.ContactCount > 0)
                    {
                        Contacts.Add(manifold);
                    }
                }
            }
            
            for (int i = 0; i < Rigidbodies.Count; ++i)
            {
                IntegrateForces( Rigidbodies[i], DeltaTime);
            }
            
            for (int i = 0; i < Contacts.Count; ++i)
            {
                Contacts[i].Initialize();
            }
            
            for (int j = 0; j < Iterations; ++j)
            {
                for (int i = 0; i < Contacts.Count; ++i)
                {
                    Contacts[i].ApplyImpulse();
                }
            }
            
            for (int i = 0; i < Rigidbodies.Count; ++i)
            {
                IntegrateVelocity(Rigidbodies[i], DeltaTime);
            }

            for (int i = 0; i < Contacts.Count; ++i)
            {
                Contacts[i].PositionalCorrection();
            }

            for (int i = 0; i < Rigidbodies.Count; ++i)
            {
                Rigidbody.Rigidbody rigidbody = Rigidbodies[i];
                
                rigidbody.SetForce(Vector2.zero);
                rigidbody.SetTorque(0f);
            }

            for (int i = 0; i < Contacts.Count; i++)
            {
                if (!Array.Exists(_previousFrameContacts, item => item.Equals(Contacts[i])))
                {
                    Contacts[i].A.Shape.OnCollisionEnter?.Invoke(Contacts[i].B.Shape);
                    Contacts[i].B.Shape.OnCollisionEnter?.Invoke(Contacts[i].A.Shape);
                }   
            }

            _previousFrameContacts = Contacts.ToArray();
        }

        private static void IntegrateForces( Rigidbody.Rigidbody rigidbody, float deltaTime)
        {
            if (Math.Abs(rigidbody.InverseMass) < Mathf.Epsilon) return;

            float dts = deltaTime * 0.5f;
            rigidbody.SetVelocity(rigidbody.Velocity + rigidbody.Force * (dts * rigidbody.InverseMass));
            rigidbody.SetVelocity(rigidbody.Velocity + rigidbody.GravityScale * Gravity * dts);
            rigidbody.SetAngularVelocity(rigidbody.AngularVelocity + rigidbody.Torque * rigidbody.InverseMass * dts);
        }

        private static void IntegrateVelocity( Rigidbody.Rigidbody rigidbody, float deltaTime)
        {
            if (Math.Abs(rigidbody.InverseMass) < Mathf.Epsilon) return;

            rigidbody.transform.position += (Vector3)rigidbody.Velocity * deltaTime;

            float orient = rigidbody.transform.eulerAngles.z * Mathf.Deg2Rad + rigidbody.AngularVelocity * deltaTime;
            rigidbody.transform.eulerAngles.WithZ(orient * Mathf.Rad2Deg);
            rigidbody.SetOrient(orient);
            
            IntegrateForces(rigidbody, deltaTime);
        }
        
        public static void Add(Rigidbody.Rigidbody collider)
        {
            if(!_isRunning) Init();
            
            Rigidbodies.Add(collider);
        }

        public static void Remove(Rigidbody.Rigidbody collider)
        {
            Rigidbodies.Remove(collider);

            if (_isRunning && Rigidbodies.Count == 0) End();
        }
    }
}