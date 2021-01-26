using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMgr : MonoBehaviour
{
    public static PlayerMgr Instance;
    private BoxCollider2D box;
    private Rigidbody2D rb;
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }    

    void Start()
    {
        box = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }
 
    void Update()
    {
        
    }
    
    
}
