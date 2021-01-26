using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
//缓存池模块
//Dictionary List
//GameObject和Resource两个公共类中的API
public class PoolData
{
    public GameObject fatherObj;
    public Stack<GameObject> pool;
    public GameObject childObj;
    private string Path;
    public int AllNum;
    public int OutPoolNum
    {
        get { return AllNum - pool.Count; }
    }
    public int InPoolNum
    {
        get { return pool.Count; }
    }
    //对象,父对象,是否动态
    public PoolData(string path,GameObject obj, Transform poolParent)
    {
        //将父节点设为obj
        Path = path;
        childObj = obj;
        fatherObj = new GameObject(obj.name);
        fatherObj.transform.parent = poolParent;
        pool = new Stack<GameObject>();
    }
    //进栈
    public void PushObj(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.parent = fatherObj.transform;
        obj.transform.position = fatherObj.transform.position;
        pool.Push(obj);
    }
    //出栈
    public GameObject PopObj()
    {
        GameObject obj;
        if (pool.Count < 1)
        {
            AllNum++;
            obj = MonoMgr.GetInstance().CopyGameObject(childObj);
        }
        else
        {
            obj = pool.Pop();
        }
        
        obj.transform.parent = null;
        obj.SetActive(true);
        return obj;
    }
    public void Clear()
    {
        AllNum = 0;
        pool.Clear();
    }
}
public class PoolMgr : MonoBehaviour
{
    static PoolMgr instance;

    public Dictionary<string, PoolData> PoolDic = new Dictionary<string, PoolData>();        //动态池
    
    //private GameObject poolObj = new GameObject("pool");

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
//        DontDestroyOnLoad(gameObject);
    }

    public static PoolMgr GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        InitAPool("FireBall","Bullet/FireBall");
        InitAPool("PeaBall","Bullet/PeaBall");
        
    }

    public void InitAPool(string name, string path)
    {
        if(PoolDic.ContainsKey(name))
            return;
        ResMgr.GetInstance().LoadAsync<GameObject>(path, (obj) =>
        {
            obj.name = name;
            obj.SetActive(false);
            PoolDic.Add(name, new PoolData(path,obj, transform));
        }); 
    }
    //手动创建一个对象池 名字  长度 和 obj
    public GameObject PopObj(string name, UnityAction<GameObject> callBack = null)
    {
        if (!PoolDic.ContainsKey(name))
        {
            throw  new NullReferenceException("Not Init This Pool");
        }

        GameObject popObj = PoolDic[name].PopObj();
        
        if(callBack != null)
            callBack(popObj);
        return popObj;
    }
    //放入
    public void PushObj(GameObject obj,UnityAction<GameObject> action = null)
    {
        //if (poolObj == null)
        //{
        //    poolObj = new GameObject("Pool");
        //}
        if (action != null)
            action(obj);
        //在这里移除组件
        
        if (!PoolDic.ContainsKey(obj.name))
        {
            throw  new NullReferenceException("Not Init This Pool");
        }
        //有抽屉
        if (PoolDic.ContainsKey(obj.name))
        {
            PoolDic[obj.name].PushObj(obj);
        }
    }

   

    //清空缓存池方法
    public void Clear()
    {
        PoolDic.Clear();
    }
}
