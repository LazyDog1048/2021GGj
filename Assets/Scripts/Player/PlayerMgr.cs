using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

namespace Player
{
    public class PlayerMgr : MonoBehaviour
    {
        
        public static PlayerMgr Instance;
        
        private BoxCollider2D box;
        private Rigidbody2D rb;
        private PlayerAnimStateMgr playerAnimState;
        private StandState standState;

        public float speed;
        public float jumpForce;
        private Vector2 _moveDir;
        private bool canCrawlTree;
        private bool touchGround;

        //audio Clip
        private AudioClip hit;
        private AudioClip jump;
        private AudioClip dig;
        private AudioClip getPinecone;
        //人物属性
        [Header("人物属性")] 
        public int pineconePower;            //吃松果回复体力
        public int maxPower;                //最大体力
        public int maxPineConeCount;        //最大携带松果数量
        private int curPineConeCount;           //携带松果数量
        private int curPower;                   //体力
//        private bool keepPinecone;              //手上是否有松果
        public Vector2 MoveDir
        {
            get { return _moveDir; }
        }

        public StandState StandState
        {
            get { return standState; }
        }

        public int CurPinecone
        {
            get { return curPineConeCount; }
        }
        
        public int CurPower
        {
            get { return curPower; }
        }
        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }    

        
        void Start()
        {
            standState = StandState.OnGround;
            box = GetComponent<BoxCollider2D>();
            rb = GetComponent<Rigidbody2D>();
            playerAnimState = GetComponent<PlayerAnimStateMgr>();
            EventCenter.GetInstance().AddEventListener<KeyCode>("KeyPress", GetKeyPress);
            EventCenter.GetInstance().AddEventListener<KeyCode>("KeyDown", GetKeyDown);
            EventCenter.GetInstance().AddEventListener<KeyCode>("KeyUp", GetKeyUp);
            EventCenter.GetInstance().AddEventListener<Vector2>("MoveVector2", GetMoveVector2);
            
            EventCenter.GetInstance().AddEventListener("GameStart",GameStart);
            EventCenter.GetInstance().AddEventListener("StartLost",StartLost);
            EventCenter.GetInstance().AddEventListener("StartFound",StartFound);
            EventCenter.GetInstance().AddEventListener("GameOver",GameOver);

            StartCoroutine(LoadAudio());
        }
        
        IEnumerator LoadAudio()
        {
            ResourceRequest rr = Resources.LoadAsync<AudioClip>("Sound/Dig");
            yield return rr;
            dig = rr.asset as AudioClip;
            
            rr = Resources.LoadAsync<AudioClip>("Sound/Jump");
            yield return rr;
            jump = rr.asset as AudioClip;
            
            rr = Resources.LoadAsync<AudioClip>("Sound/Hit");
            yield return rr;
            hit = rr.asset as AudioClip;

            rr = Resources.LoadAsync<AudioClip>("Sound/GetPinecone");
            yield return rr;
            getPinecone = rr.asset as AudioClip;
        }

        private void GameStart()
        {
            InPutMgr.GetInstance().StartOrEnd(true);
            UiMgr.Instance.InitPowerBar(maxPower);
        }

        private void StartLost()
        {
            
        }
        
        private void StartFound()
        {
            
        }
        
        private void GameOver()
        {
            
        }
        
        private void GetKeyPress(KeyCode key)
        {
            switch (key)
            {
                case KeyCode.W:
                    if (canCrawlTree && standState != StandState.OnTree)
                    {
                        Debug.Log("Crawl");
                        rb.bodyType = RigidbodyType2D.Kinematic;
                        standState = StandState.OnTree;
                        playerAnimState.TryChangeState(PlayerState.Crawl);
                    }
                    break;
                case KeyCode.S:
                    if (standState == StandState.OnGround)
                    {
                        Debug.Log("Ground");
                        playerAnimState.TryChangeState(PlayerState.Idle);
                        rb.bodyType = RigidbodyType2D.Dynamic;
                        standState = StandState.OnGround;
                    }
                    break;
            }
            
        }

        private void GetKeyDown(KeyCode key)
        {
            switch (key)
            {
                case KeyCode.Space:
                    if (standState == StandState.OnGround || standState == StandState.OnTree)
                    {
                        AudioManager.PlayerFxAudio(jump,1,false);
                        rb.bodyType = RigidbodyType2D.Dynamic;
                        playerAnimState.TryChangeState(PlayerState.Jump);
                        Jump();
                    }
                    break;
                case KeyCode.J:
                    if (standState == StandState.OnGround)
                    {
                        AudioManager.PlayerFxAudio(dig,0.5f,true);
                        DigHole(true);
                    }
                    break;
                case KeyCode.K:
                        AudioManager.PlayerFxAudio(dig,0.5f,true);
                        playerAnimState.TryChangeState(PlayerState.Attack);
                    break;
                case KeyCode.U:
                    if(curPineConeCount >0 && curPower != maxPower)
                        EatPinecone();
                    break;
            }            
        }
        
        private void GetKeyUp(KeyCode key)
        {
            switch (key)
            {
                case KeyCode.J:
                    if (standState == StandState.OnGround)
                    {
                        AudioManager.PlayerFxAudio(dig,0,false);
                        DigHole(false);
                    }
                    break;
                
                case KeyCode.K:
                    AudioManager.PlayerFxAudio(dig,0,false);
                    playerAnimState.TryChangeState(PlayerState.Idle);
                    break;
            }
        }


        private void GetMoveVector2(Vector2 moveDir)
        {
            _moveDir = moveDir;
            if(_moveDir.x > 0)
                transform.localScale = Vector3.one;
            else if(_moveDir.x < 0)
                transform.localScale = new Vector3(-1, 1, 1);
        }
        
        void Update()
        {
//            Debug.Log(playerAnimState.CurState + "  " + standState);
            Run();
            CheckGround();
            CheckState();
        }
        
        private void Run()
        {
            if(playerAnimState.CurState != PlayerState.Crawl)
                rb.velocity = new Vector2(_moveDir.x * speed, rb.velocity.y);
            else
            {
                rb.velocity = _moveDir * speed;
            }
            
        }
        
        private void Jump()
        {
            if(standState != StandState.Float)
                rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
        
        private void CheckState()
        {
            if(playerAnimState.CurState == PlayerState.Dig || playerAnimState.CurState == PlayerState.Attack)
                return;
            
            if (standState == StandState.Float && rb.velocity.y <= 0)
            {
                playerAnimState.TryChangeState(PlayerState.Fall);
            }
            else if (standState == StandState.OnGround && _moveDir.x == 0 && rb.velocity.y <= 0)
            {
                playerAnimState.TryChangeState(PlayerState.Idle);
            }
            else if(standState == StandState.OnGround && _moveDir.x != 0 && rb.velocity.y <=0)
            {
                playerAnimState.TryChangeState(PlayerState.Run);
            }
            
        }
        
        
//        private void OnDrawGizmos()
//        {
//            Vector2 checkSize = new Vector2(box.bounds.size.x, 0.025f);            //0.3
//            Vector2 checkPoint = new Vector2(transform.position.x, transform.position.y - (box.bounds.size.y + checkSize.y) / 2 - 0.125f );//-0.1f
//            Gizmos.DrawWireCube(checkSize, checkPoint);
//        }
           
        private void CheckGround()
        {
            Vector2 checkSize = new Vector2(box.bounds.size.x, 0.025f);            //0.3
            Vector2 checkPoint = new Vector2(transform.position.x, transform.position.y - (box.bounds.size.y + checkSize.y) / 2 - 0.125f );//-0.1f
            Collider2D platformCollider = Physics2D.OverlapBox(checkPoint, checkSize, 0, LayerMask.GetMask("Platform"));

            Vector2 checkTreeSize = new Vector2(box.bounds.size.x/2, box.bounds.size.y);            //0.3
            Vector2 checkTreePoint = (Vector2)transform.position + box.offset;
            Collider2D treeCollider = Physics2D.OverlapBox(checkTreeSize, checkTreePoint, 0, LayerMask.GetMask("Tree"));
                
            if (platformCollider != null)
            {
                if(standState != StandState.OnGround)
                {
//                    playerAnimState.TryChangeState(PlayerState.Idle);
                    //AudioManager.PlayerAudio("fall" + Random.Range(0,2).ToString(), Random.Range(0.3f, 0.5f));
                    standState = StandState.OnGround;
                }
                touchGround = true;
                
            }
            else
            {
                touchGround = false;
            }
            
            if (treeCollider == null)
            {
                canCrawlTree = false;
                if(standState == StandState.OnTree)
                {
                    rb.bodyType = RigidbodyType2D.Dynamic;
                    Invoke(nameof(DelayJump),0.2f);
                }
            }
            
            
            if(platformCollider == null && standState != StandState.OnTree)
                standState = StandState.Float;
            
        }

        public void EatPinecone()
        {
            Debug.Log("EatPinecone");

            curPineConeCount--;
            curPower += pineconePower;
            if (curPower > maxPower)
                curPower = maxPower;
            
            EventCenter.GetInstance().EventTrigger("KeepPineconeState", curPineConeCount > 0);
            EventCenter.GetInstance().EventTrigger("PowerChange", curPower);
        }

        public void DigHole(bool isDig)
        {
            if(isDig)
                playerAnimState.TryChangeState(PlayerState.Dig);
            if(!isDig)
                playerAnimState.ChangeState(PlayerState.Idle);
        }

         
        private void DelayJump()
        {
            standState = StandState.Float;
        }

        public void DigComlete()
        {
            curPower--;
            EventCenter.GetInstance().EventTrigger("PowerChange", curPower);
        }
        public void BuryPinecone()
        {
            curPineConeCount--;
            EventCenter.GetInstance().EventTrigger("KeepPineconeState", curPineConeCount > 0);
        }
        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Tree") && standState != StandState.OnTree)
            {
                canCrawlTree = true;
            }
            if(other.CompareTag("Pinecone") && curPineConeCount < maxPineConeCount)
            {
                curPineConeCount++;
                AudioManager.PlayerFxAudio(getPinecone,1,false);
                EventCenter.GetInstance().EventTrigger("KeepPineconeState", curPineConeCount > 0);
                other.GetComponent<IPoolObj>().Push();
            }

            if (other.CompareTag("Enemy") && playerAnimState.CurState == PlayerState.Attack)
            {
                other.GetComponent<IEnemy>().TakeDamage();
            }
        }
    }
}
