using System;
using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bird : MonoBehaviour,IEnemy
{
    public float speed;
    public float floatTime;
    public float digTime;
    
    private BoxCollider2D box;
    private BaseEnemyAnimStateMgr animState;
    private Vector3 target;
    private PineconeMgr Pinecone;
    private bool isfloat;
    private bool isFlyAway;
    
    void Start()
    {
        animState = transform.GetComponent<BaseEnemyAnimStateMgr>();
        box = GetComponent<BoxCollider2D>();
        
        EventCenter.GetInstance().AddEventListener("StartFound",StartFound);
    }

    private void StartFound()
    {
        Destroy(gameObject);
    }
    
    void Fly()
    {
        if(animState.CurState != EnemyState.Run)
            return;
        
        float distance = Vector2.Distance(target, transform.position);
        
        if (transform.position.y - target.y > 1.5f && transform.position.y - target.y < 2 && floatTime > 0)
        {
            Float();
        }
        else if (distance < 0.2f && !isFlyAway)
        {
            float random = Random.Range(0,2) % 2 == 0 ? -0.15f:0.15f;
            
            Vector2 pos = new Vector2(target.x + random,target.y + 0.15f);
            transform.position = pos;
            animState.TryChangeState(EnemyState.Attack);
            StartCoroutine(Diging());
        }
        else if(!isfloat)
        {
            transform.position =  Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }
        
        if (Pinecone == null && !isFlyAway)
            FlyAway();
    }

    private Vector2 floatPoint;
    private Vector2 floatTarget;
    void Float()
    {
        
        floatTime -= Time.deltaTime;
        if (!isfloat)
        {
            isfloat = true;
            floatPoint = transform.position;
            transform.localScale = new Vector3(-1,1,1);
            floatTarget = floatPoint + new Vector2(-3, 0);
        }else if (floatTime <= 0)
        {
            isfloat = false;
        }
        
        if (Vector2.Distance(transform.position, floatTarget) > 0.1f)
        {
            transform.position =  Vector3.MoveTowards(transform.position, floatTarget, speed * Time.deltaTime);
        }
        else
        {
            if (floatTarget.x - floatPoint.x > 0)
            {
                transform.localScale = new Vector3(-1,1,1);
                floatTarget = floatPoint + new Vector2(-3, 0);
            }
            else
            {
                transform.localScale = Vector3.one;
                floatTarget = floatPoint + new Vector2(3, 0);
            }
        }
    }
    void Update()
    {
        if (transform.position.x - target.x > 0 && !isfloat)
        {
            transform.localScale = new Vector3(-1,1,1);
        }
        else if(!isfloat) 
        {
            transform.localScale = Vector3.one;
        }
        
        Fly();
        
    }

    private IEnumerator Diging()
    {
        float time = digTime;
        while (time >= 0)
        {
            yield return new WaitForSeconds(0.2f);
            time -= 0.2f;
        }
        
        if (Pinecone != null && animState.CurState == EnemyState.Attack)
        {
            if (Pinecone.transform.parent != null)
            {
                Pinecone.transform.parent.GetComponent<HoleMgr>().DigOutPinecone();
            }
            Pinecone.gameObject.SetActive(true);
            Pinecone.transform.parent = transform;
            Pinecone.transform.localPosition = new Vector3(-0.2f,-0.1f);
            Pinecone.AnimalCatch();
        }
        FlyAway();

    }
    
    public void FlyAway()
    {
        AudioManager.AmbientAudio("BirdFly");
        isFlyAway = true;
        target = transform.position + new Vector3(Random.Range(-3f, 3f), transform.position.y + 10, 0);
        animState.TryChangeState(EnemyState.Run);
        Invoke(nameof(Push),5f);
    }

    public void Push()
    {
        if(transform.Find("Pinecone")!=null)
        {
            Pinecone.Push();
        }
        Destroy(gameObject);
    }
    public void TakeDamage()
    {
        Debug.Log("fall");
        AudioManager.AmbientAudio("BirdHurt");
        if (Pinecone != null)
        {
            Pinecone.transform.parent = null;
            Pinecone.StartFall();
            FlyAway();
        }
    }
    
    
    
    public void Born(PineconeMgr targetPinecone)
    {
        AudioManager.AmbientAudio("BirdFly");
        target = targetPinecone.transform.position;
        Pinecone = targetPinecone;
        animState.TryChangeState(EnemyState.Run);

        float randomX = target.x + Random.Range(-3f, 3f);
        transform.position = new Vector2(randomX,target.y + 5);
    }
}
