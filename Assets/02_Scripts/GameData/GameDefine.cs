

using MonsterLove.StateMachine;

public class StateDriver
{
    public StateEvent Update;
}
public enum UnitStates
{
    Spawning,
    Idle,
    Approach,
    Dodge,
    StopMove,
    RandomMove,
    Attack,
    Tackle,
    Headbutt,
    Summon,
    TackleExtraMove,
    KnockBack,
    Dead,
    Win,
}
