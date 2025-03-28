using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using UniRx;
using JetBrains.Annotations;

public class UIIntro : MonoBehaviour
{
    [SerializeField] private Button _buttonStart;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        Application.targetFrameRate = 120;
        _buttonStart.onClick.AddListener(() => StartGame().Forget());
    }

    private async UniTask StartGame()
    {
        await DataManager.Instance.LoadDataAsync();
        await DataManager.Instance.LoadConfigTable();
        foreach (var item in DataManager.Instance.LevelDic)
        {
            Debug.Log(item.Key);
        }
        var mainSceneAsync = SceneManager.LoadSceneAsync("Main");
        await UniTask.WaitUntil(() => mainSceneAsync.isDone);
    }

}
