using UnityEngine;

public class UserDataManager : Singleton<UserDataManager>
{
    private long UIDSeed;

    public long GenerateUID()
    {
        return UIDSeed++;
    }

    // 플레이어 및 적 데이터 관리
    private SerializableDictionary<long, UnitBaseData> enemyDic = new SerializableDictionary<long, UnitBaseData>();
    private UnitBaseData playerData;

    public UnitBaseData PlayerData => playerData;
    public SerializableDictionary<long, UnitBaseData> EnemyDic => enemyDic;

    protected override void init()
    {
        base.init();
        UIDSeed = 0;

        // 초기 PlayerData 생성
        playerData = new UnitBaseData()
        {
            UID = GenerateUID(),
            HP = 100,
            AttackPower = 30
        };
    }

    public UnitBaseData AddEnemy(int hp = 50, int attackPower = 20)
    {
        var data = new UnitBaseData()
        {
            UID = GenerateUID(),
            HP = hp,
            AttackPower = attackPower
        };

        enemyDic[data.UID] = data;
        return data;
    }

    public void HitToPlayer(long attackerUID)
    {
        if (enemyDic.TryGetValue(attackerUID, out var attackerData))
        {
            playerData.HP -= attackerData.AttackPower;
        }
        else
        {
            Debug.LogWarning($"[UserDataManager] Invalid attacker UID: {attackerUID}");
        }
    }

    public void ClearEnemies()
    {
        enemyDic.Clear();
    }
}
