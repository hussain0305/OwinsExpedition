using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UILeaderboard : MonoBehaviour
{
    // Start is called before the first frame update

    public Text BestDistanceText;
    public Text BestScoreText;
    public Text BestKillsText;
    public Text TotalDistanceText;
    public Text TotalScoreText;
    public Text TotalKillsText;
    public Text DeathsText;
    public Text AverageDistanceText;
    public Text AverageScoreText;
    public Text AverageKillsText;
    public GameObject CenterPanel;
    public GameObject LeftPanel;
    public GameObject RightPanel;
    public GameObject CenterMarker;
    public GameObject LeftMarker;
    public GameObject RightMarker;

    public float PanelMovementSpeed = 0.3f;

    private bool CanSwipe;
    private bool CenterMoving = false;
    private bool NeighbourMoving = false;
    private Touch Gesture;
    private Vector2 TouchPosition;

    void Start()
    {
        CanSwipe = true;
        CenterMoving = false;
        NeighbourMoving = false;
        UpdatePersonalStats();
    }

    // Update is called once per frame
    void Update()
    {
        if (CanSwipe && Input.touches.Length > 0)
        {
            Gesture = Input.GetTouch(0);
            if (Gesture.position.y < Screen.height/4 || Gesture.position.y > 3 * Screen.height / 4)
            {
                return;
            }

            if (Gesture.phase == TouchPhase.Began)
            {
                TouchPosition = Gesture.position;
            }

            else if (Gesture.phase == TouchPhase.Moved)
            {
                CanSwipe = false;
                if (Gesture.position.x < TouchPosition.x)
                {
                    MovePanelsLeft();
                }
                else
                {
                    MovePanelsRight();
                }
            }
        }
    }

    public void MovePanelsLeft()
    {
        StartCoroutine(MoveCenterLeft());
        StartCoroutine(MoveNeighbourLeft());
    }

    public void MovePanelsRight()
    {
        StartCoroutine(MoveCenterRight());
        StartCoroutine(MoveNeighbourRight());
    }

    void UpdatePersonalStats()
    {
        BestDistanceText.text = "covered <size=45><color=green>" + GameController.GameControl.GetBestDistance() + "m</color></size>";
        BestKillsText.text = "killed <size=45><color=green>" + GameController.GameControl.GetBestKills() + "</color></size> monsters";
        BestScoreText.text = "scored <size=45><color=green>" + GameController.GameControl.GetBestScore() + "</color></size> points";

        TotalDistanceText.text = "covered <size=45><color=green>" + GameController.GameControl.GetTotalDistance() + "m</color></size>";
        TotalKillsText.text = "killed <size=45><color=green>" + GameController.GameControl.GetTotalKills() + "</color></size> monsters";
        TotalScoreText.text = "scored <size=45><color=green>" + GameController.GameControl.GetTotalScore() + "</color></size> points";

        DeathsText.text = "died <size=45><color=green>" + GameController.GameControl.GetDeaths() + "</color></size> times";

        if (GameController.GameControl.GetDeaths() == 0)
        {
            AverageDistanceText.text = "covered <size=45><color=green>" + 0 + "m</color></size>";
            AverageScoreText.text = "killed <size=45><color=green>" + 0 + "</color></size> monsters";
            AverageKillsText.text = "scored <size=45><color=green>" + 0 + "</color></size> points";
        }
        else
        {
            AverageDistanceText.text = "covered <size=45><color=green>" + ((int)(GameController.GameControl.GetTotalDistance() / GameController.GameControl.GetDeaths()) * 100) / 100 + "m</color></size>";
            AverageScoreText.text = "scored <size=45><color=green>" + ((int)(GameController.GameControl.GetTotalScore() / GameController.GameControl.GetDeaths()) * 100) / 100 + "</color></size> points";
            AverageKillsText.text = "killed <size=45><color=green>" + ((int)(GameController.GameControl.GetTotalKills() / GameController.GameControl.GetDeaths()) * 100) / 100 + "</color></size> monsters";
        }
    }

    public void PressedBack()
    {
        SceneManager.LoadScene(1);
    }

    public void ShowGlobalDistance()
    {
        GooglePlayAuthenticator.ShowDistanceLeaderboard();
    }

    public void ShowGlobalKills()
    {
        GooglePlayAuthenticator.ShowKillsLeaderboard();
    }

    public void ShowGlobalScore()
    {
        GooglePlayAuthenticator.ShowScoreLeaderboard();
    }

    IEnumerator MoveCenterRight()
    {
        CenterMoving = true;
        while (Mathf.Abs(CenterPanel.transform.position.x - RightMarker.transform.position.x) > 5)
        {
            CenterPanel.transform.position = Vector2.Lerp(CenterPanel.transform.position, RightMarker.transform.position, PanelMovementSpeed);
            yield return new WaitForEndOfFrame();
        }
        CenterPanel.transform.position = RightMarker.transform.position;
        CenterMoving = false;
    }

    IEnumerator MoveNeighbourRight()
    {
        NeighbourMoving = true;
        while (Mathf.Abs(LeftPanel.transform.position.x - CenterMarker.transform.position.x) > 5)
        {
            LeftPanel.transform.position = Vector2.Lerp(LeftPanel.transform.position, CenterMarker.transform.position, PanelMovementSpeed);
            yield return new WaitForEndOfFrame();
        }
        LeftPanel.transform.position = CenterMarker.transform.position;
        NeighbourMoving = false;
        CanSwipe = true;

        GameObject CopyBuffer = CenterPanel;
        CenterPanel = LeftPanel;
        LeftPanel = RightPanel;
        LeftPanel.transform.position = LeftMarker.transform.position;
        RightPanel = CopyBuffer;
    }

    IEnumerator MoveCenterLeft()
    {
        CenterMoving = true;
        while (Mathf.Abs(CenterPanel.transform.position.x - LeftMarker.transform.position.x) > 5)
        {
            CenterPanel.transform.position = Vector2.Lerp(CenterPanel.transform.position, LeftMarker.transform.position, PanelMovementSpeed);
            yield return new WaitForEndOfFrame();
        }
        CenterPanel.transform.position = LeftMarker.transform.position;
        CenterMoving = false;
    }

    IEnumerator MoveNeighbourLeft()
    {
        NeighbourMoving = true;
        while (Mathf.Abs(RightPanel.transform.position.x - CenterMarker.transform.position.x) > 5)
        {
            RightPanel.transform.position = Vector2.Lerp(RightPanel.transform.position, CenterMarker.transform.position, PanelMovementSpeed);
            yield return new WaitForEndOfFrame();
        }
        RightPanel.transform.position = CenterMarker.transform.position;
        NeighbourMoving = false;
        CanSwipe = true;

        GameObject CopyBuffer = CenterPanel;
        CenterPanel = RightPanel;
        RightPanel = LeftPanel;
        RightPanel.transform.position = RightMarker.transform.position;
        LeftPanel = CopyBuffer;
    }
}
