using System;
using UnityEngine;

namespace Library.PhysicsEngine.Data
{
    [Serializable]
    public class Circle : Shape
    {
        public float Radius;

        public override void Initialize() { }

        public override void SetOrient(float radians) { }

        public override Type GetType()
        {
            return typeof(Circle);
        }

        private void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.color = Color.green;
            UnityEditor.Handles.DrawWireDisc(transform.position , Vector3.forward, Radius);
        }
    }
}