using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IData
{

}
public class Data<T> : IData
{
    private Dictionary<string, T> dataDic = new Dictionary<string, T>();  
    public Data(string name,T data)
    {
        dataDic.Clear();
        dataDic.Add(name, data);       
    }
    public void AddData(string name, T data)
    {
        if(!dataDic.ContainsKey(name))
         dataDic.Add(name, data);
    }
    public void ChangeData(string name,T data)
    {
        if (!dataDic.ContainsKey(name))
            dataDic[name] = data;
    }
    public T FindData(string name)
    {
        if (dataDic.ContainsKey(name))
            return dataDic[name];
        else
            return default;
    }
    public void Remove(string name)
    {
        dataDic.Remove(name);
    }
    public bool ContainsKey(string name)
    {
        return dataDic.ContainsKey(name);
    }
    public void Clear()
    {
        dataDic.Clear();
    }
}

//如果没有数据中没有该元素时  创建一个新的放入
public delegate T NewData<T>(T obj);
public class DataMgr : BaseManager<DataMgr>
{
    Dictionary<System.Type,IData> objDic = new Dictionary<System.Type, IData>();

    //添加Obj
    public void AddOrChange<T>(NewData<T> newObj,bool change =false) where T: Object
    {
        T obj = newObj(new Object() as T);

        if (newObj == null)
        {
            Debug.Log("新数据为空");
        }
        //有dataDic
        if (objDic.ContainsKey(obj.GetType()))
        {                 
            if(!change)
                (objDic[obj.GetType()] as Data<T>).AddData(obj.name, obj);        
            else
                (objDic[obj.GetType()] as Data<T>).ChangeData(obj.name, obj);
        }
        //没有dataDic
        else 
        {
            objDic.Add(obj.GetType(), new Data<T>(obj.name, obj));
        }
    }
    //添加Class
    public void AddOrChange<T>(string name,T newObj,UnityAction<T> changeObj = null, bool change = false) where T : class
    {
        T obj = newObj; /*newObj(new Object() as T);*/

        if (newObj == null)
        {
            Debug.Log("新数据为空");
            return;
        }

        //如果需要对obj进行额外操作
        if (changeObj != null)
            changeObj(obj);
      
        //有dataDic
        if (objDic.ContainsKey(obj.GetType()))
        {
            if(!change)
            (objDic[obj.GetType()] as Data<T>).AddData(name, obj);
            else
                (objDic[obj.GetType()] as Data<T>).ChangeData(name, obj);
        }
        //没有dataDic
        else
        {
            objDic.Add(obj.GetType(), new Data<T>(name, obj));
        }
    }

    //Getdata 在callback中调用
    public void GetObj<T>(string name,UnityAction<T> callBack =null) where T:Object
    {
        //存在datadic
        if(objDic.ContainsKey(typeof(T)) && (objDic[typeof(T)] as Data<T>).FindData(name) != default)
        {
            //存在该data      
            callBack((objDic[typeof(T)] as Data<T>).FindData(name));            
        }
        //不存在
        else
        {
            Debug.Log("Type "+ typeof(T) + " name "+name+ "没有该元素!"); 
        }
    }

    public void GetClass<T>(string name, UnityAction<T> callBack = null) where T : class
    {
        //存在datadic
        if (objDic.ContainsKey(typeof(T)) && (objDic[typeof(T)] as Data<T>).FindData(name) != default)
        {
            //存在该data      
            callBack((objDic[typeof(T)] as Data<T>).FindData(name));
        }
        //不存在
        else
        {
            Debug.Log("Type " + typeof(T) + " name " + name + "没有该元素!");
        }
    }

    public void Remove<T>(string name)
    {
        if (objDic.ContainsKey(typeof(T)))
        {
            if((objDic[typeof(T)] as Data<T>).ContainsKey(name))
                 (objDic[typeof(T)] as Data<T>).Remove(name);
        }
        else
        {
            Debug.Log("Type " + typeof(T) + " name " + name + "没有该元素!");
        }
    } 

}

