using UnityEngine;

namespace Library.Utilities
{
    public static class Vector2Helper
    {
        public static Vector2 GetRandomDirection()
        {
            return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        }
        
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