using UnityEngine;
using System.Collections.Generic;

public class GameManager : SingletonMono<GameManager>
{
    [SerializeField] private EnemyObj enemyObjPrefab;

    private Dictionary<long, EnemyObj> enemyDic = new Dictionary<long, EnemyObj>();

    public void SpawnEnemy(Vector3? spawnPos = null)
    {
        var enemyData = UserDataManager.Instance.AddEnemy();

        Vector3 position = spawnPos ?? GetRandomSpawnPosition();
        EnemyObj enemyObj = Lean.Pool.LeanPool.Spawn(enemyObjPrefab, position, Quaternion.identity, transform);

        enemyObj.InitObj(enemyData);
        enemyObj.AttackEvent = () =>
        {
            UserDataManager.Instance.HitToPlayer(enemyData.UID);
        };

        if (enemyDic.ContainsKey(enemyData.UID))
        {
            Debug.LogWarning($"[GameManager] Duplicate UID: {enemyData.UID} already exists.");
        }
        else
        {
            enemyDic.Add(enemyData.UID, enemyObj);
        }
    }

    public void DespawnEnemy(long uid)
    {
        if (enemyDic.TryGetValue(uid, out var enemyObj))
        {
            Lean.Pool.LeanPool.Despawn(enemyObj);
            enemyDic.Remove(uid);
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float x = Random.Range(-10f, 10f);
        float z = Random.Range(-10f, 10f);
        return new Vector3(x, 0, z);
    }
}
