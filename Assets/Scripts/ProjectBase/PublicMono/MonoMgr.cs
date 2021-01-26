using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

public class MonoMgr : BaseManager<MonoMgr>
{
    private MonoController controller;

    public MonoMgr()
    {
        //保证了MonoController的唯一性
        GameObject obj = new GameObject("MonoController");
        controller = obj.AddComponent<MonoController>();
        Debug.Log("mono");
    }

    public void AddUpdateListener(UnityAction fun)
    {
        controller.AddUpdateListener(fun);
        
    }
    public void RemoveUpdateListener(UnityAction fun)
    {
        controller.RemoveUpdateListener(fun);
    }

    public void AddFixedUpdateListener(UnityAction fun)
    {
        controller.AddFixedUpdateListener(fun);

    }
    public void RemoveFixedUpdateListener(UnityAction fun)
    {
        controller.RemoveFixedUpdateListener(fun);
    }
    public void AddLateUpdateListener(UnityAction fun)
    {
        controller.AddLateUpdateListener(fun);

    }
    public void RemoveLateUpdateListener(UnityAction fun)
    {
        controller.RemoveLateUpdateListener(fun);
    }


    //将协程封装一次
    public Coroutine StartCoroutine(IEnumerator routine)
    {
       return controller.StartCoroutine(routine);
    }
    public Coroutine StartCoroutine(string methodName,[DefaultValue("null")]object value)
    {
        return controller.StartCoroutine(methodName,value);
    }
    public Coroutine StartCoroutine(string methodName)
    {
        return controller.StartCoroutine(methodName);
    }
    public void StopCoroutine(IEnumerator routine)
    {
        controller.StopCoroutine(routine);
    }
    public void StopCoroutine(string methodName)
    {
        controller.StopCoroutine(methodName);
    }

    public GameObject CopyGameObject(GameObject original)
    {
        return controller.CopyGameObject(original);
    }
    public GameObject CopyGameObject(GameObject original,Transform parent)
    {
        return controller.CopyGameObject(original,parent);
    }
    public GameObject CopyGameObject(GameObject original,Transform parent, Vector3 position)
    {
        return controller.CopyGameObject(original,parent,position);
    }

    public void DestoryGameObject(GameObject obj)
    {
        controller.DestoryGameObject(obj);
    }

    public void DestoryGameObject(GameObject obj,float time)
    {
        controller.DestoryGameObject(obj,time);
    }
    public void DontDestroyOnLoad(GameObject obj)
    {
        controller.DontDestroyOnLoad(obj);
    }
}
