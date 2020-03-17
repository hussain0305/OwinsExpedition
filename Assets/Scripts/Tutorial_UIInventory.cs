using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Tutorial_UIInventory : MonoBehaviour
{

    public Button InventoryPageButton;
    public Button PauseButtonObj;
    public GameObject InventoryPage;
    public GameObject UIButtons;
    public TutorialManager TutorialManagerObj;

    private bool InventoryPageOpen;
    private float PromptGrowSpeed;
    private Vector3 PromptSwollenScale;
    private Vector3 PromptNormalScale;
    private Vector3 PromptCloseScale;
    private TutorialManager TM;
    private Tutorial_PlayerController PlayerControl;

    // Use this for initialization
    void Start()
    {
        InventoryPageOpen = false;
        InventoryPage.SetActive(InventoryPageOpen);

        PromptSwollenScale = new Vector3(1.25f, 1.25f, 1.25f);
        PromptNormalScale = new Vector3(1, 1, 1);
        PromptCloseScale = new Vector3(0, 0, 0);

        PromptGrowSpeed = 0.25f;

        PlayerControl = GameObject.FindGameObjectWithTag("Player").GetComponent<Tutorial_PlayerController>();
        if (PlayerPrefs.GetInt("EverythingUnlocked", 0) == 0 || PlayerPrefs.GetInt("PermanentUnlock", 0) == 0)
        {
            LockButtonOptions();
        }

        TM = GameObject.FindGameObjectWithTag("WorldManager").GetComponent<TutorialManager>();
    }

    void LockButtonOptions()
    {
        foreach (Transform CurrentChild in UIButtons.transform)
        {
            CurrentChild.GetComponent<Button>().enabled = false;
        }
        RefreshVisibilities();
    }

    public void InventoryButtonClicked(bool CalledFromUI)
    {
        if(CalledFromUI)
            TutorialManagerObj.UpdatePointingArrow(true);
        TM.PlayerOpenedInventory = true;
        InventoryPageOpen = !InventoryPageOpen;
        InventoryPage.SetActive(!InventoryPage.activeSelf);
        if (InventoryPageOpen)
        {
            //PauseButtonObj.gameObject.SetActive(true);
            Time.timeScale = 0;
            GameObject.FindGameObjectWithTag("WorldManager").GetComponent<MusicManager>().SetMusicVolume(0.2f);
        }
        else
        {
            Time.timeScale = 1;
            GameObject.FindGameObjectWithTag("WorldManager").GetComponent<MusicManager>().SetMusicVolume(1);
        }
    }

    //Deprecated Function Equip Weapon. Can be removed
    public void EquipWeapon(int Index)
    {
        //PlayerControl.NewWeaponSelected(Index);
        InventoryButtonClicked(false);
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

    public void EquipWeaponAndElement(string WeaponAndElement)
    {
        TM.NextInLine();

        string[] SplitString = WeaponAndElement.Split(',');
        int WeaponIndex = int.Parse(SplitString[0]);
        int ElementIndex = int.Parse(SplitString[1]);
        PlayerControl.NewWeaponSelected(WeaponIndex, ElementIndex);
        InventoryButtonClicked(false);
    }

    public void SuperMove(int SuperMoveIndex)
    {
        TM.NextInLine();
        PlayerControl.SetCurrentSupermove(SuperMoveIndex);
        InventoryButtonClicked(false);
    }

    public void QuitTutorial()
    {
        SceneManager.LoadScene(1);
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
        //PauseButtonObj.gameObject.SetActive(false);
    }
    
    void RemovePopups()
    {
        GameObject[] Popups = GameObject.FindGameObjectsWithTag("Popups");
        foreach (GameObject CurrentObject in Popups)
        {
            Destroy(CurrentObject);
        }
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
        yield return new WaitForSeconds(2);
    }

    IEnumerator PromptOpen(GameObject PromptInFocus)
    {
        PromptNormalScale.z = PromptInFocus.transform.localScale.z;
        while (Vector3.Distance(PromptInFocus.transform.localScale, PromptNormalScale) > 0.05f)
        {
            PromptInFocus.transform.localScale = Vector3.Lerp(PromptInFocus.transform.localScale, PromptNormalScale, PromptGrowSpeed);
            yield return null;
        }

        yield return new WaitForSeconds(2);
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
        yield return new WaitForSeconds(2);
    }
}
