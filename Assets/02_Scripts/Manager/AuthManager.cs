

using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UniRx;
using System.Linq;

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

    public async UniTask SignInAnonymouslyAsync()
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
    public async UniTask SignInWithGooglePlayGamesAsync(string authCode)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithGooglePlayGamesAsync(authCode);
            IsGPGSSignIned.Value = true;
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
        SignInAnonymouslyAsync().Forget();
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

}
