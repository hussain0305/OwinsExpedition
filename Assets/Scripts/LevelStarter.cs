using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LevelStarter : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public UIMainMenu MainMenuConnection;
    public bool buttonPressed;

    private bool AlreadyProcessedLevel;
    private float ButtonPressedDuration = 0;

    void Start()
    {
        AlreadyProcessedLevel = false;
        ButtonPressedDuration = 0;
    }

    void Update()
    {
        if (buttonPressed)
        {
            ButtonPressedDuration += Time.deltaTime;
            if (ButtonPressedDuration > 2.5f)
            {
                LoadLevelAccordingly();
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonPressed = false;
        LoadLevelAccordingly();
    }

    void LoadLevelAccordingly()
    {
        if (!AlreadyProcessedLevel)
        {
            AlreadyProcessedLevel = true;
        }
        else
        {
            return;
        }
        GameController.GameControl.PlayButtonSound();
        if (ButtonPressedDuration < 2.5f)
        {
            MainMenuConnection.PressedStart();
        }
        else
        {
            MainMenuConnection.LoadWeirdMode();
        }
    }

}
