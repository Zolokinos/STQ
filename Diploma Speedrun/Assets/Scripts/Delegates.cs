namespace DefaultNamespace
{
    public delegate void GameEnd(string message);

    public delegate void StateChanged<T>(WorldState worldState);
}