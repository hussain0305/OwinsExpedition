using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIJournal : MonoBehaviour
{
    public GameObject EnemyProfiles;
    public Image[] Frames;

    public Color SelectedColor;
    public Color NotSelectedColor;
    
    // Start is called before the first frame update
    void Start()
    {
        ToggleProfileOn(0);
    }

    public void PressedBack()
    {
        SceneManager.LoadScene(1);
    }

    public void ToggleProfileOn(int Index)
    {
        GameController.GameControl.PlayButtonSound();
        int CurrentIndex = 0;
        foreach(Transform CurrentProfile in EnemyProfiles.transform)
        {
            if (CurrentIndex == Index)
            {
                CurrentProfile.gameObject.SetActive(true);
                Frames[CurrentIndex].color = SelectedColor;
            }
            else
            {
                CurrentProfile.gameObject.SetActive(false);
                Frames[CurrentIndex].color = NotSelectedColor;
            }
            CurrentIndex++;
        }
    }
}
