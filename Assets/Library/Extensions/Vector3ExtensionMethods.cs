
using UnityEngine;

namespace Library.Extensions
{
    public static class Vector3ExtensionMethods
    {
        public static Vector3 WithX(this Vector3 source, float x)
        {
            return new Vector3(x, source.y, source.z);
        }
        
        public static Vector3 WithY(this Vector3 source, float y)
        {
            return new Vector3(source.x, y, source.z);
        }

        public static Vector3 WithZ(this Vector3 source, float z)
        {
            return new Vector3(source.x, source.y, z);
        }
    }
}