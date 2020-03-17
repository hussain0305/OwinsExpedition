using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameController : MonoBehaviour
{
    public static GameController GameControl;
    public AudioClip ButtonClick;

    public int Revives;
    public long BestDistance;
    public long BestScore;
    public long BestKills;
    public long TotalDistance;
    public long TotalKills;
    public long TotalScore;
    public int Deaths;

    private AudioSource ButtonSound;

    private void Awake()
    {
        if (!GameControl)
        {
            DontDestroyOnLoad(gameObject);
            GameControl = this;
        }
        else if (GameControl != this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadGameStats();
        ButtonSound = gameObject.AddComponent<AudioSource>();
        ButtonSound.loop = false;
        ButtonSound.clip = ButtonClick;
    }

    public void SaveGameStats()
    {
        BinaryFormatter BF = new BinaryFormatter();
        FileStream FS = File.Create(Application.persistentDataPath + "/RATSFSaveData.dat");

        PlayerData SessionData = new PlayerData();

        SessionData.Revives = Revives;
        SessionData.BestDistance = BestDistance;
        SessionData.BestKills = BestKills;
        SessionData.BestScore = BestScore;
        SessionData.TotalDistance = TotalDistance;
        SessionData.TotalKills = TotalKills;
        SessionData.TotalScore = TotalScore;
        SessionData.Deaths = Deaths;

        BF.Serialize(FS, SessionData);
        FS.Close();
    }

    public void LoadGameStats()
    {
        if(File.Exists(Application.persistentDataPath + "/RATSFSaveData.dat"))
        {
            BinaryFormatter BF = new BinaryFormatter();
            FileStream FS = File.Open(Application.persistentDataPath + "/RATSFSaveData.dat", FileMode.Open);

            PlayerData FetchedData = (PlayerData)BF.Deserialize(FS);

            FS.Close();

            Revives = FetchedData.Revives;
            BestDistance = FetchedData.BestDistance;
            BestScore = FetchedData.BestScore;
            BestKills = FetchedData.BestKills;
            TotalDistance = FetchedData.TotalDistance;
            TotalKills = FetchedData.TotalKills;
            TotalScore = FetchedData.TotalScore;
            Deaths = FetchedData.Deaths;

        }
    }

    public void RevivesAdded(int Amount)
    {
        GameController.GameControl.Revives += Amount;
        GameController.GameControl.SaveGameStats();
    }

    public bool RevivesSpent(int Amount)
    {
        if (GameController.GameControl.Revives < Amount)
        {
            return false;
        }
        else
        {
            GameController.GameControl.Revives -= Amount;
            GameController.GameControl.SaveGameStats();
            return true;
        }
    }

    public void PlayButtonSound()
    {
        if(!ButtonSound.isPlaying)
            ButtonSound.Play();
    }

    public void PostRunStats(long CurrentDistance, long Currentscore, int CurrentKills)
    {
        if(GameController.GameControl.BestDistance < CurrentDistance)
        {
            GameController.GameControl.BestDistance = CurrentDistance;
        }

        if (GameController.GameControl.BestScore < Currentscore)
        {
            GameController.GameControl.BestScore = Currentscore;
        }

        if (GameController.GameControl.BestKills < CurrentKills)
        {
            GameController.GameControl.BestKills = CurrentKills;
        }

        GameController.GameControl.TotalDistance += CurrentDistance;
        GameController.GameControl.TotalKills += CurrentKills;
        GameController.GameControl.TotalScore += Currentscore;
        GameController.GameControl.Deaths++;

        SaveGameStats();
        if(PlayerPrefs.GetInt("EasyDifficulty", 0) != 1){
            GooglePlayAuthenticator.PostDistanceToLeaderboard(CurrentDistance);
            GooglePlayAuthenticator.PostScoreToLeaderboard(Currentscore);
            GooglePlayAuthenticator.PostKillsToLeaderboard(CurrentKills);
        }
    }

    public long GetBestDistance()
    {
        return BestDistance;
    }

    public long GetBestScore()
    {
        return BestScore;
    }

    public long GetBestKills()
    {
        return BestKills;
    }

    public long GetTotalKills()
    {
        return TotalKills;
    }

    public long GetTotalDistance()
    {
        return TotalDistance;
    }

    public long GetTotalScore()
    {
        return TotalScore;
    }

    public int GetRevives()
    {
        return Revives;
    }

    public int GetDeaths()
    {
        return Deaths;
    }

}

[Serializable]
class PlayerData
{
    public int Revives;
    public long BestDistance;
    public long BestScore;
    public long BestKills;
    public long TotalDistance;
    public long TotalKills;
    public long TotalScore;
    public int Deaths;
}