using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.UI;

public class UiMgr : MonoBehaviour
{
    public static UiMgr Instance;

    public Color32 lostColor = new Color(0.5f,1f,0.5f,1f);
    public Color32 foundColor = new Color(1,0.5f,0,1f);
    public GameObject power;
    public int startLoseSec;
    public int startFoundSec;

    public GameState gameState;
    private int lostSec;
    private int foundSec;
    
    private GameObject keepPinecone;
    private GameObject stateChangeAnim;
    private Button startBtn;
    private Button quitBtn;
    private Transform powerBar;
    private List<GameObject> powerList;
    private Text pineconeNum;
    private Text timePinel;
    private Text gameStateText;

    private Image wipeImage;
    private Text wipeText;
    
    
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        keepPinecone = transform.Find("Pinecone").Find("font").gameObject;
        powerBar = transform.Find("PowerBar");
        stateChangeAnim = transform.Find("StateChangeAnim").gameObject;
        pineconeNum = transform.Find("PineconeNum").Find("Num").GetComponent<Text>();
        gameStateText = transform.Find("GameState").GetComponent<Text>();
        timePinel = gameStateText.transform.Find("TimePanel").GetComponent<Text>();
        
        wipeImage = stateChangeAnim.transform.Find("Wipe").GetComponent<Image>();
        wipeText = stateChangeAnim.transform.Find("Wipe").Find("Text").GetComponent<Text>();
        
        EventCenter.GetInstance().AddEventListener<bool>("KeepPineconeState", KeepPineconeState);
        EventCenter.GetInstance().AddEventListener<int>("PowerChange", SetPower);
        EventCenter.GetInstance().AddEventListener<int>("PineconeCollect", PineconeCollect);
        
        
        startBtn = transform.Find("StartGame").Find("Start").GetComponent<Button>();
        quitBtn = transform.Find("StartGame").Find("Quit").GetComponent<Button>();
        
        startBtn.onClick.AddListener(() =>
        {
            startBtn.transform.parent.gameObject.SetActive(false);
            GameStart();
        });
        
        quitBtn.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
    
    public void InitPowerBar(int maxPower)
    {
        powerList = new List<GameObject>();
        for (int i = 0; i < maxPower; i++)
        {
            GameObject temp = Instantiate(power, powerBar);
            GameObject font = temp.transform.Find("font").gameObject;
            powerList.Add(font);
        }
    }

    private void KeepPineconeState(bool had)
    {
        keepPinecone.SetActive(had);
    }

    private void SetPower(int power)
    {
        for (int i = 0; i < powerList.Count; i++)
        {
            if(i < power)
                powerList[i].SetActive(true);
            else
                powerList[i].SetActive(false);
        }
    }

    private void PineconeCollect(int num)
    {
        pineconeNum.text = num.ToString();
    }
    
    private void StateChange()
    {
        stateChangeAnim.SetActive(true);
        Debug.Log("StateChange");
        if (gameState == GameState.GameStart)
        {
            wipeImage.color = lostColor;
            wipeText.text = "Start";
        }
        else if (gameState == GameState.Lost)
        {
            wipeImage.color = foundColor;
            wipeText.text = "Found";
        }
        else if (gameState == GameState.Found)
        {
            
        }
        Invoke(nameof(StateChangeEnd),1.5f);
    }

    private void StateChangeEnd()
    {
        Debug.Log("End");
        stateChangeAnim.SetActive(false);
        
        if (gameState == GameState.GameStart)
        {
            EventCenter.GetInstance().EventTrigger("StartLost");
            gameState = GameState.Lost;
            gameStateText.text = "Lost:";
            gameStateText.color = lostColor;
            GameLost();
            return;
        }
        
        if (gameState == GameState.Lost)
        {
            EventCenter.GetInstance().EventTrigger("StartFound");
            gameState = GameState.Found;
            gameStateText.text = "Found:";
            gameStateText.color = foundColor;
            GameFound();
            return;
        }
        
        if (gameState == GameState.Found)
        {
            EventCenter.GetInstance().EventTrigger("GameOver");
            gameState = GameState.GameOver;
//            gameStateText.text = "Found:";
//            gameStateText.color = foundColor;
            GameOver();
            return;
        }
        
    }
    private void GameStart()
    {
        EventCenter.GetInstance().EventTrigger("GameStart");
        lostSec = startLoseSec;
        foundSec = startFoundSec;
        gameState = GameState.GameStart;
        StateChange();
        
    }
    private void GameLost()
    {
        Debug.Log("Lost");
        StartCoroutine(UpdateTime(startLoseSec));
    }
    private void GameFound()
    {
        
        StartCoroutine(UpdateTime(startFoundSec));
        Debug.Log("Found");
    }
    private void GameOver()
    {
        gameState = GameState.GameOver;
        Debug.Log("GameOver");
        EventCenter.GetInstance().EventTrigger("GameOver");
    }
    IEnumerator UpdateTime(int time)
    {
        if (time >= 0)
        {
            UpdateTimeText(time);
            yield return new WaitForSeconds(1.0f);
            time--;
            if (gameState == GameState.Lost)
            {
                if(lostSec == 0)
                    StateChange();
                lostSec--;
            }
            else if(gameState == GameState.Found)
            {
                if (foundSec == 0)
                    StateChange();
                foundSec--;
            }
            
            StartCoroutine(UpdateTime(time));
        }
    }
    
    private void UpdateTimeText(int time)
    {
        int min = time / 60;
        int sec = time - (min * 60);

        if (min > 0)
        {
            timePinel.text = string.Format("{0}m : {1}s", min,sec);
        }
        else if (sec >= 0)
        {
            timePinel.text = string.Format("{0}s", sec);
        }
            
    }
   
}
