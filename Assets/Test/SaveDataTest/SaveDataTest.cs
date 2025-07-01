using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using UniRx;
using System.Linq;
using System.Reflection;

using Random = UnityEngine.Random;

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
        public Dictionary<int, int> SkillLevels = new Dictionary<int, int>();
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

        // 각 필드/프로퍼티별로 1개 파일 저장
        public void SaveAllData()
        {
            var members = typeof(UserData)
                .GetMembers(BindingFlags.Public | BindingFlags.Instance)
                .Where(m => m.MemberType == MemberTypes.Field || m.MemberType == MemberTypes.Property);

            foreach (var member in members)
            {
                Type type;
                object value;
                string name = member.Name;

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
            SaveAllData();
        }
    }


    public class SaveDataTest : MonoBehaviour
    {
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
                UserDataManager.Instance.SaveAllData();
            }
            else
            {
                Debug.Log($"Existing User UID: {userData.BaseData.userUID}");
            }
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
            UserDataManager.Instance.SaveAllData();
            Debug.Log($"Item {itemId} added. New count: {UserDataManager.Instance.UserData.ItemCount[itemId]}");
        }
        public void OnClickAddSkill()
        {
            int skillId = Random.Range(1, 100);
            int skillLevel = Random.Range(1, 5);
            if (UserDataManager.Instance.UserData.SkillLevels.ContainsKey(skillId))
            {
                UserDataManager.Instance.UserData.SkillLevels[skillId] += skillLevel;
            }
            else
            {
                UserDataManager.Instance.UserData.SkillLevels[skillId] = skillLevel;
            }
            UserDataManager.Instance.MarkDirty(nameof(UserData.SkillLevels));
            UserDataManager.Instance.SaveAllData();
            Debug.Log($"Skill {skillId} added. New level: {UserDataManager.Instance.UserData.SkillLevels[skillId]}");
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
            UserDataManager.Instance.SaveAllData();
            Debug.Log($"Quest {questId} added. New progress: {UserDataManager.Instance.UserData.QuestData[questId].progress}");
        }

        public void OnClickAddGold()
        {
            long goldToAdd = Random.Range(100, 1000);
            UserDataManager.Instance.UserData.BaseData.Gold.Value += goldToAdd;
            UserDataManager.Instance.MarkDirty(nameof(UserData.BaseData.Gold));
            UserDataManager.Instance.SaveAllData();
            Debug.Log($"Gold added: {goldToAdd}. New total: {UserDataManager.Instance.UserData.BaseData.Gold.Value}");
        }


    }


}
