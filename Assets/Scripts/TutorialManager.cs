using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


[System.Serializable]
public struct TutorialPoints
{
    public string Objective;
    public string Instruction;
    public GameObject[] ObjectsToUnlock;
    public GameObject[] ObjectsToSpawn;
    public bool ObjectiveComplete;
}


public class TutorialManager : MonoBehaviour
{
    public GameObject AllTutorialObjects;
    public GameObject AllHeadings;
    public GameObject PointingArrow;
    public GameObject PointingArrowMarkers;
    public GameObject[] SceneContextSensitiveObjects;
    public GameObject StartOfTutorial;
    public GameObject Position_Mid;
    public GameObject Position_Left;
    public GameObject Position_Right;
    public TutorialPoints[] TutorialList;
    public GameObject ControlSchemeSelector;

    public bool Delay = false;
    public bool PlayerCompletedLunge = false;
    public bool PlayerCompletedDodge = false;
    public bool PlayerOpenedInventory = false;
    public bool FireheadDeathAccountedFor = false;


    private int CurrentTutorial;
    private int CurrentHeading;
    private int CurrentPointerPosition;
    private bool TutorialCoroutineRunning;
    private bool SpawningContextObjects;
    private Coroutine TutorialCoroutine;

    private Tutorial_PlayerHitBox PlayerHealth;

    void Start()
    {
        Delay = false;
        PlayerCompletedLunge = false;
        PlayerCompletedDodge = false;
        PlayerOpenedInventory = false;
        TutorialCoroutineRunning = false;
        FireheadDeathAccountedFor = false;

        PlayerHealth = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Tutorial_PlayerHitBox>();
        CurrentTutorial = -1;
        TurnOffAllObjects();
        Time.timeScale = 0;
        CurrentHeading = -1;
        CurrentPointerPosition = 0;

        UpdatePointingArrow(false);

        SpawningContextObjects = false;
    }

    public void StartTutorial()
    {
        StartOfTutorial.SetActive(false);
        NextInLine();
    }

    public void NextInLine()
    {
        CurrentTutorial++;

        if (TutorialCoroutineRunning || SpawningContextObjects)
        {
            StopCoroutine(TutorialCoroutine);
        }
        TutorialCoroutineRunning = false;
        SpawningContextObjects = false;

        switch (CurrentTutorial)
        {
            case 0:
                Time.timeScale = 0;
                TutorialPoint_1();
                break;
            case 1:
                Time.timeScale = 1;
                TutorialPoint_2();
                break;
            case 2:
                Time.timeScale = 0;
                TutorialPoint_3();
                break;
            case 3:
                Time.timeScale = 1;
                TutorialPoint_4();
                break;
            case 4:
                Time.timeScale = 0;
                TutorialPoint_5();
                break;
            case 5:
                Time.timeScale = 1;
                TutorialPoint_6();
                break;
            case 6:
                Time.timeScale = 0;
                TutorialPoint_7();
                break;
            case 7:
                TutorialPoint_8();
                break;
            case 8:
                Time.timeScale = 1;
                TutorialPoint_9();
                break;
            case 9:
                Time.timeScale = 0;
                TutorialPoint_10();
                break;
            case 10:
                TutorialPoint_11();
                break;
            case 11:
                TutorialPoint_12();
                break;
            case 12:
                Invoke("ProxySlowSpeed", 0.05f);
                TutorialPoint_13();
                break;
            case 13:
                Time.timeScale = 1;
                TutorialPoint_14();
                break;
            case 14:
                Time.timeScale = 0;
                TutorialPoint_15();
                break;
            case 15:
                TutorialPoint_16();
                break;
            case 16:
                TutorialPoint_17();
                break;
            case 17:
                Invoke("ProxySlowSpeed", 0.05f);
                TutorialPoint_18();
                break;
            case 18:
                Time.timeScale = 1;
                TutorialPoint_19();
                break;
            case 19:
                TutorialPoint_20();
                break;
            case 20:
                PlayerPrefs.SetInt("TutorialDone", 1);
                TurnOffAllObjects();
                ShowControlSchemeSelector();
                break;
        }
    }

    public void TurnOffAllObjects()
    {
        foreach(Transform CurrentElement in AllTutorialObjects.transform)
        {
            CurrentElement.gameObject.SetActive(false);
        }
        foreach (GameObject CurrentElement in SceneContextSensitiveObjects)
        {
            CurrentElement.gameObject.SetActive(false);
        }
    }

    public void TutorialPoint_1()
    {
        TutorialList[CurrentTutorial].ObjectsToUnlock[0].SetActive(true);
        UnlockHeading();
    }

    public void TutorialPoint_2()
    {
        TurnOffAllObjects();
        TutorialList[CurrentTutorial].ObjectiveComplete = false;
        TutorialCoroutine = StartCoroutine(SpawnContextObject(5, 0.9f));
        StartCoroutine(CheckIfHealthLimitReached(5));
    }

    public void TutorialPoint_3()
    {
        TurnOffAllObjects();
        TutorialList[CurrentTutorial].ObjectiveComplete = false;
        TutorialList[CurrentTutorial].ObjectsToUnlock[0].SetActive(true);
    }

    public void TutorialPoint_4()
    {
        TurnOffAllObjects();
        TutorialList[CurrentTutorial].ObjectiveComplete = false;
        TutorialCoroutine = StartCoroutine(SpawnContextObject(7, 0.9f));
        StartCoroutine(CheckIfHealthLimitReached(8));
    }

    public void TutorialPoint_5()
    {
        TurnOffAllObjects();
        TutorialList[CurrentTutorial].ObjectiveComplete = false;
        TutorialList[CurrentTutorial].ObjectsToUnlock[0].SetActive(true);
        UnlockHeading();
    }

    public void TutorialPoint_6()
    {
        TurnOffAllObjects();
        TutorialList[CurrentTutorial].ObjectiveComplete = false;
        if (!SpawningContextObjects)
        {
            TutorialCoroutine = StartCoroutine(SpawnContextObject(6, false));
        }
        StartCoroutine(CheckForLungeCompletion());
    }

    public void TutorialPoint_7()
    {
        TurnOffAllObjects();
        TutorialList[CurrentTutorial].ObjectiveComplete = false;
        TutorialList[CurrentTutorial].ObjectsToUnlock[0].SetActive(true);
    }

    public void TutorialPoint_8()
    {
        TurnOffAllObjects();
        TutorialList[CurrentTutorial].ObjectiveComplete = false;
        TutorialList[CurrentTutorial].ObjectsToUnlock[0].SetActive(true);
    }

    public void TutorialPoint_9()
    {
        TurnOffAllObjects();
        TutorialList[CurrentTutorial].ObjectiveComplete = false;
        if (!SpawningContextObjects)
        {
            TutorialCoroutine = StartCoroutine(SpawnContextObject(6, true));
        }
        
        StartCoroutine(CheckForDodgeCompletion());
    }

    public void TutorialPoint_10()
    {
        TurnOffAllObjects();
        TutorialList[CurrentTutorial].ObjectiveComplete = false;
        TutorialList[CurrentTutorial].ObjectsToUnlock[0].SetActive(true);
        UnlockHeading();
    }

    public void TutorialPoint_11()
    {
        UpdatePointingArrow(true);
        TurnOffAllObjects();
        TutorialList[CurrentTutorial].ObjectiveComplete = false;

        foreach (GameObject CurrentObject in TutorialList[CurrentTutorial].ObjectsToUnlock)
        {
            CurrentObject.SetActive(true);
            if (CurrentObject.GetComponent<Button>())
            {
                CurrentObject.GetComponent<Button>().enabled = true;
            }
        }
        
    }

    public void TutorialPoint_12()
    {
        TurnOffAllObjects();
        TutorialList[CurrentTutorial].ObjectiveComplete = false;
        NextInLine();
    }

    public void TutorialPoint_13()
    {
        UpdatePointingArrow(false);
        Time.timeScale = 0;
        TurnOffAllObjects();
        TutorialList[CurrentTutorial].ObjectiveComplete = false;
        TutorialList[CurrentTutorial].ObjectsToUnlock[0].SetActive(true);
    }

    public void TutorialPoint_14()
    {
        Delay = true;
        TurnOffAllObjects();
        TutorialList[CurrentTutorial].ObjectiveComplete = false;
        if (!SpawningContextObjects)
        {
            TutorialCoroutine = StartCoroutine(SpawnContextObject(6, false));
        }  
    }

    public void TutorialPoint_15()
    {
        TurnOffAllObjects();
        TutorialList[CurrentTutorial].ObjectiveComplete = false;
        TutorialList[CurrentTutorial].ObjectsToUnlock[0].SetActive(true);
    }

    public void TutorialPoint_16()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Tutorial_PlayerController>().SetPlayerCantAttack();
        TurnOffAllObjects();
        TutorialList[CurrentTutorial].ObjectiveComplete = false;
        foreach (GameObject CurrentObject in TutorialList[CurrentTutorial].ObjectsToUnlock)
        {
            CurrentObject.SetActive(true);
            if (CurrentObject.GetComponent<Button>())
            {
                CurrentObject.GetComponent<Button>().enabled = true;
            }
        }
    }

    public void TutorialPoint_17()
    {
        TurnOffAllObjects();
        UpdatePointingArrow(true);
        TutorialList[CurrentTutorial].ObjectiveComplete = false;
        foreach (GameObject CurrentObject in TutorialList[CurrentTutorial].ObjectsToUnlock)
        {
            CurrentObject.SetActive(true);
            if (CurrentObject.GetComponent<Button>())
            {
                CurrentObject.GetComponent<Button>().enabled = true;
            }
        }
    }

    public void TutorialPoint_18()
    {
        UpdatePointingArrow(false);
        TurnOffAllObjects();
        TutorialList[CurrentTutorial].ObjectiveComplete = false;
        foreach (GameObject CurrentObject in TutorialList[CurrentTutorial].ObjectsToUnlock)
        {
            CurrentObject.SetActive(true);
            if (CurrentObject.GetComponent<Button>())
            {
                CurrentObject.GetComponent<Button>().enabled = true;
            }
        }
    }

    public void TutorialPoint_19()
    {
        TurnOffAllObjects();
        TutorialList[CurrentTutorial].ObjectiveComplete = false;
        if (!SpawningContextObjects)
        {
            TutorialCoroutine = StartCoroutine(SpawnContextObject(6, true));
        }
    }

    public void TutorialPoint_20()
    {
        TurnOffAllObjects();
        TutorialList[CurrentTutorial].ObjectiveComplete = false;
        foreach (GameObject CurrentObject in TutorialList[CurrentTutorial].ObjectsToUnlock)
        {
            CurrentObject.SetActive(true);
            if (CurrentObject.GetComponent<Button>())
            {
                CurrentObject.GetComponent<Button>().enabled = true;
            }
        }
    }

    public void NextTutorialButton()
    {
        NextInLine();
    }

    public void ProxySlowSpeed()
    {
        Time.timeScale = 0;
    }

    public void ProxyNormalSpeed()
    {
        Time.timeScale = 1;
    }

    public void UnlockHeading()
    {
        CurrentHeading++;
        AllHeadings.transform.GetChild(CurrentHeading).gameObject.SetActive(true);
        int PreviousHeading = CurrentHeading - 1;
        if (PreviousHeading >= 0)
        {
            AllHeadings.transform.GetChild(PreviousHeading).gameObject.SetActive(false);
        }
    }

    public void UpdatePointingArrow(bool ShowArrow)
    {
        if (ShowArrow)
        {
            PointingArrow.SetActive(true);
            Transform CurrentMarker = PointingArrowMarkers.transform.GetChild(CurrentPointerPosition);
            PointingArrow.transform.SetPositionAndRotation(CurrentMarker.position, CurrentMarker.rotation);
            CurrentPointerPosition++;
        }
        else
        {
            PointingArrow.SetActive(false);
        }
    }

    public void ShowControlSchemeSelector()
    {
        ControlSchemeSelector.SetActive(true);
    }

    public void ClassicControlsSelected()
    {
        PlayerPrefs.SetString("ControlScheme", "Classic");
        SceneManager.LoadScene("RunLevel");
    }

    public void AlternateControlsSelected()
    {
        PlayerPrefs.SetString("ControlScheme", "Alt");
        SceneManager.LoadScene("RunLevel");
    }

    IEnumerator SpawnContextObject(int NextLimit, float SpawnGap)
    {
        SpawningContextObjects = true;
        while (!TutorialList[CurrentTutorial].ObjectiveComplete)
        {
            if (TutorialList[CurrentTutorial].ObjectsToSpawn[0])
            {
                ObjectPooler.CentralObjectPool.SpawnFromPool(TutorialList[CurrentTutorial].ObjectsToSpawn[0].name, Position_Mid.transform.position, Position_Mid.transform.rotation);
            }
            yield return new WaitForSeconds(SpawnGap);
        }
        SpawningContextObjects = false;
    }

    IEnumerator SpawnContextObject(float SpawnGap, bool AllThreeLanes)
    {
        if (SpawningContextObjects)
        {
            StopCoroutine(TutorialCoroutine);
        }

        if (Delay)
        {
            Delay = false;
            yield return new WaitForSeconds(1.5f);
        }
        SpawningContextObjects = true;
        while (!TutorialList[CurrentTutorial].ObjectiveComplete)
        {
            if (TutorialList[CurrentTutorial].ObjectsToSpawn[0])
            {
                ObjectPooler.CentralObjectPool.SpawnFromPool(TutorialList[CurrentTutorial].ObjectsToSpawn[0].name, Position_Mid.transform.position, Position_Mid.transform.rotation);

                if (AllThreeLanes)
                {
                    ObjectPooler.CentralObjectPool.SpawnFromPool(TutorialList[CurrentTutorial].ObjectsToSpawn[0].name, Position_Right.transform.position, Position_Right.transform.rotation);

                    ObjectPooler.CentralObjectPool.SpawnFromPool(TutorialList[CurrentTutorial].ObjectsToSpawn[0].name, Position_Left.transform.position, Position_Left.transform.rotation);
                }
            }
            yield return new WaitForSeconds(SpawnGap);
        }
        SpawningContextObjects = false;
    }

    IEnumerator CheckForLungeCompletion()
    {
        TutorialCoroutineRunning = true;
        PlayerCompletedLunge = false;
        while (!PlayerCompletedLunge)
        {
            yield return new WaitForSeconds(0);
        }
        TutorialCoroutineRunning = false;
        TutorialList[CurrentTutorial].ObjectiveComplete = true;
        NextInLine();
    }

    IEnumerator CheckForDodgeCompletion()
    {
        TutorialCoroutineRunning = true;
        PlayerCompletedDodge = false;
        while (!PlayerCompletedDodge)
        {
            yield return new WaitForSeconds(0);
        }
        TutorialCoroutineRunning = false;
        TutorialList[CurrentTutorial].ObjectiveComplete = true;
        NextInLine();
    }

    IEnumerator CheckForInventoryOpened()
    {
        while (!PlayerOpenedInventory)
        {
            yield return new WaitForSeconds(0);
        }
        TutorialList[CurrentTutorial].ObjectiveComplete = true;
        NextInLine();
    }

    IEnumerator CheckIfHealthLimitReached(int Limit)
    {
        while (PlayerHealth.GetHealth() < Limit)
        {
            yield return new WaitForSeconds(0);
        }
        foreach (Tutorial_Pickup CurrentPickup in GameObject.FindObjectsOfType<Tutorial_Pickup>())
        {
            //Destroy(CurrentPickup.gameObject);
            CurrentPickup.gameObject.SetActive(false);
        }
        TutorialList[CurrentTutorial].ObjectiveComplete = true;
        NextInLine();
    }

}