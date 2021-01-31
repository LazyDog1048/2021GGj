using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class HoleMgr : MonoBehaviour,IPoolObj
{
    public GameObject pineconeObj;
    public float eachDigTime;
    public float disappearTime;
    private HoleState holeState;

    public HoleState HoleState
    {
        get { return holeState; }
    }
    private float digedTime;
    

    private GameObject state1;
    private GameObject state2;
    
    private Transform digPointUi;
    private List<GameObject> digCheckPoints;

    private void Awake()
    {
        state1 = transform.Find("State1").gameObject;
        state2 = transform.Find("State2").gameObject;
        digCheckPoints = new List<GameObject>(); 
        digPointUi = transform.Find("Canvas").Find("DigProcess");
        for (int i = 0; i < digPointUi.childCount; i++)
        {
            GameObject font = digPointUi.GetChild(i).Find("font").gameObject;
            font.SetActive(false);
            digCheckPoints.Add(font);
        }
    }

    void OnEnable()
    {
        if (holeState == HoleState.NotDig)
        {
            state1.SetActive(false);
            state2.SetActive(false);
            digPointUi.gameObject.SetActive(false);
        }
    }

    private void LightProcess(int process)
    {
        
        for (int i = 0;i < digCheckPoints.Count; i++)
        {
            if(i <process)
                digCheckPoints[i].SetActive(true);
            else
                digCheckPoints[i].SetActive(false);
        }
    }

    private bool standOnHole;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            standOnHole = true;
            StartCoroutine(DigHole());
        }
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            standOnHole = false;
            digPointUi.gameObject.SetActive(false);
            digedTime = 0;
            LightProcess(0);
        }
    }

    private IEnumerator DigHole()
    {
        while (standOnHole)
        {
            if (PlayerAnimStateMgr.Instance.CurState == PlayerState.Dig)
            {
                if (holeState == HoleState.Complete)
                {
                    BuryPinecone();
                }
                else if(holeState == HoleState.HadPC)
                {
                    DigOutPinecone();
                }
                else if (holeState != HoleState.Complete && PlayerMgr.Instance.CurPower > 0)
                {
                    digPointUi.gameObject.SetActive(true);
                    digedTime += 0.1f;
                    ChangeHoleState(digedTime);
                }
            }
            else
            {
                digPointUi.gameObject.SetActive(false);
            }
            yield return  new WaitForSeconds(0.1f);
        }
    }

    public void DigOutPinecone()
    {
        digedTime = 0;
        LightProcess(0);
        holeState = HoleState.NotDig;
        if (transform.Find("Pinecone") != null)
        {
            Transform pinecone = transform.Find("Pinecone");
            pinecone.parent = null;
            pinecone.gameObject.SetActive(true);
            pinecone.transform.position = transform.position;
            PoolMgr.Instance.RemovePinecone(pinecone.GetComponent<PineconeMgr>());
        }
    }


    private void ChangeHoleState(float digTime)
    {
        if (digedTime >= eachDigTime && holeState == HoleState.NotDig)
        {
            holeState = HoleState.Stage1;
            LightProcess(1);
        }
        else if (digedTime >= eachDigTime * 2 && holeState == HoleState.Stage1)
        {
            holeState = HoleState.Stage2;
            LightProcess(2);
        }
        else if (digedTime >= eachDigTime * 3 && holeState == HoleState.Stage2)
        {
            LightProcess(3);
            holeState = HoleState.Colding;
            PlayerMgr.Instance.DigComlete();
            digPointUi.gameObject.SetActive(false);
            state1.SetActive(false);
            state2.SetActive(true);
            Invoke(nameof(DigComplete),1f);
        }
    }

    public void BuryPinecone()
    {
        if(PlayerMgr.Instance.CurPinecone < 1)
            return;
        GameObject pc = Instantiate(pineconeObj,transform);
        pc.transform.localPosition = Vector3.zero;
        pc.SetActive(false);
        PoolMgr.Instance.AddPinecone(pc.GetComponent<PineconeMgr>());
        
        PlayerMgr.Instance.BuryPinecone();
        holeState = HoleState.Colding;
        Invoke(nameof(HadPC),1f);
        state1.SetActive(false);
        state2.SetActive(false);
    }

    private void HadPC()
    {
        holeState = HoleState.HadPC;
    }
    private void DigComplete()
    {
        holeState = HoleState.Complete;
    }

    
    public void Push()
    {
        PoolMgr.Instance.PushObj(gameObject);
    }
}
