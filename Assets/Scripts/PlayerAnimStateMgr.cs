using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerAnimStateMgr : MonoBehaviour
    {
        private PlayerState _playerState;
        private PlayerState _lastState;
        private Animator _anim;
        private PlayerMgr playerMgr;
        void Start()
        {
            _anim = GetComponent<Animator>();
            playerMgr = GetComponent<PlayerMgr>();
        }

        public void SetAnimSpeed(float speed)
        {
            _anim.speed = speed;
        }
        public virtual void TryChangeState(PlayerState playerState)
        {
            if(playerState == _playerState)
                return;
                
            switch (playerState)
            {
                case PlayerState.Idle:
                        _playerState = playerState;
                    break;
                case PlayerState.Run:
                        _playerState = playerState;
                    break;
                case PlayerState.Jump:
                        _playerState = playerState;
                    break;
                case PlayerState.Fall:
                        _playerState = playerState;
                    break;
                case PlayerState.Crawl:
                    _playerState = playerState;
                    break;
            }
                
            OnPlayerStateChange(_playerState);
        }
        
        public void ChangeState(PlayerState playerState)
        {
            if(playerState == _playerState)
                return;
            
            _playerState = playerState;
            
            OnPlayerStateChange(_playerState);
        }
            
        protected void OnPlayerStateChange(PlayerState playerState)
        {
            if(playerState == _lastState)
                return;
            _lastState = playerState;
            
            StateResume();
            
            switch (playerState)
            {
                case PlayerState.Run:
                    _anim.SetBool("isRun", true);
                    break;
                case PlayerState.Jump:
                    _anim.SetBool("isJump", true);
                    break;
                case PlayerState.Fall:
                    _anim.SetBool("isFall", true);
                    break;
                case PlayerState.Crawl:
                    _anim.SetBool("isCrawl", true);
                    break;
                case PlayerState.Attack:
                    _anim.SetBool("isAttack", true);
                    break;
            }
        }
        //重置所有状态
        void StateResume()
        {
            _anim.SetBool("isRun", false );
            _anim.SetBool("isJump", false);
            _anim.SetBool("isFall", false);
            _anim.SetBool("isCrawl", false);
            _anim.SetBool("isAttack", false);
        }

        public PlayerState CurState
        {
            get {return _playerState;}
        }

    }
    

}
