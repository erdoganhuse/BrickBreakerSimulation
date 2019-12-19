using UnityEngine;

namespace Library.Extensions
{
    public static class TransformExtensionMethods
    {
        public static void SetPosX(this Transform source, float x)
        {
            source.position = source.position.WithX(x);
        }
        
        public static void SetPosY(this Transform source, float y)
        {
            source.position = source.position.WithY(y);
        }

        public static void SetPosZ(this Transform source, float z)
        {
            source.position = source.position.WithZ(z);
        }

        public static void SetLocalPosX(this Transform source, float x)
        {
            source.localPosition = source.localPosition.WithX(x);
        }
        
        public static void SetLocalPosY(this Transform source, float y)
        {
            source.localPosition = source.localPosition.WithY(y);
        }

        public static void SetLocalPosZ(this Transform source, float z)
        {
            source.localPosition = source.localPosition.WithZ(z);
        }        
    }
}