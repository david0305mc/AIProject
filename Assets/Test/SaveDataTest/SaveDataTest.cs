using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

namespace SaveData
{
    [System.Serializable]
    public class BaseData
    {
        public long uidSeed = 1000;
        public long userUID = -1;

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
    }

    public class UserDataManager : Singleton<UserDataManager>
    {
        public UserData UserData { get; set; } = new UserData();

        // 각 필드별 dirty 관리
        private Dictionary<string, bool> dirtyFlags = new Dictionary<string, bool>();
        private string dataPath => Application.persistentDataPath;

        protected override void init()
        {
            base.init();
        }

        public void LoadAllData()
        {
            var fields = typeof(UserData).GetFields();
            foreach (var field in fields)
            {
                string fileName = Path.Combine(dataPath, field.Name + ".json");
                if (File.Exists(fileName))
                {
                    var value = JsonConvert.DeserializeObject(File.ReadAllText(fileName), field.FieldType);
                    field.SetValue(UserData, value);
                }
                // dirty 초기화
                dirtyFlags[field.Name] = false;
            }
        }

        public void SaveAllData()
        {
            var fields = typeof(UserData).GetFields();
            foreach (var field in fields)
            {
                if (dirtyFlags.TryGetValue(field.Name, out bool isDirty) && isDirty)
                {
                    string fileName = Path.Combine(dataPath, field.Name + ".json");
                    var value = field.GetValue(UserData);
                    File.WriteAllText(fileName, JsonConvert.SerializeObject(value, Formatting.Indented));
                    dirtyFlags[field.Name] = false;
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

        
    }


}
