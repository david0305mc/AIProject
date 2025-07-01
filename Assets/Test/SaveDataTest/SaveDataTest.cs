using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using UniRx;
using System.Linq;
using System.Reflection;

using Random = UnityEngine.Random;
using TMPro;

namespace SaveData
{
    [System.Serializable]
    public class BaseData
    {
        public long uidSeed = 1000;
        public long userUID = -1;

        public ReactiveProperty<long> Gold { get; private set; } = new ReactiveProperty<long>(-1);

        public long GetNextUID()
        {
            return uidSeed++;
        }
    }

    [System.Serializable]
    public class UserData
    {
        public BaseData BaseData = new BaseData();
        public Dictionary<int, int> ItemCount = new Dictionary<int, int>();
        public Dictionary<int, QuestData> QuestData { get; set; } = new Dictionary<int, QuestData>();

        public long GetNextUID()
        {
            return BaseData.GetNextUID();
        }
    }

    [System.Serializable]
    public class QuestData
    {
        public int id;
        public int progress;
        public bool isCompleted;
        public ReactiveProperty<int> CompleteCount { get; private set; }
    }

    public class UserDataManager : Singleton<UserDataManager>
    {
        public UserData UserData { get; set; } = new UserData();
        private Dictionary<string, bool> dirtyFlags = new Dictionary<string, bool>();
        private string dataPath => Application.persistentDataPath;


        public void LoadAllData()
        {
            var members = typeof(UserData)
                .GetMembers(BindingFlags.Public | BindingFlags.Instance)
                .Where(m => m.MemberType == MemberTypes.Field || m.MemberType == MemberTypes.Property);

            foreach (var member in members)
            {
                Type type;
                string name = member.Name;

                if (member is FieldInfo field)
                {
                    type = field.FieldType;
                }
                else if (member is PropertyInfo prop && prop.CanRead && prop.CanWrite)
                {
                    type = prop.PropertyType;
                }
                else
                {
                    continue;
                }

                string fileName = Path.Combine(dataPath, $"{name}.json");
                if (!File.Exists(fileName)) continue;

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ReactiveProperty<>))
                {
                    var valueType = type.GetGenericArguments()[0];
                    var raw = JsonConvert.DeserializeObject(File.ReadAllText(fileName), valueType);
                    var reactive = Activator.CreateInstance(type, raw);
                    if (member is FieldInfo f)
                        f.SetValue(UserData, reactive);
                    else if (member is PropertyInfo p)
                        p.SetValue(UserData, reactive);
                }
                else
                {
                    var value = JsonConvert.DeserializeObject(File.ReadAllText(fileName), type);
                    if (member is FieldInfo f)
                        f.SetValue(UserData, value);
                    else if (member is PropertyInfo p)
                        p.SetValue(UserData, value);
                }
            }
        }

        // 필드명으로 dirty 처리
        public void MarkDirty(string fieldName)
        {
            dirtyFlags[fieldName] = true;
        }
        public void CreateNewUser()
        {
            UserData.BaseData.userUID = UserData.GetNextUID();
            MarkDirty(nameof(UserData.BaseData));
            SaveData(true);
        }


        // 각 필드/프로퍼티별로 1개 파일 저장
        public void SaveData(bool forceSave = false)
        {
            if (!forceSave && dirtyFlags.Count == 0)
                return;

            var members = typeof(UserData)
                .GetMembers(BindingFlags.Public | BindingFlags.Instance)
                .Where(m => m.MemberType == MemberTypes.Field || m.MemberType == MemberTypes.Property);

            foreach (var member in members)
            {
                string name = member.Name;
                if(!forceSave && (!dirtyFlags.ContainsKey(name) || !dirtyFlags[name]))
                {
                    continue;
                }

                Type type;
                object value;

                if (member is FieldInfo field)
                {
                    type = field.FieldType;
                    value = field.GetValue(UserData);
                }
                else if (member is PropertyInfo prop && prop.CanRead && prop.CanWrite)
                {
                    type = prop.PropertyType;
                    value = prop.GetValue(UserData);
                }
                else
                {
                    continue;
                }

                string fileName = Path.Combine(dataPath, $"{name}.json");

                // ReactiveProperty 처리
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ReactiveProperty<>))
                {
                    var valueProp = type.GetProperty("Value");
                    var raw = valueProp.GetValue(value);
                    File.WriteAllText(fileName, JsonConvert.SerializeObject(raw, Formatting.Indented));
                }
                else
                {
                    File.WriteAllText(fileName, JsonConvert.SerializeObject(value, Formatting.Indented));
                }
            }
        }
    }


    public class SaveDataTest : MonoBehaviour
    {

        [SerializeField] private TextMeshProUGUI saveDataText;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            UserDataManager.Instance.LoadAllData();
            var userData = UserDataManager.Instance.UserData;
            if (userData == null || userData.BaseData.userUID == -1)
            {
                Debug.Log("No existing user data found. Creating new user UID.");
                UserDataManager.Instance.UserData = new UserData();
                UserDataManager.Instance.CreateNewUser();
            }
            else
            {
                Debug.Log($"Existing User UID: {userData.BaseData.userUID}");
            }
            UpdateSaveDataText();
            UserDataManager.Instance.UserData.BaseData.Gold.Subscribe(_ => UpdateSaveDataText()).AddTo(this);
        }
        public void OnClickAddItem()
        {
            int itemId = Random.Range(1, 100);
            int itemCount = Random.Range(1, 10);
            if (UserDataManager.Instance.UserData.ItemCount.ContainsKey(itemId))
            {
                UserDataManager.Instance.UserData.ItemCount[itemId] += itemCount;
            }
            else
            {
                UserDataManager.Instance.UserData.ItemCount[itemId] = itemCount;
            }
            UserDataManager.Instance.MarkDirty(nameof(UserData.ItemCount));
            UserDataManager.Instance.SaveData();
            Debug.Log($"Item {itemId} added. New count: {UserDataManager.Instance.UserData.ItemCount[itemId]}");
            UpdateSaveDataText();
        }
        public void OnClickAddQuest()
        {
            int questId = Random.Range(1, 100);
            if (UserDataManager.Instance.UserData.QuestData.ContainsKey(questId))
            {
                UserDataManager.Instance.UserData.QuestData[questId].progress += 1;
            }
            else
            {
                UserDataManager.Instance.UserData.QuestData[questId] = new QuestData { id = questId, progress = 0, isCompleted = false };
            }
            UserDataManager.Instance.MarkDirty(nameof(UserData.QuestData));
            UserDataManager.Instance.SaveData();
            Debug.Log($"Quest {questId} added. New progress: {UserDataManager.Instance.UserData.QuestData[questId].progress}");
            UpdateSaveDataText();
        }

        public void OnClickAddGold()
        {
            long goldToAdd = Random.Range(100, 1000);
            UserDataManager.Instance.UserData.BaseData.Gold.Value += goldToAdd;
            UserDataManager.Instance.MarkDirty(nameof(UserData.BaseData));
            UserDataManager.Instance.SaveData();
            Debug.Log($"Gold added: {goldToAdd}. New total: {UserDataManager.Instance.UserData.BaseData.Gold.Value}");
        }

        public void UpdateSaveDataText()
        {
            var ud = UserDataManager.Instance.UserData;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine($"UserUID: {ud.BaseData.userUID}");
            sb.AppendLine($"Gold: {ud.BaseData.Gold.Value}");
            sb.AppendLine("--- ItemCount ---");
            foreach (var kv in ud.ItemCount)
                sb.AppendLine($"ItemID: {kv.Key}, Count: {kv.Value}");
            sb.AppendLine("--- QuestData ---");
            foreach (var kv in ud.QuestData)
            {
                var q = kv.Value;
                sb.AppendLine($"QuestID: {q.id}, Progress: {q.progress}, Complete: {q.isCompleted}, CompleteCount: {(q.CompleteCount != null ? q.CompleteCount.Value.ToString() : "null")}");
            }
            if (saveDataText != null)
                saveDataText.text = sb.ToString();
            else
                Debug.Log(sb.ToString());
        }
    }


}
