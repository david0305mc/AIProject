using UnityEngine;
using Unity.Services.Core;
using System;
using Unity.Services.Authentication;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Unity.Services.CloudSave;
public class UnityAuthTest : MonoBehaviour
{
    [SerializeField] TMP_InputField idInputField;
    [SerializeField] TMP_InputField passwordInputField;

    [SerializeField] Button signUpButton;
    [SerializeField] Button signInButton;

    [SerializeField] Button saveButton;
    [SerializeField] Button loadButton;
    private
    async void Awake()
    {
        try
        {
            await UnityServices.InitializeAsync();
            SetupEvents();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }

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
            await SignUpWithUsernamePasswordAsync(idInputField.text, passwordInputField.text);
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
            await SignInWithUsernamePasswordAsync(idInputField.text, passwordInputField.text);
        });
        saveButton.onClick.AddListener(() =>
        {
            SaveSomeData();
        });
        loadButton.onClick.AddListener(() =>
        {
            LoadSomeData();
        });
    }

    async UniTask SignUpWithUsernamePasswordAsync(string username, string password)
    {
        try
        {
            await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
            Debug.Log("SignUp is successful.");
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }
    async UniTask SignInWithUsernamePasswordAsync(string username, string password)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);
            Debug.Log("SignIn is successful.");
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }
    async UniTask SignInAnonymouslyAsync()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Sign in anonymously succeeded!");

            // Shows how to get the playerID
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }
    // Setup authentication event handlers if desired
    void SetupEvents()
    {
        AuthenticationService.Instance.SignedIn += () =>
        {
            // Shows how to get a playerID
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

            // Shows how to get an access token
            Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");

        };

        AuthenticationService.Instance.SignInFailed += (err) =>
        {
            Debug.LogError(err);
        };

        AuthenticationService.Instance.SignedOut += () =>
        {
            Debug.Log("Player signed out.");
        };

        AuthenticationService.Instance.Expired += () =>
          {
              Debug.Log("Player session could not be refreshed and expired.");
          };
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

