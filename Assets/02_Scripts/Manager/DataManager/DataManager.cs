using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine.Networking;

public class Table
{
    public int id;
}

public partial class DataManager : Singleton<DataManager>
{
    public static string[] tableNames =
        {
            "Localization",
            "Level",
            "StageInfo",
            "StageMSGroup",
            "UnitActionInfo",
            "UnitInfo",
            "UnitMoveInfo",
            "UnitProjectileInfo",
            "UnitTackleInfo",
            "EquipmentInfo",
            "WeaponAnimInfo",
            "EquipmentLevelInfo",
            "GachaInfo",
            "GachaList",
            "DungeonItemInfo",
            "DropInfo",
            "DungeonBoxInfo",
            "ItemInfo",
            "DungeonCharacterLevel",
            "CharacterInfo",
            "AbilityInfo",
            "AbilityEffectInfo",
            "GuideMissionInfo",
            "DungeonEventList",
            "DungeonEventDialog",
            "DungeonEventBattle",
            "DungeonEventBuff",
            "DotDmgInfo",
            "CustomerInfo",
            "ShopReputationLevel",
            "SummonInfo",
            "ObjectInfo",
            "ShopUpgrade",
        };

    public async UniTask LoadDataAsync()
    {
        foreach (var tableName in tableNames)
        {
            if (tableName == "Localization")
                continue;

            try
            {
                string data = await LoadTableDataAsync(tableName);

                if (string.IsNullOrEmpty(data))
                {
                    Debug.LogError($"데이터를 찾을 수 없습니다: {tableName}");
                    continue;
                }

                var method = GetType().GetMethod($"Bind{tableName}Data");
                if (method == null)
                {
                    Debug.LogError($"메서드를 찾을 수 없습니다: Bind{tableName}Data");
                    continue;
                }

                method.Invoke(this, new object[] { Type.GetType($"DataManager+{tableName}"), data });
            }
            catch (Exception e)
            {
                Debug.LogError($"테이블 로드 실패 {tableName}: {e.Message}");
            }
        }
    }

    public void LoadLocalization()
    {
        try
        {
            string tableName = "Localization";
            string data = Resources.Load<TextAsset>(Path.Combine("Data", tableName))?.text;
            if (string.IsNullOrEmpty(data))
            {
                Debug.LogError("로컬라이제이션 데이터를 찾을 수 없습니다.");
                return;
            }

            var method = GetType().GetMethod($"Bind{tableName}Data");
            if (method == null)
            {
                Debug.LogError("BindLocalizationData 메서드를 찾을 수 없습니다.");
                return;
            }

            method.Invoke(this, new object[] { Type.GetType($"DataManager+{tableName}"), data });
        }
        catch (Exception e)
        {
            Debug.LogError($"로컬라이제이션 로드 실패: {e.Message}");
        }
    }

    public async UniTask LoadConfigTable()
    {
        try
        {
            string data;
#if DEV
			data = Resources.Load<TextAsset>(Path.Combine("Data", "ConfigTable"))?.text;
#else
            string path = Path.Combine(LOCAL_CSV_PATH, CONFIG_TABLE_NAME);
            var request = UnityWebRequest.Get(path);
            await request.SendWebRequest();
            data = request.downloadHandler.text;
#endif

            if (string.IsNullOrEmpty(data))
            {
                Debug.LogError("설정 테이블 데이터를 찾을 수 없습니다.");
                return;
            }

            var rows = CSVSerializer.ParseCSV(data, '|');
            if (rows.Count < 2)
            {
                Debug.LogError("설정 테이블 데이터가 비어있습니다.");
                return;
            }

            rows.RemoveRange(0, 2);
            foreach (var row in rows)
            {
                if (row.Length < 3) continue;

                var field = typeof(ConfigTable).GetField(row[0], BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (field == null)
                {
                    Debug.LogWarning($"필드를 찾을 수 없습니다: {row[0]}");
                    continue;
                }

                try
                {
                    CSVSerializer.SetValue(ConfigTable.Instance, field, row[2]);
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"값 설정 실패 {row[0]}: {e.Message}");
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"설정 테이블 로드 실패: {e.Message}");
        }
    }

    protected object CSVDeserialize(string text, Type type, bool hasSkipLine = true)
    {
        try
        {
            var rows = CSVSerializer.ParseCSV(text, '|');
            if (hasSkipLine && rows.Count > 1)
                rows.RemoveAt(1);

            return CSVSerializer.Deserialize(rows, type);
        }
        catch (Exception e)
        {
            Debug.LogError($"CSV 역직렬화 실패: {e.Message}");
            throw;
        }
    }
    public async UniTask<string> LoadTableDataAsync(string tableName)
    {
#if DEV
        string path = Path.Combine(LOCAL_CSV_PATH, $"{tableName}.csv");
        return await Util.LoadFromFileAsync(path);
#else
        return Resources.Load<TextAsset>(Path.Combine("Data", tableName))?.text;
#endif
    }
}

