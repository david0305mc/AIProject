

using MonsterLove.StateMachine;

public class StateDriver
{
    public StateEvent Update;
}
public enum UnitStates
{
    Spawning,
    Idle,
    Move,
    Attack,
    Hit,
    Die
}
