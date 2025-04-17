using UnityEngine;
using MonsterLove.StateMachine;

public class EnemyObj : MonoBehaviour
{
    public EnemyData EnemyData;
    private StateMachine<UnitStates, StateDriver> fsm;

    public System.Action AttackEvent;
    public void InitObj(UnitBaseData enemyData)
    {
        EnemyData = (EnemyData)enemyData;
        fsm = new StateMachine<UnitStates, StateDriver>(this);
        fsm.ChangeState(UnitStates.Spawning);
    }
}
