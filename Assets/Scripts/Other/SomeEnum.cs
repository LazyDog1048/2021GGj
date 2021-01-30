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
}

interface IPoolObj
{
    void Push();
}