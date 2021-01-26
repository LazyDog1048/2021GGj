using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//资源加载模块
//1.异步加载
//2.委托和lambda
//3.协程
//4.泛型
public class ResMgr : BaseManager<ResMgr>
{
    //同步加载资源   name 一个路径
    public T Load<T>(string name) where T : Object
    {
        //从路径中加载object
        T res = Resources.Load<T>(name);
        //如果对象是一个GameObject类型 ,实例化后再返回 
        if (res is GameObject)
            return GameObject.Instantiate(res);
        else
            return res;
    }
    /// <summary>
    /// 异步加载资源
    /// 路径
    /// 回调函数
    /// </summary>
    /// <param name="name"></param>
    /// <param name="callBack"></param>
    public void LoadAsync<T>(string name,UnityAction<T> callBack) where T : Object
    {
        MonoMgr.GetInstance().StartCoroutine(ReallyLoadAsync(name,callBack));
        
    }
    //用于开启异步加载资源
    private IEnumerator ReallyLoadAsync<T>(string name, UnityAction<T> callBack) where T : Object
    {
        ResourceRequest r = Resources.LoadAsync<T>(name);     
        yield return r;
        //加载完毕
        if (r.asset is GameObject)
            callBack(GameObject.Instantiate(r.asset) as T);
        else
            callBack(r.asset as T);
    }
}
