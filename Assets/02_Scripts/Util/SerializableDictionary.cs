using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField]
    private List<TKey> keys = new();

    [SerializeField]
    private List<TValue> values = new();

    public void OnAfterDeserialize()
    {
        Clear();

        if (keys.Count != values.Count)
        {
            Debug.LogError($"직렬화 해제 중 오류 발생: keys({keys.Count})와 values({values.Count})의 개수가 일치하지 않습니다.");
            return;
        }

        for (int i = 0; i < keys.Count; i++)
        {
            try
            {
                Add(keys[i], values[i]);
            }
            catch (ArgumentException)
            {
                Debug.LogError($"중복된 키가 발견되었습니다: {keys[i]}");
            }
        }
    }

    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();

        foreach (var pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }
}
