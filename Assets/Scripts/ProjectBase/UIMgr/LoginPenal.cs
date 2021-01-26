using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : BasePanel
{
    // Start is called before the first frame update
    void Start()
    {
        //根据名字寻找对应的button
        GetControl<Button>("btnStart").onClick.AddListener(ClikStart);
        GetControl<Button>("btnQuit").onClick.AddListener(ClikQuit);
    }

    public void InitInfo()
    {
        Debug.Log("初始化");
    }
    private void ClikStart()
    {
        throw new NotImplementedException();
    }
    private void ClikQuit()
    {
        throw new NotImplementedException();
    }

    // Update is called once per frame
    void Update()
    {
      
    }
}
