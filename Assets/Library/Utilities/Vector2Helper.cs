using UnityEngine;

namespace Library.Utilities
{
    public static class Vector2Helper
    {
        public static Vector2 GetRandomDirection()
        {
            return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        }
    }
}