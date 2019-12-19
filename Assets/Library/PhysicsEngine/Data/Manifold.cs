using System;
using Library.PhysicsEngine.Core;
using Library.PhysicsEngine.Utilities;
using UnityEngine;

namespace Library.PhysicsEngine.Data
{
    public struct Manifold
    {        
        public readonly Rigidbody.Rigidbody A;
        public readonly Rigidbody.Rigidbody B;
        
        public readonly float DynamicFriction;
        public readonly float StaticFriction;

        public Vector2 Normal;
        public Vector2[] Contacts;
        public int ContactCount;        
        public float Penetration;
        public float AverageRestitution;
        
        public Manifold(ref Rigidbody.Rigidbody a, ref Rigidbody.Rigidbody b)
        {
            A = a;
            B = b;
            
            AverageRestitution = Mathf.Min( A.Restitution, B.Restitution);
            StaticFriction = Mathf.Sqrt( Mathf.Pow(A.StaticFriction, 2) + Mathf.Pow(B.StaticFriction, 2));
            DynamicFriction = Mathf.Sqrt( Mathf.Pow(A.DynamicFriction, 2) + Mathf.Pow(B.DynamicFriction, 2));
            
            Normal = new Vector2();
            Penetration = 0f;
            ContactCount = 0;
            
            Contacts = new Vector2[ContactCount];
        }

        public void Initialize()
        {
            for (int i = 0; i < ContactCount; ++i)
            {
                Vector2 ra = Contacts[i] - (Vector2)A.transform.position;
                Vector2 rb = Contacts[i] - (Vector2)B.transform.position;

                Vector2 rv = B.Velocity + Vector2Ext.Cross(B.AngularVelocity, rb) - A.Velocity - Vector2Ext.Cross(A.AngularVelocity, ra);

                if (rv.sqrMagnitude - (Core.PhysicsEngine.DeltaTime * Core.PhysicsEngine.Gravity).sqrMagnitude < Mathf.Epsilon)
                {
                    AverageRestitution = 0.0f;
                }
            }
        }

        public void ApplyImpulse()
        {
            if (Math.Abs(A.InverseMass) < Mathf.Epsilon && Math.Abs(+ B.InverseMass) < Mathf.Epsilon)
            {
                InfiniteMassCorrection();
                return;
            }

            for (int i = 0; i < ContactCount; ++i)
            {
                Vector2 ra = Contacts[i] - (Vector2)A.transform.position;
                Vector2 rb = Contacts[i] - (Vector2)B.transform.position;

                Vector2 rv = B.Velocity + B.AngularVelocity * rb - A.Velocity - A.AngularVelocity * ra;
                
                float contactVel = Vector2.Dot( rv, Normal);
                
                if (contactVel > 0)
                {
                    return;
                }
                
                float raCrossN = Vector2Ext.Cross( ra, Normal);
                float rbCrossN = Vector2Ext.Cross( rb, Normal);
                float invMassSum = A.InverseMass + B.InverseMass + (raCrossN * raCrossN) * A.InverseInertia + (rbCrossN * rbCrossN) * B.InverseInertia;

                
                float j = -(1.0f + AverageRestitution) * contactVel;
                j /= invMassSum;
                j /= ContactCount;
                
                
                Vector2 impulse = Normal * j;
                A.ApplyImpulse(-1f * impulse, ra);
                B.ApplyImpulse(impulse, rb);
                
                rv = B.Velocity + Vector2Ext.Cross(B.AngularVelocity, rb) - A.Velocity - Vector2Ext.Cross(A.AngularVelocity, ra);
                
                Vector2 t = rv - (Normal * Vector2.Dot(rv, Normal));
                t = t.normalized;

                float jt = -Vector2.Dot( rv, t );
                jt /= invMassSum;
                jt /= ContactCount;

                if (Math.Abs(jt) < Mathf.Epsilon)
                {
                    return;
                }

                Vector2 tangentImpulse;
                if (Mathf.Abs( jt ) < j * StaticFriction)
                {
                    tangentImpulse = t * jt;
                }
                else
                {
                    tangentImpulse = t * -j * DynamicFriction;
                }

                A.ApplyImpulse( -tangentImpulse, ra );
                B.ApplyImpulse( tangentImpulse, rb );
            }
        }
        
        public void Solve()
        {
            CollisionSolver.HandleCollision(ref this, A, B);
        }
        
        public void PositionalCorrection()
        {
            float correction = Mathf.Max( Penetration - Constants.PenetrationAllowance, 0.0f ) / (A.InverseMass + B.InverseMass) * Constants.PenetrationCorrection;

            A.transform.position += (Vector3)Normal * (-A.InverseMass * correction);
            B.transform.position += (Vector3)Normal * (B.InverseMass * correction);
        }
        
        public void InfiniteMassCorrection()
        {
            A.SetVelocity(Vector2.zero);
            B.SetVelocity(Vector2.zero);
        }

        public bool Equals(Manifold m)
        {
            return this.A == m.A && this.B == m.B;
        }
    }
}