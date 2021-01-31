using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public enum EnemyState
    {
        Idle,
        Run,
        Float,
        Hurt,
        Attack,
        GoAway
    }
    public class BaseEnemyAnimStateMgr : MonoBehaviour
    {
        private EnemyState _enemyState;
        private EnemyState _lastState;
        private Animator _anim;

        void Start()
        {
            _anim = GetComponent<Animator>();
        }
            
        public virtual void TryChangeState(EnemyState enemyState)
        {
            if(enemyState == _enemyState)
                return;
                
            switch (enemyState)
            {
                case EnemyState.Idle:
                    _enemyState = enemyState;
                    break;
                case EnemyState.Run:
                    _enemyState = enemyState;
                    break;
                case EnemyState.Attack:
                    _enemyState = enemyState;
                    break;
            }
                
            OnEnemyStateChange(_enemyState);
        }
        
        public void ChangeState(EnemyState enemyState)
        {
            if(enemyState == _enemyState)
                return;
                
            switch (enemyState)
            {
                case EnemyState.Idle:
                    _enemyState = enemyState;
                    break;
                case EnemyState.Run:
                    _enemyState = enemyState;
                    break;
                case EnemyState.Attack:
                    _enemyState = enemyState;
                    break;
            }
                
            OnEnemyStateChange(_enemyState);
        }
            
        protected void OnEnemyStateChange(EnemyState enemyState)
        {
            if(enemyState == _lastState)
                return;
            _lastState = enemyState;
                
            StateResume();
                
            switch (enemyState)
            {
                case EnemyState.Run:
                    _anim.SetBool("isRun", true);
                    break;
                case EnemyState.Attack:
                    _anim.SetBool("isAttack", true);
                    break;
            }
        }
        //重置所有状态
        void StateResume()
        {
            _anim.SetBool("isRun", false );
            _anim.SetBool("isAttack", false);
        }

        public EnemyState CurState
        {
            get{return _enemyState;}
        }

    }
    

}
