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
    public class SkillData
    {
        public Dictionary<int, int> SkillLevels = new Dictionary<int, int>();
    }

    [System.Serializable]
    public class ItemData
    {
        public Dictionary<int, int> ItemCount = new Dictionary<int, int>();
    }

    public class UserDataManager : Singleton<UserDataManager>
    {
        public BaseData BaseData { get; set; } = new BaseData();
        public SkillData SkillData { get; set; } = new SkillData();
        public ItemData ItemData { get; set; } = new ItemData();

        private string savePath => Path.Combine(Application.persistentDataPath, "userdata.json");

        protected override void init()
        {
            base.init();
        }

        public BaseData LoadData()
        {
            if (!File.Exists(savePath))
                return null;

            string json = File.ReadAllText(savePath);
            var loadedContainer = JsonConvert.DeserializeObject<DataContainer>(json);
            this.BaseData = loadedContainer.BaseData;
            this.SkillData = loadedContainer.SkillData;

            return this.BaseData;
        }

        public void SaveData()
        {
            var container = new DataContainer()
            {
                BaseData = this.BaseData,
                SkillData = this.SkillData,
                ItemData = this.ItemData 
            };

            string json = JsonConvert.SerializeObject(container, Formatting.Indented);
            File.WriteAllText(savePath, json);
        }

        public void CreateNewUser()
        {
            this.BaseData.userUID = this.BaseData.GetNextUID();
            SaveData();
        }

        [System.Serializable]
        public class DataContainer
        {
            public BaseData BaseData;
            public SkillData SkillData;
            public ItemData ItemData; 
        }
    }


    public class SaveDataTest : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            // if first User
            var userData = UserDataManager.Instance.LoadData();
            if (userData == null || userData.userUID == -1)
            {
                Debug.Log("No existing user data found. Creating new user UID.");
                // Create a new BaseData instance
                UserDataManager.Instance.BaseData = new BaseData();
                UserDataManager.Instance.CreateNewUser();
            }
            else
            {
                Debug.Log($"Existing User UID: {userData.userUID}");
            }
        }

        public void OnClickAddItem()
        {
            // Add item with ID 1 and count 10
            int itemId = Random.Range(1, 100); // Random item ID for testing
            int itemCount = Random.Range(1, 10); // Random item count for testing

            if (UserDataManager.Instance.ItemData.ItemCount.ContainsKey(itemId))
            {
                UserDataManager.Instance.ItemData.ItemCount[itemId] += itemCount;
            }
            else
            {
                UserDataManager.Instance.ItemData.ItemCount[itemId] = itemCount;
            }

            UserDataManager.Instance.SaveData();
            Debug.Log($"Item {itemId} added. New count: {UserDataManager.Instance.ItemData.ItemCount[itemId]}");
        }

        public void OnClickAddSkill()
        {
            // Add skill with ID 1 and level 1
            int skillId = Random.Range(1, 100); // Random skill ID for testing
            int skillLevel = Random.Range(1, 5); // Random skill level for testing

            if (UserDataManager.Instance.SkillData.SkillLevels.ContainsKey(skillId))
            {
                UserDataManager.Instance.SkillData.SkillLevels[skillId] += skillLevel;
            }
            else
            {
                UserDataManager.Instance.SkillData.SkillLevels[skillId] = skillLevel;
            }

            UserDataManager.Instance.SaveData();
            Debug.Log($"Skill {skillId} added. New level: {UserDataManager.Instance.SkillData.SkillLevels[skillId]}");
        }

        
    }


}
