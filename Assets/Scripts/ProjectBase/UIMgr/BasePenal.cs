using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// 面板基类
/// 找到自己面板下面的空间对象
/// 提供显示或隐藏的行为
/// </summary>
public class BasePanel : MonoBehaviour
{
    //UIBehaviour为所有ui类型的基类 通过里式转换存进来
    //一个控件可能包含多个组件
    private Dictionary<string,List<UIBehaviour>> contorlDic = new Dictionary<string, List<UIBehaviour>>();
    private RectTransform canvasRect;
    private Vector2 curSize;
    protected Vector2 curScale;

    // Start is called before the first frame update
    void Awake()
    {
        //canvasRect = gameObject.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        //curSize = canvasRect.sizeDelta;
        //curScale = Vector2.one;

        FindAllControl<Button>();
        FindAllControl<Image>();
        FindAllControl<Text>();
        FindAllControl<TextMeshPro>();
        FindAllControl<Toggle>();   //单选
        FindAllControl<Slider>();   //复选
        FindAllControl<ScrollRect>();//滚动区域
        FindAllControl<HorizontalLayoutGroup>();
        
    }

    protected void AdaptScreen()
    {
        if (canvasRect.sizeDelta != curSize)
        {
            curSize = canvasRect.sizeDelta;

            gameObject.GetComponent<RectTransform>().sizeDelta = curSize;
        }
    }
    //显示
    public virtual void ShowThis()
    {

    }
    //隐藏
    public virtual void HideThis()
    {

    }
    /// <summary>
    ///得到对应名字的对应控件脚本 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    protected T GetControl<T>(string controlName) where T:UIBehaviour
    {
        if (contorlDic.ContainsKey(controlName))
        {
            //
            for(int i = 0; i < contorlDic[controlName].Count; i++)
            {
                if (contorlDic[controlName][i] is T)
                    return contorlDic[controlName][i] as T;
            }
        }
        return null;
    }

    public void FindFile(string dirPath)
    {
        List<string> dirs = new List<string>();
        //"UI/HPUI"
        FindFiles.FindObjPath(ref dirs, dirPath);
        foreach (string path in dirs)
        {
            ResMgr.GetInstance().LoadAsync<GameObject>(path, (obj) =>
            {
                //Debug.Log(obj.name);
                obj.transform.parent = transform;
            });
        }
    }
    //寻找panel上的所有控件
    private void FindAllControl<T>() where T:UIBehaviour
    {
        //包括子物体的控件
        T[] controllers = this.GetComponentsInChildren<T>();
        string objName;
        for (int i = 0; i < controllers.Length; i++)
        {
            objName = controllers[i].gameObject.name;
            if(contorlDic.ContainsKey(objName))
            {  
                //如果该控件已经包含在字典中
                contorlDic[objName].Add(controllers[i]);
            }
            else
            //将控件的名字和控件存入字典
            contorlDic.Add(objName, new List<UIBehaviour> { controllers[i]});
        }
    }

    
}
