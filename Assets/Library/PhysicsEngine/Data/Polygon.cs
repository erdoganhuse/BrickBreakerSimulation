using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Library.PhysicsEngine.Data
{
    public class Polygon : Shape
    {        
        public int VertexCount => Vertices.Count();
        public Vector2[] Vertices;

        private Vector2[] _normals;
        public Vector2[] Normals
        {
            get
            {
                if (_normals == null)
                {
                    _normals = new Vector2[VertexCount];
                    for (int i = 0; i < VertexCount; ++i)
                    {
                        Vector2 face = Vertices[(i + 1) % VertexCount] - Vertices[i];
                        Normals[i] = new Vector2(face.y, -face.x).normalized;
                    }
                }
                return _normals;
            }
        }
        
        private Matrix2x2? _orientation;
        public Matrix2x2 Orientation
        {
            get
            {
                if(_orientation == null) _orientation = new Matrix2x2(transform.eulerAngles.z * Mathf.Deg2Rad);
                return _orientation.Value;
            }
        }

        public override void Initialize()  { }

        public override void SetOrient(float radians) { }

        public override Type GetType()
        {
            return typeof(Polygon);
        }

        public void Resize(float scalePercentageByX, float scalePercentageByY)
        {
            for (int i = 0; i < Vertices.Count(); i++)
            {
                Vertices[i] = new Vector2(Vertices[i].x * scalePercentageByX, Vertices[i].y * scalePercentageByY); 
            }
        }
        
        private void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.color = Color.green;
            for (int i = 0; i < Vertices.Length; i++)
            {
                int endPointIndex = i == (Vertices.Length - 1) ? 0 : (i + 1);
                UnityEditor.Handles.DrawLine((Vector2)transform.position + Vertices[i], (Vector2)transform.position + Vertices[endPointIndex]);
            }
        }
    }
}