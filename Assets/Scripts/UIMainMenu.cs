using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{

    //TODO replace the old enemy names in the tip text


    public Button GameStart;
    public Button Leaderboard;
    public Button Journal;
    public Button Settings;
    public Button Store;
    public Button Exit;

    public Text LoadedPercentage;
    public Text TipText;
    public Slider LoadingLevelSlider;
    public GameObject StorePage;
    public GameObject LoadingScreen;
    public GameObject AboutPage;
    public GameObject PrivacyPolicyPage;
    public GameObject TermsAndConditionsPage;
    public GameObject DevNotePage;

    private bool StorePageOpen;
    private string[] Tips;

    // Start is called before the first frame update
    void Start()
    {
        StorePageOpen = false;
        StorePage.SetActive(StorePageOpen);
        LoadingScreen.SetActive(false);
        InitializeTips();
        RefreshTipText();
        SetAboutPagesVisibility();
    }

    public void SetAboutPagesVisibility()
    {
        PrivacyPolicyPage.SetActive(false);
        TermsAndConditionsPage.SetActive(false);
        AboutPage.SetActive(false);
        DevNotePage.SetActive(false);
    }

    public void PressedStart()
    {
        if (PlayerPrefs.GetInt("TutorialDone", 0) == 1)
        {
            StartCoroutine(LoadSceneWithLoadingBar("RunLevel"));
        }
        else
        {
            StartCoroutine(LoadSceneWithLoadingBar("Tutorial"));
        }

    }

    public void LoadWeirdMode()
    {
        GameController.GameControl.PlayButtonSound();
        if (PlayerPrefs.GetInt("TutorialDone", 0) == 1)
        {
            StartCoroutine(LoadSceneWithLoadingBar("WeirdModes"));
        }
    }

    public void PressedLeaderboard()
    {
        GameController.GameControl.PlayButtonSound();
        SceneManager.LoadScene("Leaderboard");
    }

    public void PressedJournal()
    {
        GameController.GameControl.PlayButtonSound();
        SceneManager.LoadScene("Journal");
    }

    public void PressedSettings()
    {
        GameController.GameControl.PlayButtonSound();
        SceneManager.LoadScene("Settings");
    }

    public void PressedStore()
    {
        StorePageOpen = !StorePageOpen;
        StorePage.SetActive(StorePageOpen);
        StorePage.GetComponent<StoreScript>().UpdateRITText();
    }

    public void PressedExit()
    {
        Application.Quit();
    }

    public void PressedCredits()
    {
        GameController.GameControl.PlayButtonSound();
        SceneManager.LoadScene("Credits");
    }

    void InitializeTips()
    {
        Tips = new string[]{
            "Pickup upgrades and kill enemies to increase score faster",
            "Merely damaging enemies doesn't grant any score boost. Killing them does",
            "Each enemy has unique attack patterns, study them carefully for effective dodging",
            "Each enemy has unique attack patterns, study them carefully for effective dodging",
            "Each enemy has unique attack patterns, study them carefully for effective dodging",
            "Enemies have different strengths and weaknesses. Learn their afflictions and use the suitable weapon or Elemental ammo type",
            "Beware of the bunny!! He can move and lunge across all three lanes",
            "Poison Puddles deal damage over time",
            "You get a free Revive for every 15th kill in a run.",
            "If the game feels too difficult, try the easy mode",
            "If the game feels too difficult, try the easy mode",
            "If the game feels too difficult, try the easy mode",
            "If the game feels too difficult, try the easy mode",
            "Hmmm, what happens if you hold the Start button for more than 3 seconds?",
            "Hmmm, what happens if you hold the Start button for more than 3 seconds?",
            "Hmmm, what happens if you hold the Start button for more than 3 seconds?",
            "Hmmm, what happens if you hold the Start button for more than 3 seconds?",
            "Pro Tip : Plan ahead for the minion army and drop a poison bomb ahead of the Goblin as soon as you spot him.",
            "Ice ammo type unlocks once you have the all the weapons and get 25 kills",
            "Fire ammo type unlocks once you have the all the weapons and get 35 kills",
            "Magical ammo type unlocks once you have the all the weapons and get 50 kills",
            "The minion horde can be overwhelming to deal with. The poison bomb or sword make it a little easier though",
            "Dodging Ghost's attack is difficult, but can be done with proper timing. Keep an eye for indications of imminent attacks",
            "Dodging Ghost's attack is difficult, but can be done with proper timing. Keep an eye for indications of imminent attacks",
            "Some enemies only attack straight, many don't",
            "The poison bomb must be aimed a little ahead of where you want it to fall. The physics in this game are quite real and it takes time for the bomb to fall",
            "Study enemy attack patterns and their weaknesses, and use the suitable weapon to deal with them",
            "If your regular health is full, taking health pickups would re-enforce your health points. Re-enforced health points absord 2 damage instead of 1",
            "Try killing the Goblin before he raises his minion army. There's little chance you'd be able to afterwards",
            "Try killing the Goblin before he raises his minion army. There's little chance you'd be able to afterwards",
            "Try killing the Goblin before he raises his minion army. There's little chance you'd be able to afterwards"
        };
    }

    public void RefreshTipText()
    {
        TipText.text = Tips[Random.Range(0, Tips.Length)];
        Invoke("RefreshTipText", 4);
    }

    IEnumerator LoadSceneWithLoadingBar(string SceneName)
    {
        AsyncOperation LevelLoading = SceneManager.LoadSceneAsync(SceneName);
        LoadingScreen.SetActive(true);

        while (!LevelLoading.isDone)
        {
            float Progress = Mathf.Clamp01(LevelLoading.progress / 0.9f);
            LoadingLevelSlider.value = Progress;
            LoadedPercentage.text = "" + (int)(Progress * 100) + "%";
            yield return null;
        }
    }

    public void ToggleAboutPage()
    {
        AboutPage.SetActive(!AboutPage.activeSelf);
    }

    public void TogglePrivacyPolicyPage()
    {
        PrivacyPolicyPage.SetActive(!PrivacyPolicyPage.activeSelf);
    }

    public void ToggleTermsConditionsPage()
    {
        TermsAndConditionsPage.SetActive(!TermsAndConditionsPage.activeSelf);
    }

    public void ToggleDevNotePage()
    {
        DevNotePage.SetActive(!DevNotePage.activeSelf);
    }

}
