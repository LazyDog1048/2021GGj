using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
//场景切换模块
//1.场景异步加载
//2.协程
//3.委托
public class SceneMgr :BaseManager<SceneMgr>
{
  //切换场景 同步
  public void LoadScene(string name,UnityAction fun)
    {
        SceneManager.LoadScene(name);

        fun();
    }
    //提供外部的异步加载的接口方法
    public void LoadSceneAsyn(string name,UnityAction fun)
    {
        MonoMgr.GetInstance().StartCoroutine(ReallyLoadSceneAsyn(name,fun));
    }

    private IEnumerator ReallyLoadSceneAsyn(string name,UnityAction fun)
    {
        AsyncOperation ao =  SceneManager.LoadSceneAsync(name);
        //可以得到场景加载的一个进度
        while (!ao.isDone)
        {
            //事件中心向外分发 
            EventCenter.GetInstance().EventTrigger("Loading",ao.progress);
            //更新进度条
            yield return ao.progress;
        }
        //加载完之后才会执行fun
        fun();
    }
}
