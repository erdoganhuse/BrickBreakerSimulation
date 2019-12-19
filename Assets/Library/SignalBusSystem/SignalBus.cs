using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Library.SignalBusSystem
{
    public class SignalBus
    {
        private static SignalBus _instance;
        private static SignalBus Instance => _instance ?? (_instance = new SignalBus());

        private readonly IDictionary<Type, IList<Action>> _listeners;
        private readonly IDictionary<Type, IList<Action<ISignal>>> _listenersWithParams;
        
        private SignalBus()
        {
            _listeners = new Dictionary<Type, IList<Action>>();
            _listenersWithParams = new Dictionary<Type, IList<Action<ISignal>>>();
        }
        
        public static void Subscribe<T>(Action listener) where T : ISignal
        {
            if (!Instance._listeners.ContainsKey(typeof(T)))
            {
                Instance._listeners.Add(typeof(T), new List<Action>());
            }
                
            Instance._listeners[typeof(T)].Add(listener);
        }        
        
        public static void Subscribe<T>(Action<T> listener) where T : ISignal
        {
            if (!Instance._listenersWithParams.ContainsKey(typeof(T)))
            {
                Instance._listenersWithParams.Add(typeof(T), new List<Action<ISignal>>());
            }
                         
            Action<ISignal> convertedListener = o => listener((T) o);
                        
            Instance._listenersWithParams[typeof(T)].Add(convertedListener);
        }

        public static void Unsubscribe<T>(Action listener) where T : ISignal
        {
            if (!Instance._listeners.ContainsKey(typeof(T))) return;

            if (!Instance._listeners[typeof(T)].Contains(listener)) return;

            Instance._listeners[typeof(T)].Remove(listener);
        }

        public static void Unsubscribe<T>(Action<T> listener) where T : ISignal
        {
            if (!Instance._listenersWithParams.ContainsKey(typeof(T))) return;
                        
            if (Instance._listenersWithParams[typeof(T)].All(item => item.Method != listener.Method)) return;

            Instance._listenersWithParams[typeof(T)].All(item => item.Method == listener.Method);
        }

        public static void Fire<T>(T signal) where T : ISignal
        {
            if (Instance._listeners.ContainsKey(signal.GetType()))
            {
                foreach (var listener in Instance._listeners[signal.GetType()])
                {
                    listener?.Invoke();
                }
            }            
            
            if (Instance._listenersWithParams.ContainsKey(signal.GetType()))
            {
                foreach (var listener in Instance._listenersWithParams[signal.GetType()])
                {
                    listener?.Invoke(signal);
                }
            }
        }      
    }
}