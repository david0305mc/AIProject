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
    [SerializeField] private TMP_InputField idInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private TextMeshProUGUI pointText;
    [SerializeField] private TextMeshProUGUI playerIDText;
    [SerializeField] private Button signUpButton;
    [SerializeField] private Button signInButton;
    [SerializeField] private Button signInGPGS;
    [SerializeField] private Button linkGPGS;
    [SerializeField] private Button unLinkGPGS;
    [SerializeField] private Button guestLogin;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button increasePointButton;
    [SerializeField] private Button cloudCodeButton;

    private class GiveLoginBonus
    {
        public int Roll;
        public int Sides;
    }

    private class RequestServerTime
    {
        public long Timestamp;
        public string FormattedDate;
    }

    private async void Awake()
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

        Observable.CombineLatest(
            AuthManager.Instance.IsGPGSSignIned,
            AuthManager.Instance.IsGuestSignIned,
            (isGPGS, isGuest) => new { isGPGS, isGuest })
            .Subscribe(status =>
            {
                guestLogin.gameObject.SetActive(!status.isGPGS && !status.isGuest);
                signInGPGS.gameObject.SetActive(!status.isGPGS && !status.isGuest);
                linkGPGS.gameObject.SetActive(status.isGuest && !status.isGPGS);
                unLinkGPGS.gameObject.SetActive(status.isGPGS);
            })
            .AddTo(this);

        AuthManager.Instance.PlayerID
            .Subscribe(id => playerIDText.SetText(id))
            .AddTo(gameObject);

        signUpButton.onClick.AddListener(async () =>
        {
            if (idInputField.text.Length <= 3 || passwordInputField.text.Length <= 3)
            {
                Debug.LogError("id/password must > 4");
                return;
            }
            await AuthManager.Instance.SignUpWithUsernamePasswordAsync(idInputField.text, passwordInputField.text);
        });

        signInButton.onClick.AddListener(async () =>
        {
            if (idInputField.text.Length <= 3 || passwordInputField.text.Length <= 3)
            {
                Debug.LogError("id/password must > 4");
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
                string code = await AuthManager.Instance.RequestAuthTokenFromGPGS();
                if (!string.IsNullOrEmpty(code))
                    await AuthManager.Instance.SignInWithGooglePlayGamesAsync(code);
                else
                    Debug.LogWarning("Google Play Games login failed or was canceled.");
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
                string code = await AuthManager.Instance.RequestAuthTokenFromGPGS();
                if (!string.IsNullOrEmpty(code))
                    await AuthManager.Instance.LinkWithGooglePlayGamesAsync(code);
                else
                    Debug.LogWarning("Google Play Games login failed or was canceled.");
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

        saveButton.onClick.AddListener(SaveSomeData);
        loadButton.onClick.AddListener(LoadSomeData);

        increasePointButton.onClick.AddListener(async () =>
        {
            UserDataManager.Instance.ReactivePlayerData.SparklingInt.Value++;
            var saveData = UserDataManager.Instance.ToPlayerSaveData();
            // await AuthManager.Instance.ForceSaveObjectData("Save01", saveData);
            await AuthManager.Instance.SaveWithSessionValidationAsync(saveData);
        });

        cloudCodeButton.onClick.AddListener(async () =>
        {
            try
            {
                var result = await CloudCodeService.Instance.CallEndpointAsync<RequestServerTime>("RequestServerTime", new());
                Debug.Log($"result : {result.Timestamp} {result.FormattedDate}");
            }
            catch (CloudCodeException e)
            {
                Debug.LogError($"Cloud Code 에러 발생: {e.Message}");
            }
        });

        UserDataManager.Instance.ReactivePlayerData.SparklingInt
            .Subscribe(point => pointText.SetText(point.ToString()));
    }

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
                return item.Value.GetAs<T>();
            Debug.Log($"There is no such key as {key}!");
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

