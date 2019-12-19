using System;
using System.Collections.Generic;

namespace Library.ServiceLocatorSystem
{
    public class ServiceLocator
    {
        private static readonly Dictionary<Type, Object> Bindings = new Dictionary<Type, Object>();

        public static T Get<T>()
        {
            return (T)Bindings[typeof(T)];
        }

        public static void Bind<T1, T2>() where T2 : T1
        {
            Bindings.Add(typeof(T1), typeof(T2));
        }

        public static void Bind<T>(Object instance)
        {
            Bindings.Add(typeof(T), instance);
        }

        public static void BindInstance(Object instance)
        {
            Bindings.Add(instance.GetType(), instance);
        }

        public static void Clear()
        {
            Bindings.Clear();
        }
    }
}