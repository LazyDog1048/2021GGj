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
        private float _playerMoveX;
        private bool isTouchTree;

        public float PlayerMoveX
        {
            get { return _playerMoveX; }
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
            EventCenter.GetInstance().AddEventListener<float>("MoveHorizontal", GetHorizontal);
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
                        Jump();
                        playerAnimState.TryChangeState(PlayerState.Jump);
                    }
                    break;
                case KeyCode.W:
                    if (isTouchTree)
                    {
                        playerAnimState.TryChangeState(PlayerState.Crawl);
                    }
                    break;
            }            
        }
        
        private void GetKeyUp(KeyCode key)
        {
            
        }
        
        private void GetHorizontal(float horizontal)
        {
            _playerMoveX = horizontal;
            if(horizontal > 0)
                transform.localScale = Vector3.one;
            else if(horizontal < 0)
                transform.localScale = new Vector3(-1, 1, 1);
        }
        
        void Update()
        {
            Run();
            CheckGround();
            CheckState();
        }
        
        private void Run()
        {
            rb.velocity = new Vector2(_playerMoveX * speed, rb.velocity.y);
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
            else if (_playerMoveX == 0 && rb.velocity.y <= 0 && standState == StandState.OnGround)
            {
                playerAnimState.TryChangeState(PlayerState.Idle);
            }
            else if(_playerMoveX != 0 && rb.velocity.y <=0 && standState == StandState.OnGround)
            {
                playerAnimState.TryChangeState(PlayerState.Run);
            }
        }
        
        
//    private void OnDrawGizmos()
//    {
//        Vector2 checkTreeSize = new Vector2(0.025f,box.bounds.size.y);            //0.3
//        Vector2 checkTreePoint = new Vector2(transform.position.x + (box.bounds.size.x + checkTreeSize.x) - 0.125f , transform.position.y);//-0.1f
//        Gizmos.DrawWireCube(checkTreePoint, checkTreeSize);
//    }
       
        private void CheckGround()
        {
            Vector2 checkSize = new Vector2(box.bounds.size.x, 0.025f);            //0.3
            Vector2 checkPoint = new Vector2(transform.position.x, transform.position.y - (box.bounds.size.y + checkSize.y) / 2 - 0.125f );//-0.1f
            Collider2D platformCollider = Physics2D.OverlapBox(checkPoint, checkSize, 0, LayerMask.GetMask("Platform"));

            Vector2 checkTreeSize = new Vector2(0.025f,box.bounds.size.y);            //0.3
            Vector2 checkTreePoint = new Vector2(transform.position.x + (box.bounds.size.x + checkTreeSize.x) - 0.125f , transform.position.y);//-0.1f
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
//                    playerAnimState.TryChangeState(PlayerState.Idle);
                    //AudioManager.PlayerAudio("fall" + Random.Range(0,2).ToString(), Random.Range(0.3f, 0.5f));
                }

                if (playerAnimState.CurState == PlayerState.Crawl)
                    standState = StandState.OnTree;
                else
                    isTouchTree = true;
            }
            else
            {
                isTouchTree = false;
            }
            
            if(platformCollider == null && treeCollider == null)
                standState = StandState.Float;
            
        }
        
    }
}
