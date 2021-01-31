using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPush : MonoBehaviour
{
    public float PushTime;
    private void OnEnable()
    {
        Invoke(nameof(DelayPush),PushTime);
    }
    private void DelayPush()
    {
        PoolMgr.Instance.PushObj(gameObject);
    }

    public void PushRightNow()
    {
        PoolMgr.Instance.PushObj(gameObject);
    }
}
