using UnityEngine;

namespace Library.PhysicsEngine.Utilities
{
    public static class Vector2Ext
    {
        public static float Cross(Vector2 a, Vector2 b)
        {
            return a.x * b.y - a.y * b.x;
        }

        public static Vector2 Cross(Vector2 a, float f)
        {
            return new Vector2(a.y * f,  a.x * -f);
        }

        public static Vector2 Cross(float f, Vector2 a)
        {
            return new Vector2(a.y * -f,  a.x * f);
        }
    }
}