using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//mono管理
//1.生命周期函数
//2.事件
//3.协程
public class MonoController : MonoBehaviour
{
    static MonoController Instance;
    private event UnityAction updateEvent; 
    private event UnityAction fixedUpdateEvent;
    private event UnityAction lateUpdateEvent;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);       
    }

  
    // Update is called once per frame
    void Update()
    {
        if(updateEvent!=null)
            updateEvent();
    }

    void FixedUpdate()
    {
        if(fixedUpdateEvent!=null)
            fixedUpdateEvent();
    }
    private void LateUpdate()
    {
        if (lateUpdateEvent != null)
            lateUpdateEvent();
    }
    //添加帧更新事件的函数
    public void AddUpdateListener(UnityAction fun)
    {
        updateEvent += fun;
    }
    public void RemoveUpdateListener(UnityAction fun)
    {
        updateEvent -= fun;
    }
    //添加Fixed帧更新事件的函数
    public void AddFixedUpdateListener(UnityAction fun)
    {        
        fixedUpdateEvent += fun;
    }
    public void RemoveFixedUpdateListener(UnityAction fun)
    {
        fixedUpdateEvent -= fun;
    }
    public void AddLateUpdateListener(UnityAction fun)
    {
        lateUpdateEvent += fun;
    }
    public void RemoveLateUpdateListener(UnityAction fun)
    {
        lateUpdateEvent -= fun;
    }

    public GameObject CopyGameObject(GameObject original)
    {
        GameObject obj = Instantiate(original);
        obj.name = original.name;
        return obj;
    }
    public GameObject CopyGameObject(GameObject original,Transform parent)
    {
        GameObject obj = Instantiate(original,parent);
        obj.transform.localPosition = Vector3.zero;
        obj.name = original.name;
        return obj;
    }
    public GameObject CopyGameObject(GameObject original,Transform parent,Vector3 position)
    {
        GameObject obj = Instantiate(original, transform);
        obj.transform.position = position;
        obj.name = original.name;
        return obj;
    }

    public void DestoryGameObject(GameObject obj)
    {
        Destroy(obj);
    }

    public void DestoryGameObject(GameObject obj,float time)
    {
        Destroy(obj,time);
    }

    public void DontDestroyOnLoad(GameObject obj)
    {
         UnityEngine.Object.DontDestroyOnLoad(obj);
    }


}
