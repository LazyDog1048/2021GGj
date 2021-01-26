using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public enum PlayerState
    {
        Idle,
        Run,
        Attack
    }
    public class PlayerAnimStateMgr : MonoBehaviour
    {
        private PlayerState _playerState;
        private PlayerState _lastState;
        private Animator _anim;

        void Start()
        {
            _anim = GetComponent<Animator>();
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
                case PlayerState.Attack:
                    _playerState = playerState;
                    break;
            }
                
            OnPlayerStateChange(_playerState);
        }
        
        public void ChangeState(PlayerState playerState)
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
                case PlayerState.Attack:
                    _playerState = playerState;
                    break;
            }
                
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
                    _anim.SetBool("isWalk", true);
                    break;
                case PlayerState.Attack:
                    _anim.SetBool("isAttack", true);
                    break;
            }
        }
        //重置所有状态
        void StateResume()
        {
            _anim.SetBool("isWalk", false );
            _anim.SetBool("isAttack", false);
        }

        public PlayerState CurState()
        {
            return _playerState;
        }

    }
    

}
