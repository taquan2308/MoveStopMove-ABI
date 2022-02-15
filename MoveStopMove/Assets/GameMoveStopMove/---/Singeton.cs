using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Singeton<T> : MonoBehaviour where T : MonoBehaviour

{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                var newGO = new GameObject(typeof(T).Name);
                _instance = newGO.AddComponent<T>();
            }

            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool HasInstance()
    {
        return Instance != null;
    }
}
