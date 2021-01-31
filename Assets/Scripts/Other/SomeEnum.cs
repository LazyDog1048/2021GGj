using UnityEngine;

namespace Player
{
    public enum PlayerState
    {
        Idle,
        Run,
        Fall,
        Jump,
        Crawl,
        CrawlIdle,
        Dig,
        Attack
    }
    
    public enum StandState
    {
        OnGround,
        OnTree,
        Float
    }

    public enum GameState
    {
        GameStart,
        Lost,
        Found,
        GameOver
    }

    public enum PineconeState
    {
        Hang,
        Shake,
        Fall,
        OnGround
    }

    public enum HoleState
    {
        NotDig,
        Stage1,
        Stage2,
        Colding,
        Complete,
        HadPC
    }
}

interface IPoolObj
{
    void Push();
}

interface IEnemy
{
    void Born(PineconeMgr targetPinecone);
    void TakeDamage();
}