using System;
using UnityEngine;

namespace Data_Scripts
{
    public class SingletonData<T> : ScriptableObject where T : SingletonData<T>
    {
        private static T _instance;

        public static T instance
        {
            get
            {
                if (_instance == null)
                {
                    T[] assets = Resources.LoadAll<T>("");
                    if (assets == null || assets.Length < 1)
                    {
                        throw new Exception("Couldn't find a singleton scriptable object instance in Resources.");
                    }
                    else if (assets.Length > 1)
                    {
                        Debug.LogWarning("Multiple instances of scriptable singleton found in Resources");
                    }

                    _instance = assets[0];
                }

                return _instance;
            }
        }
    }
}
