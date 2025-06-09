namespace DefaultNamespace
{
    public delegate void GameEnd(string message);

    public delegate void StateChanged<T>(WorldState worldState);

    //BAD BAD CODE, REDO THIS. TEMP DECISION
    public delegate void FrontLoad(bool frontLoad);
}