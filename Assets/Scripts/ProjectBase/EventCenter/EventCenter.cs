using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IEventInfo
{

}
public class EventInfo<T> : IEventInfo           //基类存子类 里氏转换原则
{
    public UnityAction<T> actions;

    public EventInfo(UnityAction<T> action)
    {
        actions += action;
    }    
}

public class EventInfo<T0,T1> : IEventInfo           //基类存子类 里氏转换原则
{
    public UnityAction<T0,T1> actions;

    public EventInfo(UnityAction<T0,T1> action)
    {
        actions += action;
    }

}

//用于无参事件
public class EventInfo : IEventInfo           //基类存子类 里氏转换原则
{
    public UnityAction actions;

    public EventInfo(UnityAction action)
    {
        actions += action;
    }
}
//事件中心  单例模式
//dictionary
//委托
//观察者设计模式
//泛型
public class EventCenter : BaseManager<EventCenter>
{
    //UnityAction unity自带的委托
    //key 时间名字 value 监听事件的函数们
    private Dictionary<string, IEventInfo> eventDic = new Dictionary<string, IEventInfo>();

    //添加监听事件    
    public void AddEventListener<T>(string name, UnityAction<T> action)
    {
        //有没有对应的事件监听
        //有的情况 
        if(eventDic.ContainsKey(name))
        {
            //值类型是个接口 直接将接口转换为子类再调用子类中的action
            (eventDic[name] as EventInfo<T>).actions += action;
        }
        //没有的情况
        else
        {
            //如果没有就new一个EventInfo添加进词典
            eventDic.Add(name, new EventInfo<T>(action));
        }
    }
    public void AddEventListener<T0,T1>(string name, UnityAction<T0,T1> action)
    {
        //有没有对应的事件监听
        //有的情况 
        if (eventDic.ContainsKey(name))
        {
            //值类型是个接口 直接将接口转换为子类再调用子类中的action
            (eventDic[name] as EventInfo<T0,T1>).actions += action;
        }
        //没有的情况
        else
        {
            //如果没有就new一个EventInfo添加进词典
            eventDic.Add(name, new EventInfo<T0,T1>(action));
        }
    }
    //无参重载
    public void AddEventListener(string name, UnityAction action)
    {
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventInfo).actions += action;
        }  
        else
        {
            eventDic.Add(name, new EventInfo(action));
        }
    }

    //移除对应事件监听
    public void RemoveEventListener<T>(string name, UnityAction<T> action)
    {
        if (eventDic.ContainsKey(name))
            (eventDic[name] as EventInfo<T>).actions -= action;
    }
    public void RemoveEventListener<T0,T1>(string name, UnityAction<T0,T1> action)
    {
        if (eventDic.ContainsKey(name))
            (eventDic[name] as EventInfo<T0,T1>).actions -= action;
    }
    //无参
    public void RemoveEventListener(string name, UnityAction action)
    {
        if (eventDic.ContainsKey(name))
            (eventDic[name] as EventInfo).actions -= action;
    }


    //事件触发
    public void EventTrigger<T>(string name,T info)
    {
        //有没有对应的事件监听
        //有人监听
        if (eventDic.ContainsKey(name))
        {
            //事件不为空
            if((eventDic[name] as EventInfo<T>).actions != null)
                    (eventDic[name] as EventInfo<T>).actions.Invoke(info);
            //eventDic[name].Invoke(info);
        }    
    }

    public void EventTrigger<T0,T1>(string name, T0 info0,T1 info1)
    {
        //有没有对应的事件监听
        //有人监听
        if (eventDic.ContainsKey(name))
        {
            //事件不为空
            if ((eventDic[name] as EventInfo<T0,T1>).actions != null)
                (eventDic[name] as EventInfo<T0,T1>).actions.Invoke(info0,info1);
            //eventDic[name].Invoke(info);
        }
    }
    //无参
    public void EventTrigger(string name)
    {
        if (eventDic.ContainsKey(name))
        {
            if ((eventDic[name] as EventInfo).actions != null)
                (eventDic[name] as EventInfo).actions.Invoke();            
        }
    }

    //情况事件中心 主要在场景切换上
    public  void Clear()
    {
        eventDic.Clear();
    }
}
