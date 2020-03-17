using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerScoring : MonoBehaviour {

    public Color ScoreLabelColor;
    public Color KillLabelColor;
    public Color DistanceLabelColor;
    public Text ScoreLabel;
    public Text KillText;
    public GameObject ScoreObject;

    private int ScoreFromRunning;
    private int WMMultiplier;
    private int Kills = 0;
    private int KillsForRevive = 0;
    private int CurrentlyDisplaying = 0;
    private int ChangeDisplayAfterSeconds = 5;
    private long Score;
    private long DistanceCovered;
    private bool AllWeaponsUnlocked = false;
    private bool IceUnlocked = false;
    private bool FireUnlocked = false;
    private bool MagicUnlocked = false;
    private bool RunningIncrementScheduled;
    private bool[] ScoreAchievements;
    private float RunningScoreIncrementCycle;
    private float RunningScoreUpdateDuration;
    private Text UIScoreText;
    private Queue<int> ScoreSteps;
    private Coroutine ScoringRoutine;
    private UIInventory InventoryPage;
    private WorldManager Manager;

    // Use this for initialization
	void Start () {
        WMMultiplier = 1;
        Score = 0;
        DistanceCovered = 0;
        Kills = 0;
        RunningScoreIncrementCycle = 4;
        ScoreFromRunning = 1;
        RunningIncrementScheduled = false;
        RunningScoreUpdateDuration = 1;
        ScoreSteps = new Queue<int>(new[] { 5, 15, 25 });
        UIScoreText = ScoreObject.GetComponent<Text>();

        ScoringRoutine = StartCoroutine(RunningScore());
        StartCoroutine(LogDistance());
        StartCoroutine(ProgressDisplay());

        Manager = GameObject.FindGameObjectWithTag("WorldManager").GetComponent<WorldManager>();
        InventoryPage = GameObject.Find("InventoryUI").GetComponent<UIInventory>();

        InitializeAchievementBools();
    }

    public void InitializeAchievementBools()
    {
        ScoreAchievements = new bool[3];
        ScoreAchievements[0] = false;
        ScoreAchievements[1] = false;
        ScoreAchievements[2] = false;
    }
    
    public void IndividualEnemyKillCount(int Tag)
    {
        switch (Tag)
        {
            case 0:
                PlayerPrefs.SetInt("Kills_Pikeman", ((PlayerPrefs.GetInt("Kills_Pikeman", 0)) + 1));
                if(PlayerPrefs.GetInt("Kills_Pikeman", 0) == 5)
                {
                    GooglePlayAuthenticator.RegisterAchievement("CgkInJX8ptwQEAIQEg", 100.0f);
                }
                else if (PlayerPrefs.GetInt("Kills_Pikeman", 0) == 25)
                {
                    GooglePlayAuthenticator.RegisterAchievement("CgkInJX8ptwQEAIQHw", 100.0f);
                }
                else if (PlayerPrefs.GetInt("Kills_Pikeman", 0) == 100)
                {
                    GooglePlayAuthenticator.RegisterAchievement(" CgkInJX8ptwQEAIQLQ", 100.0f);
                }
                break;
            case 1:
                PlayerPrefs.SetInt("Kills_Fiersome", ((PlayerPrefs.GetInt("Kills_Fiersome", 0)) + 1));
                if (PlayerPrefs.GetInt("Kills_Fiersome", 0) == 5)
                {
                    GooglePlayAuthenticator.RegisterAchievement("CgkInJX8ptwQEAIQCw", 100.0f);
                }
                else if (PlayerPrefs.GetInt("Kills_Fiersome", 0) == 25)
                {
                    GooglePlayAuthenticator.RegisterAchievement(" CgkInJX8ptwQEAIQGA", 100.0f);
                }
                else if (PlayerPrefs.GetInt("Kills_Fiersome", 0) == 100)
                {
                    GooglePlayAuthenticator.RegisterAchievement("CgkInJX8ptwQEAIQJg", 100.0f);
                }
                break;
            case 2:
                PlayerPrefs.SetInt("Kills_Icykill", ((PlayerPrefs.GetInt("Kills_Icykill", 0)) + 1));
                if (PlayerPrefs.GetInt("Kills_Icykill", 0) == 5)
                {
                    GooglePlayAuthenticator.RegisterAchievement("CgkInJX8ptwQEAIQDg", 100.0f);
                }
                else if (PlayerPrefs.GetInt("Kills_Icykill", 0) == 25)
                {
                    GooglePlayAuthenticator.RegisterAchievement("CgkInJX8ptwQEAIQGw", 100.0f);
                }
                else if (PlayerPrefs.GetInt("Kills_Icykill", 0) == 100)
                {
                    GooglePlayAuthenticator.RegisterAchievement("CgkInJX8ptwQEAIQKQ", 100.0f);
                }
                break;
            case 3:
                PlayerPrefs.SetInt("Kills_Bugs", ((PlayerPrefs.GetInt("Kills_Bugs", 0)) + 1));
                if (PlayerPrefs.GetInt("Kills_Bugs", 0) == 5)
                {
                    GooglePlayAuthenticator.RegisterAchievement("CgkInJX8ptwQEAIQEw", 100.0f);
                }
                else if (PlayerPrefs.GetInt("Kills_Bugs", 0) == 25)
                {
                    GooglePlayAuthenticator.RegisterAchievement(" CgkInJX8ptwQEAIQIA", 100.0f);
                }
                else if (PlayerPrefs.GetInt("Kills_Bugs", 0) == 100)
                {
                    GooglePlayAuthenticator.RegisterAchievement(" CgkInJX8ptwQEAIQLg", 100.0f);
                }
                break;
            case 4:
                PlayerPrefs.SetInt("Kills_Buzzbee", ((PlayerPrefs.GetInt("Kills_Buzzbee", 0)) + 1));
                if (PlayerPrefs.GetInt("Kills_Buzzbee", 0) == 5)
                {
                    GooglePlayAuthenticator.RegisterAchievement(" CgkInJX8ptwQEAIQCA", 100.0f);
                }
                else if (PlayerPrefs.GetInt("Kills_Buzzbee", 0) == 25)
                {
                    GooglePlayAuthenticator.RegisterAchievement("CgkInJX8ptwQEAIQFQ", 100.0f);
                }
                else if (PlayerPrefs.GetInt("Kills_Buzzbee", 0) == 100)
                {
                    GooglePlayAuthenticator.RegisterAchievement("CgkInJX8ptwQEAIQIw", 100.0f);
                }
                break;
            case 5:
                PlayerPrefs.SetInt("Kills_Phoenix", ((PlayerPrefs.GetInt("Kills_Phoenix", 0)) + 1));
                if (PlayerPrefs.GetInt("Kills_Phoenix", 0) == 5)
                {
                    GooglePlayAuthenticator.RegisterAchievement("CgkInJX8ptwQEAIQEQ", 100.0f);
                }
                else if (PlayerPrefs.GetInt("Kills_Phoenix", 0) == 25)
                {
                    GooglePlayAuthenticator.RegisterAchievement(" CgkInJX8ptwQEAIQHg", 100.0f);
                }
                else if (PlayerPrefs.GetInt("Kills_Phoenix", 0) == 100)
                {
                    GooglePlayAuthenticator.RegisterAchievement("CgkInJX8ptwQEAIQLA", 100.0f);
                }
                break;
            case 6:
                PlayerPrefs.SetInt("Kills_Djinn", ((PlayerPrefs.GetInt("Kills_Djinn", 0)) + 1));
                if (PlayerPrefs.GetInt("Kills_Djinn", 0) == 5)
                {
                    GooglePlayAuthenticator.RegisterAchievement(" CgkInJX8ptwQEAIQDA", 100.0f);
                }
                else if (PlayerPrefs.GetInt("Kills_Djinn", 0) == 25)
                {
                    GooglePlayAuthenticator.RegisterAchievement("CgkInJX8ptwQEAIQGQ", 100.0f);
                }
                else if (PlayerPrefs.GetInt("Kills_Djinn", 0) == 100)
                {
                    GooglePlayAuthenticator.RegisterAchievement("CgkInJX8ptwQEAIQJw", 100.0f);
                }
                break;
            case 7:
                PlayerPrefs.SetInt("Kills_Razer", ((PlayerPrefs.GetInt("Kills_Razer", 0)) + 1));
                if (PlayerPrefs.GetInt("Kills_Razer", 0) == 5)
                {
                    GooglePlayAuthenticator.RegisterAchievement("CgkInJX8ptwQEAIQDQ", 100.0f);
                }
                else if (PlayerPrefs.GetInt("Kills_Razer", 0) == 25)
                {
                    GooglePlayAuthenticator.RegisterAchievement("CgkInJX8ptwQEAIQGg", 100.0f);
                }
                else if (PlayerPrefs.GetInt("Kills_Razer", 0) == 100)
                {
                    GooglePlayAuthenticator.RegisterAchievement("CgkInJX8ptwQEAIQKA", 100.0f);
                }
                break;
            case 8:
                PlayerPrefs.SetInt("Kills_IBall", ((PlayerPrefs.GetInt("Kills_IBall", 0)) + 1));
                if (PlayerPrefs.GetInt("Kills_IBall", 0) == 5)
                {
                    GooglePlayAuthenticator.RegisterAchievement("CgkInJX8ptwQEAIQCg", 100.0f);
                }
                else if (PlayerPrefs.GetInt("Kills_IBall", 0) == 25)
                {
                    GooglePlayAuthenticator.RegisterAchievement("CgkInJX8ptwQEAIQFw", 100.0f);
                }
                else if (PlayerPrefs.GetInt("Kills_IBall", 0) == 100)
                {
                    GooglePlayAuthenticator.RegisterAchievement("CgkInJX8ptwQEAIQJQ", 100.0f);
                }
                break;
            case 9:
                PlayerPrefs.SetInt("Kills_Maneotaur", ((PlayerPrefs.GetInt("Kills_Maneotaur", 0)) + 1));
                if (PlayerPrefs.GetInt("Kills_IBall", 0) == 5)
                {
                    GooglePlayAuthenticator.RegisterAchievement("CgkInJX8ptwQEAIQEA", 100.0f);
                }
                else if (PlayerPrefs.GetInt("Kills_IBall", 0) == 25)
                {
                    GooglePlayAuthenticator.RegisterAchievement("CgkInJX8ptwQEAIQHQ", 100.0f);
                }
                else if (PlayerPrefs.GetInt("Kills_IBall", 0) == 100)
                {
                    GooglePlayAuthenticator.RegisterAchievement("CgkInJX8ptwQEAIQKw", 100.0f);
                }
                break;
        }
    }
	
    public void SetAllWeaponsUnlocked(bool Value)
    {
        AllWeaponsUnlocked = Value;
    }

    public long GetScore()
    {
        return Score;
    }

    public long GetDistanceCovered()
    {
        return DistanceCovered;
    }

    public int GetKills()
    {
        return Kills;
    }

    public void AddToScore(int ScoreGained)
    {
        Score += ScoreGained;

        if(Score > 2000 && !ScoreAchievements[0])
        {
            ScoreAchievements[0] = true;
            CheckScoreForAchievement(0);
        }
        if (Score > 5000 && !ScoreAchievements[1])
        {
            ScoreAchievements[1] = true;
            CheckScoreForAchievement(1);
        }
        if (Score > 10000 && !ScoreAchievements[2])
        {
            ScoreAchievements[2] = true;
            CheckScoreForAchievement(2);
        }

        //UIScoreText.text = "" + Score;
        if (ScoreSteps.Count > 0 && Score > ScoreSteps.Peek())
        {
            ScoreSteps.Dequeue();
            ReduceRunningScoreUpdateDuration();
        }
        else if(ScoreSteps.Count <= 0 && !RunningIncrementScheduled)
        {
            RunningIncrementScheduled = true;
            Invoke("IncreaseScoreFromRunning", RunningScoreIncrementCycle);
        }
    }

    void ReduceRunningScoreUpdateDuration()
    {
        if(RunningScoreUpdateDuration > 0.3f)
        {
            RunningScoreUpdateDuration -= 0.2f;
        }
    }

    void GiveReviveForKills()
    {
        KillsForRevive = 0;
        GameController.GameControl.RevivesAdded(1);
    }

    public void CheckKillsForAchievement(int Kills_A)
    {
        if(Kills_A == 10)
        {
            //Maybe
            GooglePlayAuthenticator.RegisterAchievement("CgkInJX8ptwQEAIQDw", 100.0f);
        }
        else if (Kills_A == 50)
        {
            //Maybe
            GooglePlayAuthenticator.RegisterAchievement("CgkInJX8ptwQEAIQHA", 100.0f);
        }
        else if (Kills_A == 100)
        {
            //Maybe
            GooglePlayAuthenticator.RegisterAchievement("CgkInJX8ptwQEAIQKg", 100.0f);
        }
    }

    public void CheckDistanceForAchievement(int Distance_A)
    {
        if (Distance_A == 1)
        {
            //Maybe
            GooglePlayAuthenticator.RegisterAchievement("CgkInJX8ptwQEAIQCQ", 100.0f);
        }
        else if (Distance_A == 2)
        {
            //Maybe
            GooglePlayAuthenticator.RegisterAchievement("CgkInJX8ptwQEAIQFg", 100.0f);
        }
        else if (Distance_A == 3)
        {
            //Maybe
            GooglePlayAuthenticator.RegisterAchievement("CgkInJX8ptwQEAIQJA", 100.0f);
        }
    }

    public void CheckScoreForAchievement(int Score_A)
    {
        if (Score_A == 0)
        {
            //Maybe
            GooglePlayAuthenticator.RegisterAchievement("CgkInJX8ptwQEAIQFA", 100.0f);
        }
        else if (Score_A == 1)
        {
            //Maybe
            GooglePlayAuthenticator.RegisterAchievement("CgkInJX8ptwQEAIQIQ", 100.0f);
        }
        else if (Score_A == 2)
        {
            //Maybe
            GooglePlayAuthenticator.RegisterAchievement("CgkInJX8ptwQEAIQLw", 100.0f);
        }
    }

    public void RegisterKill(int KillTag)
    {
        Kills++;
        KillsForRevive++;

        CheckKillsForAchievement(Kills);

        IndividualEnemyKillCount(KillTag);

        if (KillsForRevive > 15)
        {
            GiveReviveForKills();
        }
        KillText.text = "Kills : " + Kills;
        if(AllWeaponsUnlocked && !IceUnlocked && Kills >= 25)//25
        {
            InventoryPage.UnlockButtons("Ice");
            IceUnlocked = true;
            Manager.SendMessenger("Ice ammo type \n unlocked");
            Manager.IncreaseSpawnProbability();

            //Maybe
            GooglePlayAuthenticator.RegisterAchievement("CgkInJX8ptwQEAIQBg", 100.0f);

        }
        else if (AllWeaponsUnlocked && !FireUnlocked && Kills >= 35)//35
        {
            InventoryPage.UnlockButtons("Fire");
            FireUnlocked = true;
            Manager.SendMessenger("Fire ammo type \n unlocked");
            Manager.IncreaseSpawnProbability();

            //Maybe
            GooglePlayAuthenticator.RegisterAchievement("CgkInJX8ptwQEAIQBw", 100.0f);            
        }
        else if(AllWeaponsUnlocked && !MagicUnlocked && Kills >= 50)//50
        {
            InventoryPage.UnlockButtons("Magic");
            MagicUnlocked = true;
            PlayerPrefs.SetInt("EverythingUnlocked", 1);
            Manager.SetProjectilesDestroyEachOther(true);
            Manager.SendMessenger("Magic ammo type \n unlocked");
            Manager.IncreaseSpawnProbability();

            //Maybe
            GooglePlayAuthenticator.RegisterAchievement("CgkInJX8ptwQEAIQBQ", 100.0f);
        }
    }

    void IncreaseScoreFromRunning()
    {
        if (ScoreFromRunning < 6)
        {
            RunningIncrementScheduled = false;
            ScoreFromRunning++;
        }
    }

    public void StartWeirdModeScoring(int Multi)
    {
        StopCoroutine(ScoringRoutine);
        WMMultiplier = Multi;
        ScoringRoutine = StartCoroutine(WeirdModeRunningScore());
    }

    IEnumerator RunningScore()
    {
        while (true)
        {
            DistanceCovered += (int)RunningScoreUpdateDuration;
            AddToScore(ScoreFromRunning);
            yield return new WaitForSeconds(RunningScoreUpdateDuration);
        }
    }

    IEnumerator WeirdModeRunningScore()
    {
        while (true)
        {
            DistanceCovered += (int)RunningScoreUpdateDuration;
            AddToScore(ScoreFromRunning * WMMultiplier);
            yield return new WaitForSeconds(RunningScoreUpdateDuration);
        }
    }

    IEnumerator LogDistance()
    {
        while (true)
        {
            DistanceCovered++;

            if(DistanceCovered >= 100 & DistanceCovered < 102)
            {
                CheckDistanceForAchievement(1);
            }

            if (DistanceCovered >= 500 & DistanceCovered < 502)
            {
                CheckDistanceForAchievement(2);
            }

            if (DistanceCovered >= 1000 & DistanceCovered < 1002)
            {
                CheckDistanceForAchievement(3);
            }

            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator ProgressDisplay()
    {
        int Tracker = 0;
        while (true)
        {
            switch (CurrentlyDisplaying)
            {
                case 0:
                    ScoreLabel.text = "SCORE";
                    ScoreLabel.color = ScoreLabelColor;
                    UIScoreText.text = "" + Score;
                    UIScoreText.color = ScoreLabelColor;
                    break;
                case 1:
                    ScoreLabel.text = "DISTANCE";
                    ScoreLabel.color = DistanceLabelColor;
                    UIScoreText.text = "" + DistanceCovered;
                    UIScoreText.color = DistanceLabelColor;
                    break;
                case 2:
                    ScoreLabel.text = "KILLS";
                    ScoreLabel.color = KillLabelColor;
                    UIScoreText.text = "" + Kills;
                    UIScoreText.color = KillLabelColor;
                    break;
            }
            Tracker++;
            if (Tracker >= 15)
            {
                Tracker = 0;
            }
            CurrentlyDisplaying = Tracker / ChangeDisplayAfterSeconds;
            yield return new WaitForSeconds(1);
        }
    }
}
