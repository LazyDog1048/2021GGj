using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerate : MonoBehaviour
{

    public GameObject bird;
    public GameObject pig;
    public float BornCD;

    private GameObject enemy;
    private PineconeMgr target;
    void Start()
    {
        StartCoroutine(BornEnemy());
    }

    void FingTarget()
    {
        if (PoolMgr.Instance.pineconList.Count > 0)
        {
            List<PineconeMgr> temp = new List<PineconeMgr>(PoolMgr.Instance.pineconList);
            int random = Random.Range(0, temp.Count);
            target = temp[random];
            PoolMgr.Instance.RemovePinecone(target);
            if(target.transform.position.y < 1)
                Born(false);
            else
                Born(true);
        }
        else
        {
            Debug.Log("NOPC");
        }
    }

    private void Born(bool isBird)
    {
        Debug.Log("isbrid" + isBird);
        if (isBird)
            enemy = Instantiate(bird);
        else
            enemy = Instantiate(pig);
        Invoke(nameof(DelaySetBorn),0.5f);
    }

    void DelaySetBorn()
    {
        
        enemy.GetComponent<IEnemy>().Born(target);
    }

    private IEnumerator BornEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(BornCD);
            FingTarget();
            
        }
    }
}
