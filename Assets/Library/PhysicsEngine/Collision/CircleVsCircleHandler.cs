using Library.PhysicsEngine.Data;
using UnityEngine;

namespace Library.PhysicsEngine.Collision
{
    public class CircleVsCircleHandler : ICollisionHandler
    {
        public bool CanHandle(Shape a, Shape b)
        {
            return a.GetType() == typeof(Circle) && b.GetType() == typeof(Circle);
        }

        public void Handle(ref Manifold manifold, Rigidbody.Rigidbody a, Rigidbody.Rigidbody b)
        {
            Circle A = (Circle)a.Shape;
            Circle B = (Circle)b.Shape;

            Vector2 normal = b.transform.position - a.transform.position;

            float distance = normal.magnitude;
            float radius = A.Radius + B.Radius;

            if (distance >= radius)
            {
                manifold.ContactCount = 0;
                return;
            }

            manifold.ContactCount = 1;
            manifold.Contacts = new Vector2[manifold.ContactCount];
            
            if (Mathf.Abs(distance) < Mathf.Epsilon)
            {
                manifold.Penetration = A.Radius;
                manifold.Normal = new Vector2( 1.0f, 0.0f );
                
                manifold.Contacts[0] = a.transform.position;
            }
            else
            {
                manifold.Penetration = radius - distance;
                manifold.Normal = normal / distance;
                manifold.Contacts[0] = manifold.Normal * A.Radius + (Vector2)a.transform.position;
            }
        }
    }
}