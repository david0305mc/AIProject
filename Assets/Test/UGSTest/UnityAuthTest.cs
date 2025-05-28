using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Unity.Services.CloudSave;
using UniRx;
using Unity.Services.CloudCode;
public class UnityAuthTest : MonoBehaviour
{
    [SerializeField] TMP_InputField idInputField;
    [SerializeField] TMP_InputField passwordInputField;

    [SerializeField] TextMeshProUGUI PointText;
    [SerializeField] TextMeshProUGUI PlayerIDText;

    [SerializeField] Button signUpButton;
    [SerializeField] Button signInButton;

    [SerializeField] Button signInGPGS;
    [SerializeField] Button linkGPGS;
    [SerializeField] Button unLinkGPGS;
    [SerializeField] Button guestLogin;

    [SerializeField] Button saveButton;
    [SerializeField] Button loadButton;
    [SerializeField] Button IncreasePointButton;
    [SerializeField] Button cloudCodeButton;
    class GiveLoginBonus
    {
        public int roll;
        public int sides;
    };

    class RequestServerTime
    {
        public long timestamp;
        public string formattedDate;
    };

    private
    async void Awake()
    {
        try
        {
            await AuthManager.Instance.InitializeAsync();
            AuthManager.Instance.InitGPGS();
            await AuthManager.Instance.SignInCachedUserAsync();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }

        Observable.CombineLatest(AuthManager.Instance.IsGPGSSignIned, AuthManager.Instance.IsGuestSignIned, (isGPGS, isGuest) => new { isGPGS, isGuest })
        .Subscribe(status =>
        {
            // GPGS에 로그인했을 경우
            guestLogin.gameObject.SetActive(!status.isGPGS && !status.isGuest);
            signInGPGS.gameObject.SetActive(!status.isGPGS && !status.isGuest);
            linkGPGS.gameObject.SetActive(status.isGuest && !status.isGPGS); // 익명 로그인 중일 때만 연동 가능
            unLinkGPGS.gameObject.SetActive(status.isGPGS); // GPGS 로그인된 상태에서만 해제 가능
        })
        .AddTo(this); // 또는 gameObject

        AuthManager.Instance.PlayerID.Subscribe(id =>
        {
            PlayerIDText.SetText(id);
        }).AddTo(gameObject);

        signUpButton.onClick.AddListener(async () =>
        {
            if (idInputField.text.Length <= 3)
            {
                Debug.LogError("id must > 4");
                return;
            }
            if (passwordInputField.text.Length <= 3)
            {
                Debug.LogError("password must > 4");
                return;
            }
            await AuthManager.Instance.SignUpWithUsernamePasswordAsync(idInputField.text, passwordInputField.text);
        });
        signInButton.onClick.AddListener(async () =>
        {
            if (idInputField.text.Length <= 3)
            {
                Debug.LogError("id must > 4");
                return;
            }
            if (passwordInputField.text.Length <= 3)
            {
                Debug.LogError("password must > 4");
                return;
            }
            await AuthManager.Instance.SignInWithUsernamePasswordAsync(idInputField.text, passwordInputField.text);
        });

        signInGPGS.onClick.AddListener(async () =>
        {
            if (AuthManager.Instance.IsGPGSSignIned.Value)
            {
                Debug.LogWarning("이미 로그인되어 있습니다.");
                return;
            }
            signInGPGS.interactable = false;
            try
            {
                string code = await AuthManager.Instance.LoginGooglePlayGames();
                if (!string.IsNullOrEmpty(code))
                {
                    await AuthManager.Instance.SignInWithGooglePlayGamesAsync(code);
                }
                else
                {
                    Debug.LogWarning("Google Play Games login failed or was canceled.");
                }
            }
            finally
            {
                signInGPGS.interactable = true;
            }

        });

        linkGPGS.onClick.AddListener(async () =>
        {
            if (AuthManager.Instance.IsGPGSSignIned.Value)
            {
                Debug.LogWarning("이미 로그인되어 있습니다.");
                return;
            }
            try
            {
                linkGPGS.interactable = false;
                string code = await AuthManager.Instance.LoginGooglePlayGames();
                if (!string.IsNullOrEmpty(code))
                {
                    await AuthManager.Instance.LinkWithGooglePlayGamesAsync(code);
                }
                else
                {
                    Debug.LogWarning("Google Play Games login failed or was canceled.");
                }
            }
            finally
            {
                linkGPGS.interactable = true;
            }
        });
        guestLogin.onClick.AddListener(async () =>
        {
            await AuthManager.Instance.SignInAnonymouslyAsync();
        });

        unLinkGPGS.onClick.AddListener(async () =>
        {
            await AuthManager.Instance.UnlinkGooglePlayGamesAsync();
        });

        saveButton.onClick.AddListener(() =>
        {
            SaveSomeData();
        });
        loadButton.onClick.AddListener(() =>
        {
            LoadSomeData();
        });
        IncreasePointButton.onClick.AddListener(async () =>
        {
            // UserDataManager.Instance.PlayPoint.Value++;
            UserDataManager.Instance.ReactivePlayerData.SparklingInt.Value++;
            var saveData = UserDataManager.Instance.ToPlayerSaveData();
            await AuthManager.Instance.ForceSaveObjectData("Save01", saveData);

        });
        cloudCodeButton.onClick.AddListener(async () =>
        {
            // try
            // {

            //     var result = await CloudCodeService.Instance.CallEndpointAsync<GiveLoginBonus>("GiveLoginBonus", new());
            //     Debug.Log($"result : {result.roll} {result.sides}");
            // }
            // catch (CloudCodeException e)
            // {
            //     Debug.LogError($"Cloud Code 에러 발생: {e.Message}");
            // }
            try
            {
                var result = await CloudCodeService.Instance.CallEndpointAsync<RequestServerTime>("RequestServerTime", new());
                Debug.Log($"result : {result.timestamp} {result.formattedDate}");
            }
            catch (CloudCodeException e)
            {
                Debug.LogError($"Cloud Code 에러 발생: {e.Message}");
            }

        });

        UserDataManager.Instance.ReactivePlayerData.SparklingInt.Subscribe(point =>
                {
                    PointText.SetText(point.ToString());
                });
        // UserDataManager.Instance.PlayPoint.Subscribe(point =>
        // {
        //     PointText.SetText(point.ToString());
        // });
    }

    // private void UpdateUI()
    // {
    //     PointText.SetText(UserDataManager.Instance.PlayerSaveData.SparklingInt.ToString());
    // }

    public async void SaveSomeData()
{
    var data = new Dictionary<string, object> { { "key", "someValue" } };
    await CloudSaveService.Instance.Data.Player.SaveAsync(data);
}
public async void LoadSomeData()
{
    var savedData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { "key" });

    Debug.Log("Done: " + savedData["key"]);
}
private async UniTask<T> RetrieveSpecificData<T>(string key)
{
    try
    {
        var results = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { key });

        if (results.TryGetValue(key, out var item))
        {
            return item.Value.GetAs<T>();
        }
        else
        {
            Debug.Log($"There is no such key as {key}!");
        }
    }
    catch (CloudSaveValidationException e)
    {
        Debug.LogError(e);
    }
    catch (CloudSaveRateLimitedException e)
    {
        Debug.LogError(e);
    }
    catch (CloudSaveException e)
    {
        Debug.LogError(e);
    }

    return default;
}
}
[Serializable]
public class SampleItem
{
    public string SophisticatedString;
    public int SparklingInt;
    public float AmazingFloat;
}

