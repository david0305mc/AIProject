using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Security.Cryptography;
using System.Linq;

public static class Util
{
    public static T MakeGameObject<T>(string objectName, Transform parent) where T : MonoBehaviour
    {
        return MakeGameObject(objectName, parent).AddComponent<T>();
    }

    public static GameObject MakeGameObject(string objectName, Transform parent)
    {
        var tempObject = new GameObject(objectName);
        if (parent != null)
        {
            tempObject.transform.SetParent(parent);
        }
        return tempObject;
    }

    public static void SetLayerRecursively(GameObject obj, int layer)
    {
        if (obj == null) return;

        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }

    public static T InstantiateGameObject<T>(GameObject prefab, Transform parent = null) where T : MonoBehaviour
    {
        return InstantiateGameObject(prefab, parent).GetComponent<T>();
    }

    public static GameObject InstantiateGameObject(GameObject prefab, Transform parent = null)
    {
        if (prefab == null) return null;

        var go = GameObject.Instantiate(prefab, parent, false);
        go.name = prefab.name;
        return go;
    }

    public static void QuitApp()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public static string LoadFromFile(string filePath)
    {
        if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            return string.Empty;

        try
        {
            using var fs = new FileStream(filePath, FileMode.Open);
            using var sr = new StreamReader(fs);
            return sr.ReadToEnd();
        }
        catch (Exception e)
        {
            Debug.LogError($"파일 로드 중 오류 발생: {e.Message}");
            return string.Empty;
        }
    }

    public static void SaveFile(string filePath, string data)
    {
        if (string.IsNullOrEmpty(filePath)) return;

        try
        {
            var directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            using var fs = new FileStream(filePath, FileMode.Create);
            using var writer = new StreamWriter(fs);
            writer.Write(data);
        }
        catch (Exception e)
        {
            Debug.LogError($"파일 저장 중 오류 발생: {e.Message}");
        }
    }

    public static async UniTask<string> LoadFromFileAsync(string filePath)
    {
        if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            return string.Empty;

        try
        {
            using var fs = new FileStream(filePath, FileMode.Open);
            using var sr = new StreamReader(fs);
            return await sr.ReadToEndAsync();
        }
        catch (Exception e)
        {
            Debug.LogError($"파일 비동기 로드 중 오류 발생: {e.Message}");
            return string.Empty;
        }
    }

    public static async UniTask SaveFileAsync(string filePath, string data)
    {
        if (string.IsNullOrEmpty(filePath)) return;

        try
        {
            var directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            using var fs = new FileStream(filePath, FileMode.Create);
            using var writer = new StreamWriter(fs);
            await writer.WriteAsync(data);
        }
        catch (Exception e)
        {
            Debug.LogError($"파일 비동기 저장 중 오류 발생: {e.Message}");
        }
    }

    private static readonly string EncryptionKey = "SecureKey123";

    public static string EncryptXOR(string data)
    {
        if (string.IsNullOrEmpty(data)) return string.Empty;

        var result = new StringBuilder();
        for (int i = 0; i < data.Length; i++)
        {
            result.Append((char)(data[i] ^ EncryptionKey[i % EncryptionKey.Length]));
        }
        return result.ToString();
    }

    public static T Load<T>(string path) where T : UnityEngine.Object
    {
        if (string.IsNullOrEmpty(path)) return null;
        return Resources.Load<T>(path);
    }

    public static void CopyAll<T>(T target, T source) where T : new()
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (target == null)
            throw new ArgumentNullException(nameof(target));

        var type = typeof(T);

        var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        foreach (var field in fields)
        {
            try
            {
                var value = field.GetValue(source);
                field.SetValue(target, value);
            }
            catch (Exception e)
            {
                Debug.LogError($"필드 복사 중 오류 발생: {e.Message}");
            }
        }

        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var property in properties)
        {
            if (!property.CanWrite) continue;

            try
            {
                var value = property.GetValue(source);
                property.SetValue(target, value);
            }
            catch (Exception e)
            {
                Debug.LogError($"프로퍼티 복사 중 오류 발생: {e.Message}");
            }
        }
    }

    public static T MergeFields<T>(T target, T source) where T : class, new()
    {
        if (target == null) throw new ArgumentNullException(nameof(target));
        if (source == null) throw new ArgumentNullException(nameof(source));

        var type = typeof(T);
        var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

        foreach (var field in fields)
        {
            var sourceValue = field.GetValue(source);
            if (sourceValue == null) continue;

            var targetValue = field.GetValue(target);

            if (IsDictionaryType(field.FieldType))
            {
                MergeDictionaries(targetValue, sourceValue);
            }
            else
            {
                field.SetValue(target, sourceValue);
            }
        }

        return target;
    }

    private static bool IsDictionaryType(Type type)
    {
        return type.IsGenericType && (
            type.GetGenericTypeDefinition() == typeof(Dictionary<,>) ||
            type.GetGenericTypeDefinition() == typeof(SerializableDictionary<,>)
        );
    }

    private static void MergeDictionaries(object targetValue, object sourceValue)
    {
        if (targetValue is IDictionary targetDict && sourceValue is IDictionary sourceDict)
        {
            foreach (var key in sourceDict.Keys)
            {
                targetDict[key] = sourceDict[key];
            }
        }
    }

    public static string ComputeMD5Hash(string input)
    {
        if (string.IsNullOrEmpty(input)) return string.Empty;

        using var md5 = MD5.Create();
        var inputBytes = Encoding.UTF8.GetBytes(input);
        var hashBytes = md5.ComputeHash(inputBytes);

        var sb = new StringBuilder();
        foreach (var b in hashBytes)
        {
            sb.Append(b.ToString("x2"));
        }
        return sb.ToString();
    }

    public static string GenerateAesSessionKey()
    {
        using var aes = Aes.Create();
        aes.GenerateKey();
        return Convert.ToBase64String(aes.Key);
    }

    public static RaycastHit2D? Get2DHitObject()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var mousePos2d = new Vector2(mousePos.x, mousePos.y);

        var hits = Physics2D.RaycastAll(mousePos2d, Vector2.zero);
        return hits.Length > 0
            ? hits.OrderByDescending(i => i.collider.transform.position.z).FirstOrDefault()
            : null;
    }

    public static T StringToEnum<T>(string value) where T : Enum
    {
        if (string.IsNullOrEmpty(value))
            throw new ArgumentNullException(nameof(value));

        return (T)Enum.Parse(typeof(T), value);
    }

    public static T IntToEnum<T>(int value) where T : Enum
    {
        if (!Enum.IsDefined(typeof(T), value))
            throw new ArgumentException($"값 {value}는 열거형 {typeof(T).Name}에 정의되지 않았습니다.");

        return (T)(object)value;
    }
}

public static class RaycastUtilities
{
    private static readonly PointerEventData s_pointerEventData;
    private static readonly List<RaycastResult> s_raycastResults;

    static RaycastUtilities()
    {
        s_pointerEventData = new PointerEventData(EventSystem.current);
        s_raycastResults = new List<RaycastResult>();
    }

    public static bool PointerIsOverUI(Vector2 screenPos)
    {
        var hitObject = UIRaycast(ScreenPosToPointerData(screenPos));
        return hitObject != null && hitObject.layer == LayerMask.NameToLayer("UI");
    }

    public static GameObject UIRaycast(PointerEventData pointerData)
    {
        if (EventSystem.current == null) return null;

        s_raycastResults.Clear();
        EventSystem.current.RaycastAll(pointerData, s_raycastResults);

        return s_raycastResults.Count < 1 ? null : s_raycastResults[0].gameObject;
    }

    private static PointerEventData ScreenPosToPointerData(Vector2 screenPos)
    {
        s_pointerEventData.position = screenPos;
        return s_pointerEventData;
    }

    public static GameObject UIRaycast(PointerEventData pointerData, int layerMask)
    {
        if (EventSystem.current == null) return null;

        s_raycastResults.Clear();
        EventSystem.current.RaycastAll(pointerData, s_raycastResults);

        var result = s_raycastResults.Find(x => x.gameObject.layer == layerMask);
        return !result.Equals(default) ? result.gameObject : null;
    }
}

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        if (string.IsNullOrEmpty(json)) return Array.Empty<T>();

        try
        {
            var wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper?.data ?? Array.Empty<T>();
        }
        catch (Exception e)
        {
            Debug.LogError($"JSON 파싱 중 오류 발생: {e.Message}");
            return Array.Empty<T>();
        }
    }

    public static string ToJson<T>(T[] array)
    {
        if (array == null) return string.Empty;

        try
        {
            var wrapper = new Wrapper<T> { data = array };
            return JsonUtility.ToJson(wrapper);
        }
        catch (Exception e)
        {
            Debug.LogError($"JSON 직렬화 중 오류 발생: {e.Message}");
            return string.Empty;
        }
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] data;
    }
}

public static class PathInfo
{
    private static string _dataPath = string.Empty;

    public static string DataPath
    {
        get
        {
            if (string.IsNullOrEmpty(_dataPath))
            {
                var sb = new StringBuilder();
                sb.Append(Application.isEditor
                    ? $"{Application.dataPath}/../Cache"
                    : Application.persistentDataPath);
                _dataPath = sb.ToString();
            }
            return _dataPath;
        }
    }

    public static Transform FindDeep(this Transform parent, string name)
    {
        if (string.IsNullOrEmpty(name) || parent == null) return null;

        if (parent.name == name)
            return parent;

        foreach (Transform child in parent)
        {
            var result = child.FindDeep(name);
            if (result != null)
                return result;
        }

        return null;
    }
}