using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using TMPro;
public class GpgsManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI logText;



    public void LoginGPGS()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);

    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            var displayName = PlayGamesPlatform.Instance.GetUserDisplayName();
            var userID = PlayGamesPlatform.Instance.GetUserId();

            logText.text = $"Login Success {displayName}";
        }
        else
        {
            logText.text = $"Login Failed";
        }
    }
}
