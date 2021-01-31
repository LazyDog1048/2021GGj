using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PineconeTree : MonoBehaviour
{
    public GameObject pinecone;
    public float generateTime;

    private bool isgenerate;
    void Start()
    {
        EventCenter.GetInstance().AddEventListener("StartLost",StartLost);
        EventCenter.GetInstance().AddEventListener("StartFound",StartFound);
    }

    private void StartLost()
    {
        isgenerate = true;
        StartCoroutine(Generate());   
    }

    private void StartFound()
    {
        isgenerate = false;
    }
    private IEnumerator Generate()
    {
        yield return new WaitForSeconds(generateTime);
        GameObject pc = Instantiate(pinecone);
        pinecone.transform.position = transform.position;
        if(isgenerate)
            StartCoroutine(Generate());
    }
}
