using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class PineconeMgr : MonoBehaviour,IPoolObj
{
    public float hangTime;
    public float ShakeTime;
    private Rigidbody2D _rb;
    private Animator _anim;
    private PineconeState pineconeState;

    public PineconeState PineconeState
    {
        get { return pineconeState; }
    }
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        pineconeState = PineconeState.Hang;
        _rb.bodyType = RigidbodyType2D.Kinematic;
        _anim.enabled = false;
        Invoke(nameof(StartShake),hangTime);
    }

    private void StartShake()
    {
        pineconeState = PineconeState.Shake;
        _anim.enabled = true;
        Invoke(nameof(StartFall),ShakeTime);
    }

    public void StartFall()
    {
        pineconeState = PineconeState.Fall;
        _anim.enabled = false;
        _rb.bodyType = RigidbodyType2D.Dynamic;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Floor"))
        {
            PoolMgr.Instance.AddPinecone(this);
            pineconeState = PineconeState.OnGround;    
        }
    }

    public void AnimalCatch()
    {
        _rb.bodyType = RigidbodyType2D.Kinematic;
    }
    public void Push()
    {
        Debug.Log("Push");
        PoolMgr.Instance.RemovePinecone(this);
        Destroy(gameObject);
    }
    
}
