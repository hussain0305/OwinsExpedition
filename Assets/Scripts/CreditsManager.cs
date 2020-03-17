using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreditsManager : MonoBehaviour
{
    public GameObject SpecialSection;
    public void PressedBack()
    {
        SceneManager.LoadScene(1);
    }

    public void ToggleSpecialSection()
    {
        SpecialSection.SetActive(!SpecialSection.activeSelf);
    }
}
