using System.Collections;
using Enemy;
using UnityEngine;


public class Pig : MonoBehaviour,IEnemy
{
    public float speed;
    public float digTime;
    private BaseEnemyAnimStateMgr animState;

    private bool isGoAway;
    private PineconeMgr Pinecone;
    private Vector3 target;
    void Start()
    {
        animState = GetComponent<BaseEnemyAnimStateMgr>();
    }
 
   
    
    void Update()
    {
        if (transform.position.x - target.x > 0)
        {
            transform.localScale = new Vector3(-1,1,1);
        }
        else 
        {
            transform.localScale = Vector3.one;
        }

        
        Run();
        if (Pinecone != null)
        {
        }
    }

    void Run()
    {
        if(animState.CurState != EnemyState.Run)
            return;
        
            float dis = Vector2.Distance(target, transform.position);
            if (dis < 0.2f && animState.CurState == EnemyState.Run)
            {
//                float random = Random.Range(0,2) % 2 == 0 ? -0.4f:0.4f;
            
                Vector2 pos = new Vector2(target.x ,0.15f);
                transform.position = pos;
                
                Debug.Log(pos);
                animState.TryChangeState(EnemyState.Attack);
                StartCoroutine(Diging());
            }
            else
            {
                transform.position =  Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            }
    }
    
    private IEnumerator Diging()
    {
        float time = digTime;
        AudioManager.EnemyFXAudio("PigSmell");
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

//            Pinecone.gameObject.SetActive(true);
//            Pinecone.transform.parent = transform;
//            Pinecone.transform.localPosition = new Vector3(-0.2f, -0.1f);
            Pinecone.Push();
        }

        GoAway();
    }

    public void GoAway()
    {
        isGoAway = true;
        target = transform.position + new Vector3(Random.Range(-100, 100f), transform.position.y, 0);
        animState.TryChangeState(EnemyState.Run);
        Invoke(nameof(Push),5f);
    }

    public void TakeDamage()
    {
        AudioManager.EnemyFXAudio("PigHurt");
        if (Pinecone != null)
        {
            Pinecone.transform.parent = null;
            PoolMgr.Instance.pineconList.Add(Pinecone);
            Pinecone.StartFall();
        }
        GoAway();
    }
    
    public void Push()
    {
        Destroy(gameObject);
    }
    public void Born(PineconeMgr pinecone)
    {
        if(pinecone == null)
            return;
        Pinecone = pinecone;
        transform.position = new Vector3(pinecone.transform.position.x +Random.Range(-10, 10f),0.15f,0);
        target = new Vector3(pinecone.transform.position.x - 0.4f,0.15f);
        Debug.Log(target);
        animState.TryChangeState(EnemyState.Run);
    }
}
