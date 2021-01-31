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
        isgenerate = true;
        StartCoroutine(Generate());
    }

    private IEnumerator Generate()
    {
        yield return new WaitForSeconds(generateTime);
        GameObject pc = Instantiate(pinecone);
        pinecone.transform.position = transform.position;
        if(isgenerate)
            StartCoroutine(Generate());
    }


    public void StopGenerate()
    {
        isgenerate = false;
    }
}
