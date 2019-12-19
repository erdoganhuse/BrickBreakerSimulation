using Library.PhysicsEngine.Data;
using UnityEngine;

namespace Library.PhysicsEngine.Collision
{
    public class CircleVsPolygonHandler : ICollisionHandler
    {
        public bool CanHandle(Shape a, Shape b)
        {
            return (a.GetType() == typeof(Circle) && b.GetType() == typeof(Polygon)) ||
                   (a.GetType() == typeof(Polygon) && b.GetType() == typeof(Circle));
        }

        public void Handle(ref Manifold manifold, Rigidbody.Rigidbody a, Rigidbody.Rigidbody b)
        {
	        if (a.Shape.GetType() == typeof(Circle) && b.Shape.GetType() == typeof(Polygon))
	        {
		        AbsoluteHandler(ref manifold, a, b);
	        }            
	        else if (a.Shape.GetType() == typeof(Polygon) && b.Shape.GetType() == typeof(Circle))
	        {
		        AbsoluteHandler(ref manifold, b, a);

		        if (manifold.ContactCount > 0)
		        {
			        manifold.Normal = -manifold.Normal;
		        }
	        }
        }

        private void AbsoluteHandler(ref Manifold manifold, Rigidbody.Rigidbody a, Rigidbody.Rigidbody b)
        {
			Circle A = (Circle) a.Shape;
		    Polygon B = (Polygon) b.Shape;

		    manifold.ContactCount = 0;

		    Vector2 center = B.Orientation.Transpose().Multiply(a.transform.position - b.transform.position);
		    
		    float separation = Mathf.NegativeInfinity;
		    int faceNormal = 0;

		    for (int i = 0; i < B.VertexCount; ++i)
		    {
			    float s = Vector2.Dot(B.Normals[i], center - B.Vertices[i]);

			    if (s > A.Radius) return;

			    if (s > separation)
			    {
				    separation = s;
				    faceNormal = i;
			    }
		    }

		    Vector2 v1 = B.Vertices[faceNormal];
		    int i2 = faceNormal + 1 < B.VertexCount? faceNormal + 1 : 0;
		    Vector2 v2 = B.Vertices[i2];

		    if (separation < Mathf.Epsilon)
		    {
			    manifold.ContactCount = 1;			    
			    manifold.Normal = -B.Orientation.Multiply( B.Normals[faceNormal]);
			    manifold.Contacts = new Vector2[manifold.ContactCount];
			    manifold.Contacts[0] = manifold.Normal * A.Radius + (Vector2)a.transform.position;
			    manifold.Penetration = A.Radius;
			    return;
		    }

		    float dot1 = Vector2.Dot(center - v1, v2- v1);
		    float dot2 = Vector2.Dot(center - v2, v1 - v2);
		    manifold.Penetration= A.Radius- separation;

		    if (dot1 <= 0.0f)
		    {
			    if (Vector2.Distance(center, v1) > A.Radius) return;

			    manifold.ContactCount = 1;
			    manifold.Normal = B.Orientation.Multiply(v1 - center).normalized;
			    manifold.Contacts = new Vector2[manifold.ContactCount];
			    manifold.Contacts[0] = B.Orientation.Multiply(v1) + (Vector2)b.transform.position;
		    }
		    else if (dot2 <= 0.0f)
		    {
			    if (Vector2.Distance(center, v2) > A.Radius) return;

			    manifold.ContactCount = 1;
			    manifold.Normal = B.Orientation.Multiply(v2 - center).normalized;
			    manifold.Contacts = new Vector2[manifold.ContactCount];
			    manifold.Contacts[0] = B.Orientation.Multiply(v2) + (Vector2)b.transform.position;
		    }
		    else
		    {			    
			    Vector2 n = B.Normals[faceNormal];

			    if (Vector2.Dot(center - v1, n) > A.Radius) return;

			    manifold.ContactCount = 1;			    
			    manifold.Normal = -B.Orientation.Multiply(n);
			    manifold.Contacts = new Vector2[manifold.ContactCount];
			    manifold.Contacts[0] = manifold.Normal * A.Radius + (Vector2)a.transform.position;
		    }
        }
    }
}