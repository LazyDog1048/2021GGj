using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum UILayer
{
    Bot,
    Mid,
    Top,
    System
}

public class UIMgr : BaseManager<UIMgr>
{
    public Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();

    private Transform canvas;
    private Transform bot;
    private Transform mid;
    private Transform top;
    private Transform system;
    public UIMgr()
    {
        //不移除canvas
        GameObject obj = ResMgr.GetInstance().Load<GameObject>("UI/Canvas");
        canvas = obj.transform;
        GameObject.DontDestroyOnLoad(obj);

        //obj= ResMgr.GerInstance().Load<GameObject>("UI/EventSystem");
        //GameObject.DontDestroyOnLoad(obj);

        bot = canvas.Find("Bot");
        mid = canvas.Find("Mid");
        top = canvas.Find("Top");
        system = canvas.Find("System");
    }
    //显示面板
    //面板名  面板层级 面板回调函数    
    public void ShowPanel<T>(string panelName,UILayer layer = UILayer.Mid,UnityAction<T> callBack = null) where T:BasePanel
    {
        if (panelDic.ContainsKey(panelName))
        {
            panelDic[panelName].ShowThis();
            if (callBack != null)
                callBack(panelDic[panelName] as T);
        }

        ResMgr.GetInstance().LoadAsync<GameObject>("UI/" + panelName, (obj) =>
        {
            //加载完后需要做的事
            //找到父对象显示层级
            Transform father = bot;
            switch (layer)
            {
                case UILayer.Bot:
                    father = bot;
                    break;
                case UILayer.Mid:
                    father = mid;
                    break;
                case UILayer.System:
                    father = system;
                    break;
                case UILayer.Top:
                    father = top;
                    break;
            }
            //设置相对位置大小 和父对象
            obj.transform.SetParent(father);

            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            (obj.transform as RectTransform).offsetMax = Vector2.zero;

            //得到预设体的面板脚本
            T panel = obj.GetComponent<T>();
            //处理面板创建后的逻辑
            if (callBack != null)
                callBack(panel);

            panel.ShowThis();
            panelDic.Add(panelName, panel);
        });
    }
    //隐藏面板
    public void HidePanel(string panelName)
    {
        if(panelDic.ContainsKey(panelName))
        {
            panelDic[panelName].HideThis();
            GameObject.Destroy(panelDic[panelName].gameObject);
            panelDic.Remove(panelName);
        }
    }
}
