using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEyeMonster : BaseEnemy{

    public AudioClip EyeMonsterSound;

    private int NumberOfPositions;
    private bool BlindingCoRunning;
    private float BlindingSpeed;
    private Coroutine VisibilityChange;
    private GameObject PossiblePositions;
    private WorldManager Manager;
    private SpriteRenderer BlindingFilm;

    // Use this for initialization
    new void Start () {
        base.Awake();
        BlindingSpeed = 0.9f;
        BlindingCoRunning = false;
        BlindingFilm = GameObject.FindGameObjectWithTag("BlindingFilm").GetComponent<SpriteRenderer>();
        Manager = GameObject.FindGameObjectWithTag("WorldManager").GetComponent<WorldManager>();
        PossiblePositions = GameObject.FindGameObjectWithTag("ScreenSpaceMarkers");

        //PlayIdleSFX(EyeMonsterSound);

        NumberOfPositions = PossiblePositions.transform.childCount;
    }

    new void OnEnable()
    {
        base.OnEnable();
        BlindingCoRunning = false;
        PlayIdleSFX(EyeMonsterSound);
    }

    public void StartBlinding()
    {
        if (BlindingCoRunning)
        {
            StopCoroutine(VisibilityChange);
        }
        VisibilityChange = StartCoroutine(BlindingCoroutine(1));
    }

    public void EndBlinding()
    {
        if (BlindingCoRunning)
        {
            StopCoroutine(VisibilityChange);
        }
        VisibilityChange = StartCoroutine(BlindingCoroutine(0));
    }

    public void AdjustAlpha(float Alpha)
    {
        Color newColor = new Color(1, 1, 1, Alpha);
        BlindingFilm.color = newColor;
    }

    void AdjustIBallPosition(Vector2 Destination)
    {
        transform.position = Destination;
        StartCoroutine(IBallFlight(PossiblePositions.transform.GetChild(Random.Range(0, NumberOfPositions)).transform.position));
    }

    public void StartFlying()
    {
        if (!Manager.GetIBallMoves())
        {
            return;
        }
        StartCoroutine(IBallFlight(PossiblePositions.transform.GetChild(Random.Range(0, NumberOfPositions)).transform.position));
    }


    IEnumerator BlindingCoroutine(float AlphaValue)
    {
        BlindingCoRunning = true;
        float alpha = BlindingFilm.color.a;

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / BlindingSpeed)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, AlphaValue, t));
            BlindingFilm.color = newColor;
            yield return null;
        }

        BlindingCoRunning = false;
        yield return new WaitForSeconds(2);
    }

    IEnumerator IBallFlight(Vector2 Destination)
    {
        yield return new WaitForSeconds(0.5f);
        while (Vector2.Distance(Destination, transform.position) > 0.1f)
        {
            transform.position = Vector2.Lerp(transform.position, Destination, 2 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        AdjustIBallPosition(Destination);
    }

    public override void DestroyEnemy()
    {
        StopAllCoroutines();
        this.gameObject.SetActive(false);
    }
}
