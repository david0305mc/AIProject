

using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UniRx;
using System.Linq;
using System.Collections.Generic;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models;
using UniRx;

[Serializable]
public class PlayerSaveData
{
    public string SophisticatedString;
    public int SparklingInt;
    public float AmazingFloat;

}

public class ReactivePlayerData
{
    public ReactiveProperty<string> SophisticatedString = new("Default");
    public ReactiveProperty<int> SparklingInt = new(0);
    public ReactiveProperty<float> AmazingFloat = new(0.0f);
}


public class AuthManager : Singleton<AuthManager>
{
    public ReactiveProperty<bool> IsGPGSSignIned;
    public ReactiveProperty<bool> IsGuestSignIned;
    public ReactiveProperty<string> PlayerID;

    public async UniTask InitializeAsync()
    {
        try
        {
            await UnityServices.InitializeAsync();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
        IsGPGSSignIned = new ReactiveProperty<bool>(false);
        IsGuestSignIned = new ReactiveProperty<bool>(false);
        PlayerID = new ReactiveProperty<string>("None");
        SetupEvents();
    }

    public void InitGPGS()
    {
        PlayGamesPlatform.Activate();
    }

    public UniTask<string> LoginGooglePlayGames()
    {
        var tcs = new UniTaskCompletionSource<string>();

        PlayGamesPlatform.Instance.Authenticate((status) =>
        {
            if (status == SignInStatus.Success)
            {
                Debug.Log("GPGS Login Success");

                PlayGamesPlatform.Instance.RequestServerSideAccess(true, code =>
                {
                    Debug.Log("Auth Code: " + code);
                    tcs.TrySetResult(code); // 반환 완료
                });
            }
            else
            {
                Debug.LogError("GPGS Login Failed");
                tcs.TrySetResult(string.Empty); // 실패 시 빈 문자열 반환
            }
        });
        return tcs.Task;
    }
    // Setup authentication event handlers if desired
    private void SetupEvents()
    {
        AuthenticationService.Instance.SignedIn += async () =>
        {
            // Shows how to get a playerID
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");
            PlayerID.Value = AuthenticationService.Instance.PlayerId;

            var playerInfo = await AuthenticationService.Instance.GetPlayerInfoAsync();

            foreach (var identity in playerInfo.Identities)
            {
                Debug.Log($"Identity Provider: {identity.TypeId}");
            }
            IsGPGSSignIned.Value = !string.IsNullOrEmpty(playerInfo.GetGooglePlayGamesId());
            IsGuestSignIned.Value = playerInfo.Identities.Count() == 0;

            // Shows how to get an access token
            Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");
            var test = await AuthenticationService.Instance.GetPlayerInfoAsync();

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

    private async UniTask LoadUserDataFromServer()
    {
        var data = await RetrieveSpecificData<PlayerSaveData>("Save01");
        if (data != null)
        {
            UserDataManager.Instance.LoadFrom(data);
        }
        else
        {
            Debug.LogWarning("No data found, applying fallback...");
        }
    }

    public async UniTask SignInAnonymouslyAsync()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Sign in anonymously succeeded!");

            // Shows how to get the playerID
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");
            await LoadUserDataFromServer();

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
    public async UniTask SignInWithGooglePlayGamesAsync(string authCode)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithGooglePlayGamesAsync(authCode);
            IsGPGSSignIned.Value = true;
            await LoadUserDataFromServer();

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
    public async UniTask LinkWithGooglePlayGamesAsync(string authCode)
    {
        try
        {
            await AuthenticationService.Instance.LinkWithGooglePlayGamesAsync(authCode);
            IsGPGSSignIned.Value = true;
            Debug.Log("Link is successful.");
        }
        catch (AuthenticationException ex) when (ex.ErrorCode == AuthenticationErrorCodes.AccountAlreadyLinked)
        {
            // Prompt the player with an error message.
            Debug.LogError("This user is already linked with another account. Log in instead.");
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
    public async UniTask UnlinkGooglePlayGamesAsync()
    {
        try
        {
            await AuthenticationService.Instance.UnlinkGooglePlayGamesAsync();
            IsGPGSSignIned.Value = false;
            Debug.Log("Unlink is successful.");
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

    public async UniTask SignInCachedUserAsync()
    {
        // Check if a cached player already exists by checking if the session token exists
        if (!AuthenticationService.Instance.SessionTokenExists)
        {
            // if not, then do nothing
            return;
        }
        await SignInAnonymouslyAsync();
    }
    public async UniTask SignUpWithUsernamePasswordAsync(string username, string password)
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
    public async UniTask SignInWithUsernamePasswordAsync(string username, string password)
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

    public async UniTask<string> ForceSaveObjectData<T>(string key, T value)
    {
        try
        {
            Dictionary<string, object> oneElement = new Dictionary<string, object>
        {
            { key, value }
        };

            Dictionary<string, string> result = await CloudSaveService.Instance.Data.Player.SaveAsync(oneElement);
            string writeLock = result[key];

            Debug.Log($"Successfully saved {key}:{JsonUtility.ToJson(value)} with updated write lock {writeLock}");
            return writeLock;
        }
        catch (CloudSaveException e)
        {
            Debug.LogError(e);
        }

        return null;
    }


    public async UniTask<string> SaveObjectData(string key, PlayerSaveData value, string writeLock)
    {
        try
        {
            // Although we are only saving a single value here, you can save multiple keys
            // and values in a single batch.
            // Use SaveItem to specify a write lock. The request will fail if the provided write lock
            // does not match the one currently saved on the server.
            Dictionary<string, SaveItem> oneElement = new Dictionary<string, SaveItem>
                {
                    { key, new SaveItem(value, writeLock) }
                };

            // Saving data with write lock validation by using a SaveItem with the write lock specified
            Dictionary<string, string> result = await CloudSaveService.Instance.Data.Player.SaveAsync(oneElement);
            string newWriteLock = result[key];

            Debug.Log($"Successfully saved {key}:{value} with updated write lock {newWriteLock}");

            return newWriteLock;
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

        return null;
    }
    public async UniTask<T> RetrieveSpecificData<T>(string key)
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

    public async UniTask RetrieveEverything()
    {
        try
        {
            // If you wish to load only a subset of keys rather than everything, you
            // can call a method LoadAsync and pass a HashSet of keys into it.
            var results = await CloudSaveService.Instance.Data.Player.LoadAllAsync();

            Debug.Log($"{results.Count} elements loaded!");

            foreach (var result in results)
            {
                Debug.Log($"Key: {result.Key}, Value: {result.Value.Value}");
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
    }

}
