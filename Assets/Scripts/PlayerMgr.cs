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

        public Vector2 MoveDir
        {
            get { return _moveDir; }
        }

        public StandState StandState
        {
            get { return standState; }
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
            InPutMgr.GetInstance().StartOrEnd(true);
            EventCenter.GetInstance().AddEventListener<KeyCode>("KeyPress", GetKeyPress);
            EventCenter.GetInstance().AddEventListener<KeyCode>("KeyDown", GetKeyDown);
            EventCenter.GetInstance().AddEventListener<KeyCode>("KeyUp", GetKeyUp);
//            EventCenter.GetInstance().AddEventListener<float>("MoveHorizontal", GetHorizontal);
            EventCenter.GetInstance().AddEventListener<Vector2>("MoveVector2", GetMoveVector2);
            
        }

        private void GetKeyPress(KeyCode key)
        {
            
        }

        private void GetKeyDown(KeyCode key)
        {
            switch (key)
            {
                case KeyCode.Space:
                    if (standState == StandState.OnGround || standState == StandState.OnTree)
                    {
                        rb.bodyType = RigidbodyType2D.Dynamic;
                        playerAnimState.TryChangeState(PlayerState.Jump);
                        Jump();
                    }
                    break;
                case KeyCode.W:
                    if (canCrawlTree && standState != StandState.OnTree)
                    {
                        rb.bodyType = RigidbodyType2D.Kinematic;
                        standState = StandState.OnTree;
                        playerAnimState.TryChangeState(PlayerState.Crawl);
                    }
                    break;
            }            
        }
        
        private void GetKeyUp(KeyCode key)
        {
            
        }
        
//        private void GetHorizontal(float horizontal)
//        {
//            _playerMoveX = horizontal;

//        }

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
            Debug.Log(playerAnimState.CurState);
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
                playerAnimState.SetAnimSpeed(_moveDir.x * _moveDir.y);
            }
            
        }
        
        private void Jump()
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
        
        private void CheckState()
        {
            if (rb.velocity.y <= 0 && standState != StandState.OnGround)
            {
                playerAnimState.TryChangeState(PlayerState.Fall);
            }
            else if (_moveDir.x == 0 && rb.velocity.y <= 0 && standState == StandState.OnGround)
            {
                playerAnimState.TryChangeState(PlayerState.Idle);
            }
            else if(_moveDir.x != 0 && rb.velocity.y <=0 && standState == StandState.OnGround)
            {
                playerAnimState.TryChangeState(PlayerState.Run);
            }
            
        }
        
        
    //    private void OnDrawGizmos()
    //    {
    //        Vector2 checkTreeSize = box.bounds.size;
    //        Vector2 checkTreePoint = (Vector2)transform.position + box.offset;
    //        Gizmos.DrawWireCube(checkTreePoint, checkTreeSize);
    //    }
           
        private void CheckGround()
        {
            Vector2 checkSize = new Vector2(box.bounds.size.x, 0.025f);            //0.3
            Vector2 checkPoint = new Vector2(transform.position.x, transform.position.y - (box.bounds.size.y + checkSize.y) / 2 - 0.125f );//-0.1f
            Collider2D platformCollider = Physics2D.OverlapBox(checkPoint, checkSize, 0, LayerMask.GetMask("Platform"));

            Vector2 checkTreeSize = box.bounds.size;
            Vector2 checkTreePoint = (Vector2)transform.position + box.offset;
            Collider2D treeCollider = Physics2D.OverlapBox(checkTreeSize, checkTreePoint, 0, LayerMask.GetMask("Tree"));
                
            if (platformCollider != null)
            {
                if(standState != StandState.OnGround)
                {
//                    playerAnimState.TryChangeState(PlayerState.Idle);
                    //AudioManager.PlayerAudio("fall" + Random.Range(0,2).ToString(), Random.Range(0.3f, 0.5f));
                }
                if(playerAnimState.CurState != PlayerState.Jump)
                    standState = StandState.OnGround;
            }
            
            if (treeCollider != null)
            {
                
                if(standState != StandState.OnTree)
                {
                    canCrawlTree = true;                  
                }
            }
            else
            {
                canCrawlTree = false;
            }
            
            if(platformCollider == null && treeCollider == null)
                standState = StandState.Float;
            
        }
        
    }
}
