using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiMgr : MonoBehaviour
{
    public static UiMgr Instance;
    public GameObject power;
    private Transform powerBar;
    private Transform pineconeNum;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        powerBar = transform.Find("PowerBar");
        pineconeNum = transform.Find("PineconeNum");
        EventCenter.GetInstance().AddEventListener<int>("PowerChange", SetPower);
    }
    
    public void InitPowerBar(int maxPower)
    {
        for (int i = 0; i < maxPower; i++)
        {
            GameObject temp = Instantiate(power, powerBar);
        }
    }
    

    private void SetPower(int power)
    {
        for (int i = 0; i < powerBar.childCount; i++)
        {
            GameObject font = powerBar.GetChild(i).Find("font").gameObject;
            if(i < power)
                font.SetActive(true);
            else
                font.SetActive(false);
        }
    }
}
