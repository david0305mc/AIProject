using UnityEngine;
using Unity.Services.Core;
using System;
using Unity.Services.Authentication;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
public class UnityAuthTest : MonoBehaviour
{
    [SerializeField] Button signInButton;
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
        signInButton.onClick.AddListener(async () =>
        {
            await SignInAnonymouslyAsync();
        });
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
}

