using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct PostProcessList
{
    public Component[] IndividualEffects;
}

public class WeirdModeManager : MonoBehaviour
{
    public Text ChoosingText;
    public Color NormalColor;
    public Color HighlightedColor;
    public GameObject TimerBar;
    public GameObject ModeSelectionScreen;
    public GameObject ModeNames;
    public GameObject ModeDescriptions;
    public GameObject SelectedModeNamePosition;
    public PlayerScoring PlayerScoringComponent;
    public PostProcessList[] PostProcessComposite;

    private int SelectedMode = 0;
    private int TotalModes;
    private int TimeSpentOnSelection = 0;
    private int TotalSelectionTime = 21;
    private bool AlreadyColoring = false;
    private float AllAnimationSpeed = 15;
    private Vector3 TopPosition;

    void Start()
    {
        AlreadyColoring = false;
        TotalModes = ModeNames.transform.childCount;
        SelectedMode = Random.Range(0, TotalModes);

        if (!AlreadyColoring)
        {
            AlreadyColoring = true;
            StartCoroutine(ColorModes());
        }

        TimerBar.SetActive(false);

        TopPosition = SelectedModeNamePosition.transform.position;
    }

    void ProxySetter()
    {
        GetComponent<WorldManager>().SetRegularMode(false);
    }

    void ProxySetter(bool NewStatus)
    {
        GetComponent<WorldManager>().SetRegularMode(NewStatus);
        GetComponent<MusicManager>().enabled = NewStatus;
    }

    public void ApplyModeSettings()
    {
        if (SelectedMode == 3)
        {
            GetComponent<InvisibleEnemies>().enabled = true;
            return;
        }

        PostProcessList SelectedComponents = PostProcessComposite[SelectedMode];
        foreach (MonoBehaviour CurrentComponent in SelectedComponents.IndividualEffects)
        {
            CurrentComponent.enabled = true;
        }
    }

    public void Proxy_ApplyModeScoring()
    {
        Invoke("ApplyModeScoring", 2);
    }

    public void ApplyModeScoring()
    {
        switch (SelectedMode)
        {
            case 0:
                PlayerScoringComponent.StartWeirdModeScoring(2);
                break;
            case 1:
                PlayerScoringComponent.StartWeirdModeScoring(5);
                break;
            case 2:
                PlayerScoringComponent.StartWeirdModeScoring(4);
                break;
            case 3:
                PlayerScoringComponent.StartWeirdModeScoring(3);
                break;
        }
    }

    IEnumerator ColorModes()
    {
        int CurrentMode = 0;
        while (TimeSpentOnSelection < TotalSelectionTime)
        {
            TimeSpentOnSelection++;
            ModeNames.transform.GetChild(CurrentMode).GetComponent<Text>().color = NormalColor;
            CurrentMode++;
            if (CurrentMode >= TotalModes)
            {
                CurrentMode = 0;
            }
            ModeNames.transform.GetChild(CurrentMode).GetComponent<Text>().color = HighlightedColor;
            yield return new WaitForSeconds(0.15f);
        }

        int CurrentIndex = 0;
        foreach (Transform CurrentModeName in ModeNames.transform)
        {
            if (CurrentIndex != SelectedMode)
            {
                StartCoroutine(FadeAwayModeName(CurrentModeName.gameObject));
            }
            CurrentIndex++;
        }

        ChoosingText.text = "Selected Mode";
        StartCoroutine(HighlightSelectedGameModeName());
    }

    IEnumerator FadeAwayModeName(GameObject ModeName)
    {
        Text ModeText = ModeName.GetComponent<Text>();
        while (ModeText.color.a > 0.1f)
        {
            ModeText.color = Color.Lerp(ModeText.color, Color.clear, AllAnimationSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        ModeName.SetActive(false);
    }

    IEnumerator HighlightSelectedGameModeName()
    {
        GameObject SelectedNameObject = ModeNames.transform.GetChild(SelectedMode).gameObject;
        SelectedNameObject.GetComponent<Text>().color = HighlightedColor;
        Vector3 BloatedScale = new Vector3(1.3f, 1.3f, 1.3f);
        Text Description = ModeDescriptions.transform.GetChild(SelectedMode).gameObject.GetComponent<Text>();
        while (Vector3.Distance(SelectedNameObject.transform.localScale, BloatedScale) > 0.03f)
        {
            SelectedNameObject.transform.position = Vector3.Lerp(SelectedNameObject.transform.position, TopPosition, AllAnimationSpeed * Time.deltaTime);
            SelectedNameObject.transform.localScale = Vector3.Lerp(SelectedNameObject.transform.localScale, BloatedScale, AllAnimationSpeed * Time.deltaTime);
            //Description.color = Color.Lerp(Description.color, Color.black, (AllAnimationSpeed + 5) * Time.deltaTime);

            yield return new WaitForEndOfFrame();
        }
        Description.color = Color.white;
        StartCoroutine(StartTimer());
    }

    IEnumerator StartTimer()
    {
        GetComponent<WorldManager>().enabled = true;
        GetComponent<BackgroundManager>().enabled = true;
        GetComponent<WorldManager>().SetRegularMode(false);

        TimerBar.SetActive(true);
        Vector3 TimerBarFinishScale = new Vector3(0, TimerBar.transform.localScale.y, TimerBar.transform.localScale.z);
        while (TimerBar.transform.localScale.x > 0.1f)
        {
            TimerBar.transform.localScale = Vector3.Lerp(TimerBar.transform.localScale, TimerBarFinishScale, 0.3f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        ProxySetter(true);
        ApplyModeSettings();
        ModeSelectionScreen.SetActive(false);
        Proxy_ApplyModeScoring();
    }

}
