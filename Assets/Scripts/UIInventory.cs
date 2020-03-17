using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIInventory : MonoBehaviour
{
    public Text YouHaveText;
    public Text ReviveButtonText;
    public Text EndDistanceText;
    public Text EndScoreText;
    public Text EndKillsText;
    public Image WeaponIconBackground;
    public Color DefaultBGColor;
    public Color IceBGColor;
    public Color FireBGColor;
    public Color MagicBGColor;
    public Button InventoryPageButton;
    public Button PauseButtonObj;
    public GameObject Error;
    public GameObject InventoryPage;
    public GameObject UIButtons;
    public GameObject StorePage;
    public GameObject RevivePrompt;
    public GameObject QuitPrompt;
    public GameObject EndgameStatsScreen;

    private bool StorePageOpen;
    private bool InventoryPageOpen;
    private bool ErrorFlashing;
    private float PromptGrowSpeed;
    private Vector3 PromptSwollenScale;
    private Vector3 PromptNormalScale;
    private Vector3 PromptCloseScale;
    private GameObject Manager;
    private PlayerController PlayerControl;

    // Use this for initialization
    void Start()
    {
        Error.SetActive(false);
        ErrorFlashing = false;
        InventoryPageOpen = false;
        InventoryPage.SetActive(InventoryPageOpen);
        StorePageOpen = false;
        StorePage.SetActive(StorePageOpen);
        RevivePrompt.SetActive(false);
        QuitPrompt.SetActive(false);
        EndgameStatsScreen.SetActive(false);

        PromptSwollenScale = new Vector3(1.25f, 1.25f, 1.25f);
        PromptNormalScale = new Vector3(1, 1, 1);
        PromptCloseScale = new Vector3(0, 0, 0);

        PromptGrowSpeed = 0.25f;

        Manager = GameObject.FindGameObjectWithTag("WorldManager");
        PlayerControl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        if (PlayerPrefs.GetInt("EverythingUnlocked", 0) == 0 || PlayerPrefs.GetInt("PermanentUnlock", 0) == 0)
        {
            Manager.GetComponent<WorldManager>().StartSpawningWeapons();
            LockButtonOptions();
        }
    }

    void LockButtonOptions()
    {
        foreach (Transform CurrentChild in UIButtons.transform)
        {
            CurrentChild.GetComponent<Button>().enabled = false;
        }
        RefreshVisibilities();
    }

    public void InventoryButtonClicked()
    {
        InventoryPageOpen = !InventoryPageOpen;
        InventoryPage.SetActive(InventoryPageOpen);
        if (InventoryPageOpen)
        {
            PauseButtonObj.gameObject.SetActive(true);
            Time.timeScale = 0.3f;
            Manager.GetComponent<MusicManager>().SetMusicVolume(0.2f);
            PlayerControl.SetPlayerActivityPermission(false);
        }
        else
        {
            Time.timeScale = 1;
            Manager.GetComponent<MusicManager>().SetMusicVolume(1);
            PlayerControl.SetPlayerActivityPermission(true);
        }
        PlayerControl.AlternateButtonsActivity(!InventoryPage.activeSelf);
    }
    
    public void UnlockButton(string ButtonName)
    {
        UIButtons.transform.Find(ButtonName).GetComponent<Button>().enabled = true;
        RefreshVisibilities();
    }

    public void UnlockButtons(string ElementalSuffix)
    {
        foreach (Transform CurrentButton in UIButtons.transform)
        {
            if (CurrentButton.name.Contains(ElementalSuffix))
            {
                CurrentButton.GetComponent<Button>().enabled = true;
            }
        }
        RefreshVisibilities();
    }

    //This function equips the weapon and element
    public void EquipWeaponAndElement(string WeaponAndElement)
    {
        string[] SplitString = WeaponAndElement.Split(',');
        int WeaponIndex = int.Parse(SplitString[0]);
        int ElementIndex = int.Parse(SplitString[1]);
        InventoryButtonClicked();
        PlayerControl.NewWeaponSelected(WeaponIndex, ElementIndex);
    }

    //This function equips Supermoves
    public void SuperMove(int SuperMoveIndex)
    {
        PlayerControl.SetCurrentSupermove(SuperMoveIndex);
        InventoryButtonClicked();
    }

    void RefreshVisibilities()
    {
        foreach (Transform CurrentButton in UIButtons.transform)
        {
            if (CurrentButton.GetComponent<Button>().enabled)
            {
                CurrentButton.gameObject.SetActive(true);
            }
            else
            {
                CurrentButton.gameObject.SetActive(false);
            }
        }
    }

    public void PressedBack()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void PauseButton()
    {
        Time.timeScale = 0;
        PauseButtonObj.gameObject.SetActive(false);
    }

    public void ShowStorePage()
    {
        StorePageOpen = true;
        StorePage.SetActive(StorePageOpen);
    }

    public void StoreBack()
    {
        StorePageOpen = false;
        StorePage.SetActive(StorePageOpen);
        UpdateRevivePromptText();
    }

    public void UpdateRevivePromptText()
    {
        int RevivesNeeded = PlayerControl.GetRevivesNeeded();
        YouHaveText.text = "You have " + GameController.GameControl.GetRevives() + " revives";
        if (GameController.GameControl.GetRevives() >= RevivesNeeded)
        {
            ReviveButtonText.text = "Use " + RevivesNeeded + " revives";
        }
        else
        {
            ReviveButtonText.text = "Get Revives";
        }
    }

    void UpdateEndgameStatsScreenText()
    {
        long Final_Score = PlayerControl.GetComponent<PlayerScoring>().GetScore();
        long Final_Distance = PlayerControl.GetComponent<PlayerScoring>().GetDistanceCovered();
        int Final_Kills = PlayerControl.GetComponent<PlayerScoring>().GetKills();

        EndDistanceText.text = "" + Final_Distance + "m";
        EndScoreText.text = "" + Final_Score;
        EndKillsText.text = "" + Final_Kills;

        GameController.GameControl.PostRunStats(Final_Distance, Final_Score, Final_Kills);
    }

    public void ShowRevivePrompt()
    {
        RemovePopups();
        InventoryButtonOff();
        RevivePrompt.SetActive(true);
        UpdateRevivePromptText();
        RevivePrompt.transform.localScale = new Vector2(0, 0);
        StartCoroutine(PromptSwell(true, RevivePrompt));
    }

    void RemovePopups()
    {
        GameObject[] Popups = GameObject.FindGameObjectsWithTag("Popups");
        foreach (GameObject CurrentObject in Popups)
        {
            CurrentObject.gameObject.SetActive(false);
        }
    }

    public void ShowQuitPrompt()
    {
        Time.timeScale = 0;
        QuitPrompt.SetActive(true);
    }

    public void CloseQuitPrompt()
    {
        Time.timeScale = 1;
        InventoryButtonClicked();
        QuitPrompt.SetActive(false);
    }

    public void ReviveButtonPressed()
    {
        int RevivesNeeded = PlayerControl.GetRevivesNeeded();
        if (GameController.GameControl.GetRevives() >= RevivesNeeded)
        {
            PlayerControl.Revived();
        }
        else
        {
            ShowStorePage();
        }
    }

    public void ShowEndgameStats()
    {
        Time.timeScale = 1;
        RevivePrompt.SetActive(false);
        EndgameStatsScreen.SetActive(true);
        UpdateEndgameStatsScreenText();
        EndgameStatsScreen.transform.localScale = new Vector2(0, 0);
        StartCoroutine(PromptSwell(true, EndgameStatsScreen));
    }


    public void RestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //SceneManager.LoadScene("Transition");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ProxyCallPromptSwell(bool GoingForward, GameObject PromptInFocus)
    {
        StartCoroutine(PromptSwell(GoingForward, PromptInFocus));
    }

    public void InventoryButtonOn()
    {
        InventoryPageButton.gameObject.SetActive(true);
    }

    public void InventoryButtonOff()
    {
        InventoryPageButton.gameObject.SetActive(false);
    }

    public void ErrorEncountered()
    {
        if (!ErrorFlashing)
        {
            StartCoroutine(FlashError());
        }
    }

    public void ColorWeaponIconBG(int Elem)
    {
        switch (Elem)
        {
            case 0:
                WeaponIconBackground.color = DefaultBGColor;
                break;
            case 1:
                WeaponIconBackground.color = IceBGColor;
                break;
            case 2:
                WeaponIconBackground.color = FireBGColor;
                break;
            case 3:
                WeaponIconBackground.color = MagicBGColor;
                break;
        }
    }

    public IEnumerator FlashError()
    {
        ErrorFlashing = true;
        int NumFlashes = 0;
        while (NumFlashes < 8)
        {
            Error.SetActive(!Error.activeSelf);
            NumFlashes++;
            yield return new WaitForSeconds(0.25f);
        }
        ErrorFlashing = false;
        Error.SetActive(false);
    }

    public IEnumerator PromptSwell(bool GoingForward, GameObject PromptInFocus)
    {
        PromptSwollenScale.z = PromptInFocus.transform.localScale.z;
        while (Vector3.Distance(PromptInFocus.transform.localScale, PromptSwollenScale) > 0.05f)
        {
            PromptInFocus.transform.localScale = Vector3.Lerp(PromptInFocus.transform.localScale, PromptSwollenScale, PromptGrowSpeed);
            yield return null;
        }

        if (GoingForward)
        {
            StartCoroutine(PromptOpen(PromptInFocus));
        }
        else
        {
            StartCoroutine(PromptClose(PromptInFocus));
        }
    }

    IEnumerator PromptOpen(GameObject PromptInFocus)
    {
        PromptNormalScale.z = PromptInFocus.transform.localScale.z;
        while (Vector3.Distance(PromptInFocus.transform.localScale, PromptNormalScale) > 0.05f)
        {
            PromptInFocus.transform.localScale = Vector3.Lerp(PromptInFocus.transform.localScale, PromptNormalScale, PromptGrowSpeed);
            yield return null;
        }

        Time.timeScale = 0;
    }

    IEnumerator PromptClose(GameObject PromptInFocus)
    {
        PromptCloseScale.z = PromptInFocus.transform.localScale.z;
        while (Vector3.Distance(PromptInFocus.transform.localScale, PromptCloseScale) > 0.05f)
        {
            PromptInFocus.transform.localScale = Vector3.Lerp(PromptInFocus.transform.localScale, PromptCloseScale, PromptGrowSpeed);
            yield return null;
        }

        PromptInFocus.SetActive(false);
    }
}
