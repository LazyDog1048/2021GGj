using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager<T>  where T:new() 
{
    private static T instance;
    public static T GetInstance()
    {
        if (instance == null)
            instance = new T();
        return instance;
    }
}


public class SingleMono<T> : MonoBehaviour where T :SingleMono<T>
{
    static T _instance;
    //public static T Instance
    //{
    //    get
    //    {
    //        if (_instance == null)
    //            _instance = FindObjectOfType(typeof(T)) as T;
    //        return _instance;
    //    }
    //}

    public static T GetInstance()
    {
        return _instance;
    }
    protected virtual void Awake()
    {
        if (_instance == null)
            _instance = this as T;
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
}