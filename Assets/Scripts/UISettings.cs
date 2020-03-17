using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UISettings : MonoBehaviour
{
    public Text VolumeLevelText;
    public Text InputSensitivityText;
    public Text AngularToleranceText;
    public Text ModeName;
    public Toggle HapticToggle;
    public Toggle PermanentUnlockToggle;
    public Toggle GraphicsToggle;
    public Toggle DifficultyToggle;
    public Toggle ShowRingToggle;
    public Sprite MuteIcon;
    public Sprite UnmuteIcon;
    public Slider VolumeSlider;
    public Slider InputSensitivitySlider;
    public Slider AngularToleranceSlider;
    public Button VolumeIcon;
    public GameObject InputPage;
    public GameObject ClassicControlsPage;
    public GameObject AlternateControlsPage;

    private int IsMuted;
    private int HapticFeedbackOn;
    private bool ScreenJustOpened;
    // Start is called before the first frame update
    void Start()
    {
        ScreenJustOpened = true;
        RefreshExistingPreferences();
    }

    public void PressedBack()
    {
        SceneManager.LoadScene(1);
    }

    public void VolumeLevelChanged(float Value)
    {
        if (IsMuted == 1 && !ScreenJustOpened)
        {
            MuteUnmute();
        }
        ScreenJustOpened = false;
        PlayerPrefs.SetFloat("VolumeLevel", Value);
        AudioListener.volume = Value;
        VolumeLevelText.text = "" + ((int)(1000 * PlayerPrefs.GetFloat("VolumeLevel", 1)) / 10) + "%";
    }
    
    public void MuteUnmute()
    {
        GameController.GameControl.PlayButtonSound();
        if (IsMuted == 0)
        {
            IsMuted = 1;
            AudioListener.volume = 0;
        }
        else
        {
            IsMuted = 0;
            AudioListener.volume = PlayerPrefs.GetFloat("VolumeLevel", 1);
        }
        PlayerPrefs.SetInt("IsMuted", IsMuted);
        RefreshExistingPreferences();
    }

    public void InputSensitivityChanged(float Value)
    {
        PlayerPrefs.SetInt("InputThreshold", (int)Value);
        InputSensitivityText.text = "" + PlayerPrefs.GetInt("InputThreshold");
    }

    public void AngularToleranceChanged(float Value)
    {
        PlayerPrefs.SetInt("AngularTolerance", (int)Value);
        AngularToleranceText.text = "" + PlayerPrefs.GetInt("AngularTolerance");
    }

    public void PermaUnlockToggle(bool NewPreference)
    {
        GameController.GameControl.PlayButtonSound();
        PlayerPrefs.SetInt("PermanentUnlock", NewPreference ? 1 : 0);
    }

    public void ToggleHaptic(bool NewHaptic)
    {
        GameController.GameControl.PlayButtonSound();
        PlayerPrefs.SetInt("HapticFeedbackOn", NewHaptic ? 1 : 0);
    }

    public void ToggleBeautifulGraphics(bool NewGraphics)
    {
        GameController.GameControl.PlayButtonSound();
        PlayerPrefs.SetInt("BeautifulGraphics", NewGraphics ? 1 : 0);
    }

    public void ToggleDifficulty(bool NewDifficulty)
    {
        GameController.GameControl.PlayButtonSound();
        PlayerPrefs.SetInt("EasyDifficulty", NewDifficulty ? 1 : 0);
    }

    public void ToggleRing(bool NewRing)
    {
        GameController.GameControl.PlayButtonSound();
        PlayerPrefs.SetInt("ShowRing", NewRing ? 1 : 0);
    }

    void RefreshExistingPreferences()
    {
        IsMuted = PlayerPrefs.GetInt("IsMuted", 0);
        HapticFeedbackOn = PlayerPrefs.GetInt("HapticFeedbackOn", 1);

        VolumeSlider.value = PlayerPrefs.GetFloat("VolumeLevel", 1);
        VolumeLevelText.text = "" + ((int)(1000 * PlayerPrefs.GetFloat("VolumeLevel", 1)) / 10) + "%";

        InputSensitivitySlider.value = PlayerPrefs.GetInt("InputThreshold", 8);
        InputSensitivityText.text = "" + PlayerPrefs.GetInt("InputThreshold", 8);

        AngularToleranceSlider.value = PlayerPrefs.GetInt("AngularTolerance", 60);
        AngularToleranceText.text = "" + PlayerPrefs.GetInt("AngularTolerance", 60);


        // Applying Existing Is Muted Value

        if (IsMuted == 0)
        {
            AudioListener.volume = PlayerPrefs.GetFloat("VolumeLevel", 1);

            VolumeIcon.GetComponent<Image>().sprite = UnmuteIcon;
        }
        else
        {
            AudioListener.volume = 0;

            VolumeIcon.GetComponent<Image>().sprite = MuteIcon;
        }

        // Applying Existing Haptic Feedback Value

        if (PlayerPrefs.GetInt("HapticFeedbackOn", 1) == 1)
        {
            HapticToggle.isOn = true;
        }
        else
        {
            HapticToggle.isOn = false;
        }


        // Applying Existing Permanent Unlock Value

        if (PlayerPrefs.GetInt("PermanentUnlock", 0) == 0)
        {
            PermanentUnlockToggle.isOn = false;
        }
        else
        {
            PermanentUnlockToggle.isOn = true;
        }

        if (PlayerPrefs.GetInt("BeautifulGraphics", 1) == 1)
        {
            GraphicsToggle.isOn = true;
        }
        else
        {
            GraphicsToggle.isOn = false;
        }

        if (PlayerPrefs.GetInt("EasyDifficulty", 0) == 0)
        {
            DifficultyToggle.isOn = false;
        }
        else
        {
            DifficultyToggle.isOn = true;
        }

        if (PlayerPrefs.GetInt("ShowRing", 0) == 0)
        {
            ShowRingToggle.isOn = false;
        }
        else
        {
            ShowRingToggle.isOn = true;
        }

        if (PlayerPrefs.GetString("ControlScheme", "Classic") == "Classic")
        {
            ModeName.text = "Classic";
            AlternateControlsPage.SetActive(false);
        }
        else
        {
            ModeName.text = "Alternate";
            ClassicControlsPage.SetActive(false);
        }

        CloseInputPage();

    }

    public void TryTutorial()
    {
        //Delete This
        GameController.GameControl.PlayButtonSound();
        SceneManager.LoadScene("Tutorial");
    }

    public void SwitchGoogleAccounts()
    {
        GooglePlayAuthenticator.SwitchGoogleAccount();
    }

    public void OpenInputPage()
    {
        InputPage.SetActive(true);
    }

    public void CloseInputPage()
    {
        InputPage.SetActive(false);
    }

    public void SwitchInputMode()
    {
        if (PlayerPrefs.GetString("ControlScheme", "Classic") == "Alt")
        {
            ModeName.text = "Classic";
            ClassicControlsPage.SetActive(true);
            AlternateControlsPage.SetActive(false);
            PlayerPrefs.SetString("ControlScheme", "Classic");
        }
        else
        {
            ModeName.text = "Alternate";
            ClassicControlsPage.SetActive(false);
            AlternateControlsPage.SetActive(true);
            PlayerPrefs.SetString("ControlScheme", "Alt");
        }
    }
}
