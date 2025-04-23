using System.IO;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

public class BaseData
{
    public long uidSeed;
    public long dbVersion;
    public string userUID;

    public long GetNextUID()
    {
        return uidSeed++;
    }
}
public class UserDataManager : Singleton<UserDataManager>
{
    public BaseData BaseData { get; set; }
    private static readonly string LocalFilePath = Path.Combine(Application.persistentDataPath, "SaveData");

    public long GenerateUID()
    {
        return BaseData.GetNextUID();
    }

    // 플레이어 및 적 데이터 관리
    private SerializableDictionary<long, UnitBaseData> enemyDic = new SerializableDictionary<long, UnitBaseData>();
    private UnitBaseData playerData;

    public UnitBaseData PlayerData => playerData;
    public SerializableDictionary<long, UnitBaseData> EnemyDic => enemyDic;

    protected override void init()
    {
        base.init();
    }
    public void GeneratePlayerData()
    {
        playerData = new UnitBaseData()
        {
            UID = GenerateUID(),
            HP = 100,
            AttackPower = 30
        };
    }

    public EnemyData AddEnemy(int hp = 50, int attackPower = 20)
    {
        var data = new EnemyData()
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
    public async UniTask SaveLocalDataAsync(bool updateVersion = true)
    {
        await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate);

        if (updateVersion)
        {
            // BaseData.dbVersion = GameTime.Get();
        }

        try
        {
            string saveData = JsonUtility.ToJson(BaseData);
            // saveData = Utill.EncryptXOR(saveData);
            Util.SaveFile(LocalFilePath, saveData);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[UserDataManager] 로컬 저장 실패: {e.Message}");
        }
    }

    public async UniTask LoadLocalDataAsync(string uid)
    {
        bool loaded = TryLoadLocalData(uid);
        if (!loaded)
        {
            await CreateNewUserAsync(uid);
        }

        Debug.Log($"[UserDataManager] 로컬 데이터 불러오기 완료 - UID: {BaseData.userUID}");
    }
    private bool TryLoadLocalData(string uid)
    {
        if (!File.Exists(LocalFilePath))
            return false;
        try
        {
            string json = Util.LoadFromFile(LocalFilePath);
            var loaded = JsonUtility.FromJson<BaseData>(json);

            if (loaded == null || string.IsNullOrEmpty(loaded.userUID))
            {
                Debug.LogWarning("[UserDataManager] 잘못된 저장 데이터");
                return false;
            }

            if (loaded.userUID != uid)
                return false;

            BaseData = Util.MergeFields(new BaseData(), loaded);
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[UserDataManager] 로컬 데이터 로딩 실패: {e.Message}");
            return false;
        }
    }

    public async UniTask CreateNewUserAsync(string _uid)
    {
        InitData();
        BaseData.userUID = _uid;
        GeneratePlayerData(); // 여기서 명시적 호출
        await SaveLocalDataAsync(false);
    }
    public void InitData()
    {
        BaseData = new BaseData();
        enemyDic.Clear();
        playerData = null;
    }
}
