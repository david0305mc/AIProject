using UnityEngine;
using MonsterLove.StateMachine;

public class EnemyObj : UnitBaseObj
{
    private StateMachine<UnitStates, StateDriver> fsm;
    public EnemyData EnemyData => UnitBaseData as EnemyData;

    public System.Action<long> OnDead; // 죽음 알림 (GameManager용)

    public override void InitObj(UnitBaseData data)
    {
        base.InitObj(data);
        UID = data.UID;

        fsm = new StateMachine<UnitStates, StateDriver>(this);
        fsm.ChangeState(UnitStates.Spawning);
    }

    #region FSM 상태들

    void Spawning_Enter()
    {
        // 등장 애니메이션 등
        fsm.ChangeState(UnitStates.Idle);
    }

    void Idle_Update()
    {
        // 플레이어 찾고 공격 사거리 진입 시:
        fsm.ChangeState(UnitStates.Attack);
    }

    void Attack_Enter()
    {
        // 공격 애니메이션 재생
        AttackEvent?.Invoke(); // 공격 이벤트 호출
        fsm.ChangeState(UnitStates.Idle);
    }

    void Die_Enter()
    {
        // 죽음 애니메이션, 이펙트
        OnDead?.Invoke(UID); // GameManager에 알려줌
    }

    #endregion

    public override void TakeDamage(int amount)
    {
        UnitBaseData.HP -= amount;

        if (UnitBaseData.HP <= 0)
        {
            fsm.ChangeState(UnitStates.Die);
        }
        else
        {
            fsm.ChangeState(UnitStates.Hit); // 경직 등 처리 가능
        }
    }
}
public class UnitBaseObj : MonoBehaviour
{
    public long UID { get; protected set; }
    public System.Action AttackEvent;
    public virtual void TakeDamage(int amount) { }
    public UnitBaseData UnitBaseData { get; set; }
    public virtual void InitObj(UnitBaseData data)
    {
        UnitBaseData = data;
    }
}

public class PlayerObj : UnitBaseObj
{
    private StateMachine<UnitStates, StateDriver> fsm;
    public PlayerData PlayerData => UnitBaseData as PlayerData;

    public override void InitObj(UnitBaseData data)
    {
        base.InitObj(data);
        UID = data.UID;

        fsm = new StateMachine<UnitStates, StateDriver>(this);
        fsm.ChangeState(UnitStates.Spawning);
    }
    public override void TakeDamage(int amount)
    {
        UnitBaseData.HP -= amount;

        if (UnitBaseData.HP <= 0)
        {
            fsm.ChangeState(UnitStates.Die);
        }
    }
}