using System;
using UnityEngine;
using System.Collections.Generic;

namespace Core.Utility
{
    [CreateAssetMenu(menuName = "Core/MethodData", fileName = "Method_")]
    public sealed class MethodData : ScriptableObject
    {
        private List<MethodInfo> _methods;
        private Action _methods_Void;

        public void Initialize() => _methods = new List<MethodInfo>();

        public void MethodSet(MethodInfo[] methods, bool isAdd)
        {
            foreach (MethodInfo method in methods)
            {
                if (isAdd) _methods.Add(method);
                else _methods.Remove(method);
            }
        }
        public void MethodSet(Action[] methods_Void, bool isAdd)
        {
            foreach (Action method in methods_Void)
            {
                if (isAdd) _methods_Void += method;
                else _methods_Void -= method;
            }
        }

        public void Execute<T>(T value)
        {
            foreach (MethodInfo info in _methods.FindAll(i => i.Type == value.GetType()))
            {
                ((Action<T>)info.Method)?.Invoke(value);
            }
        }
        public void Execute() => _methods_Void?.Invoke();
    }

    public sealed class MethodInfo
    {
        internal Delegate Method { get; private set; }
        internal Type Type { get; private set; }

        public MethodInfo(Delegate method, Type type)
        {
            Method = method;
            Type = type;
        }
    }
}