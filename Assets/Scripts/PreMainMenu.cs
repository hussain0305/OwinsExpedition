using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class PreMainMenu : MonoBehaviour
{
    public float TransparencySpeed;
    public float CallDelay_1;
    public float CallDelay_2;
    public float CallDelay_3;
    public float WritingSpeed;
    public Text EurusText;
    public Image[] Triangles_1;
    public Image[] Triangles_2;
    public Image[] Triangles_3;
    public Image Screen;

    private int CurrentTriangle_1;
    private int CurrentTriangle_2;
    private int CurrentTriangle_3;
    private int NoOfTriangles_1;
    private int NoOfTriangles_2;
    private int NoOfTriangles_3;
    private int CurrentCharacter;
    private int NetScore;
    private string EurusGamesText;

    void Start()
    {
        CurrentCharacter = 0;
        CurrentTriangle_1 = -1;
        CurrentTriangle_2 = -1;
        CurrentTriangle_3 = -1;
        NetScore = -3;
        NoOfTriangles_1 = Triangles_1.Length;
        NoOfTriangles_2 = Triangles_2.Length;
        NoOfTriangles_3 = Triangles_3.Length;
        EurusGamesText = "Eurus Games";
        StartAnimatingLogo();
        StartCoroutine(EurusTextCoroutine());
    }

    public void StartAnimatingLogo()
    {
        Invoke("ProcessTriangle_1", CallDelay_1);
        Invoke("ProcessTriangle_2", CallDelay_2);
        Invoke("ProcessTriangle_3", CallDelay_3);
        //ProcessTriangle_1();
        //ProcessTriangle_2();
        //ProcessTriangle_3();

    }

    public void OnToMainMenu()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }

    public void ProcessTriangle_1()
    {
        CurrentTriangle_1++;
        if (CurrentTriangle_1 < NoOfTriangles_1)
        {
            Triangles_1[CurrentTriangle_1].gameObject.GetComponent<Animator>().SetTrigger("Appear");
            Invoke("ProcessTriangle_1", TransparencySpeed);
        }
        else
        {
            IsLogoComplete();
        }
    }

    public void ProcessTriangle_2()
    {
        CurrentTriangle_2++;
        if (CurrentTriangle_2 < NoOfTriangles_2)
        {
            Triangles_2[CurrentTriangle_2].gameObject.GetComponent<Animator>().SetTrigger("Appear");
            Invoke("ProcessTriangle_2", TransparencySpeed);
        }
        else
        {
            IsLogoComplete();
        }
    }

    public void ProcessTriangle_3()
    {
        CurrentTriangle_3++;
        if (CurrentTriangle_3 < NoOfTriangles_3)
        {
            Triangles_3[CurrentTriangle_3].gameObject.GetComponent<Animator>().SetTrigger("Appear");
            Invoke("ProcessTriangle_3", TransparencySpeed);
        }
        else
        {
            IsLogoComplete();
        }
    }

    public void IsLogoComplete()
    {
        NetScore++;
        if (NetScore >= 0)
        {
            //OnToMainMenu();
            //Invoke("OnToMainMenu", 1);
            StartCoroutine(BGScreen());
        }
    }
    
    IEnumerator EurusTextCoroutine()
    {
        while (EurusText.text != EurusGamesText)
        {
            CurrentCharacter++;
            if(CurrentCharacter <= EurusGamesText.Length)
                EurusText.text = EurusGamesText.Substring(0, CurrentCharacter);
            yield return new WaitForSeconds(WritingSpeed);
        }
    }

    IEnumerator BGScreen()
    {
        yield return new WaitForSeconds(0.75f);
        Color ColorToReach = new Color(Screen.color.r, Screen.color.g, Screen.color.b, 1);
        while (Screen.color.a < .5f)
        {
            Screen.color = Color.Lerp(Screen.color, ColorToReach, 3 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        OnToMainMenu();
        while (Screen.color.a < 1)
        {
            Screen.color = Color.Lerp(Screen.color, ColorToReach, 3 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }
}
