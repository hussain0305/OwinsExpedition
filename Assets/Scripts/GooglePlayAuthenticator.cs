using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GooglePlayAuthenticator : MonoBehaviour
{
    public Color ConnectedColor;
    public Color DisconnectedColor;
    public Image ConnectionIndicator;
    // Start is called before the first frame update
    void Start()
    {
        AuthenticateUser();
    }
    
    void AuthenticateUser()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();
        Social.localUser.Authenticate((bool success) =>
        {
            if (success == true)
            {
                ConnectionIndicator.color = ConnectedColor;
            }
            else
            {
                ConnectionIndicator.color = DisconnectedColor;
            }
        });
    }

    public static void SwitchGoogleAccount()
    {
        PlayGamesPlatform.Instance.SignOut();
        new GooglePlayAuthenticator().AuthenticateUser();
    }

    public static void PostKillsToLeaderboard(int newKills)
    {
        Social.ReportScore(newKills, GPGSIds.leaderboard_kills, (bool success) =>
        {
            if (success)
            {

            }
            else
            {

            }
        });
    }

    public static void PostDistanceToLeaderboard(long newDistance)
    {
        Social.ReportScore(newDistance, GPGSIds.leaderboard_distance, (bool success) =>
        {
            if (success)
            {

            }
            else
            {

            }
        });
    }

    public static void PostScoreToLeaderboard(long newScore)
    {
        Social.ReportScore(newScore, GPGSIds.leaderboard_score, (bool success) =>
        {
            if (success)
            {

            }
            else
            {

            }
        });
    }

    public static void ShowScoreLeaderboard()
    {
        PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_score);
    }

    public static void ShowKillsLeaderboard()
    {
        PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_kills);
    }

    public static void ShowDistanceLeaderboard()
    {
        PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_distance);
    }

    public static void RegisterAchievement(string AchieveID, float AchievePercentage)
    {
        Social.ReportProgress(AchieveID, AchievePercentage, (bool Status) =>
        {

        });
    }
}
