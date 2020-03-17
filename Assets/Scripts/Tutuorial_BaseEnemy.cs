using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutuorial_BaseEnemy : MonoBehaviour
{

    public int Health;
    public int BumpDamage = 1;
    public Color SkinTone;
    public Color DefaultShade;
    public Color LowDamageShade;
    public Color MidDamageShade;
    public Color HighDamageShade;
    public Elements DamageMultipliers;
    public GameObject DeathVFX;
    public SpriteRenderer ThisSpriteRenderer;

    private int MaxHealth;
    private int NumberOfTrickles = 16;
    private int OriginalSortingLayerOrder;
    private bool CoroutineRunning;
    private bool PoisonDamageCoroutineRunning;
    private bool DeathProcessed = false;
    private float Speed = 16;
    private string OriginalSortingLayerName;
    private Coroutine HurtCoroutine;
    private Coroutine PoisonDamageCoroutine;
    private Transform CanvasObject;
    private AudioSource EnemyIdleSound;
    private AudioSource EnemyAttackSound;

    // Use this for initialization
    public void Awake()
    {
        ThisSpriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        MaxHealth = Health;
        CoroutineRunning = false;
        DeathProcessed = false;
        EnemyIdleSound = gameObject.AddComponent<AudioSource>();
        EnemyAttackSound = gameObject.AddComponent<AudioSource>();
        EnemyIdleSound.loop = true;
        EnemyAttackSound.loop = false;
        CanvasObject = GameObject.FindGameObjectWithTag("Canvas").transform;
        OriginalSortingLayerName = ThisSpriteRenderer.sortingLayerName;
        OriginalSortingLayerOrder = ThisSpriteRenderer.sortingOrder;
    }

    public void OnEnable()
    {
        ThisSpriteRenderer.sortingLayerName = OriginalSortingLayerName;
        ThisSpriteRenderer.sortingOrder = OriginalSortingLayerOrder;
    }

    public void WasAttacked()
    {

    }

    public void CalculateDamage(int Damage, int ElementIndex)
    {
        int InflictedDamage = 0;

        InflictedDamage = (int)(DamageMultipliers.FetchCurrentElementValue(ElementIndex) * Damage);
        TakeDamage(InflictedDamage);
    }

    public void TakeDamage(int DamageAmount)
    {
        Color ColorToSend = DefaultShade;
        Health -= DamageAmount;

        if (Health <= 0)
        {
            ThisGuyDied();

        }
        else
        {

            if (CoroutineRunning)
            {
                RestoreDefaultColor();
            }

            float HealthPercentage = (float)Health / (float)MaxHealth;
            if (HealthPercentage > 0.75f)
            {
                ColorToSend = LowDamageShade;
            }
            else if (HealthPercentage > 0.5f)
            {
                ColorToSend = MidDamageShade;
            }
            else
            {
                ColorToSend = HighDamageShade;
            }

            HurtCoroutine = StartCoroutine(HurtShade(ColorToSend));

        }
    }

    public void TakePoisonDamage(int DamageAmount)
    {
        if (!PoisonDamageCoroutineRunning)
        {
            StartCoroutine(PoisonDamage(DamageAmount));
        }
    }

    public int GetBumpDamage()
    {
        return BumpDamage;
    }

    public void ThisGuyDied()
    {
        if (DeathProcessed)
            return;

        if (this.gameObject.GetComponent<Tutorial_AIFirehead>())
        {
            this.gameObject.GetComponent<Tutorial_AIFirehead>().FireheadKilled();
            //Destroy(this.gameObject);
            this.gameObject.SetActive(false);
        }

        DeathProcessed = true;
        //Destroy(this.gameObject);
        this.gameObject.SetActive(false);
    }

    public void AutoDestruct()
    {
        if (this.gameObject.GetComponent<AIEyeMonster>())
        {
            GameObject.FindGameObjectWithTag("BlindingFilm").GetComponent<BlindingFilmScript>().AdjustAlpha(0);
        }
        GameObject FX = Instantiate(DeathVFX, transform.position, transform.rotation);
        FX.GetComponent<SpriteRenderer>().color = SkinTone;
        //Destroy(this.gameObject);
        this.gameObject.SetActive(false);
    }

    void RestoreDefaultColor()
    {
        if (CoroutineRunning)
        {
            StopCoroutine(HurtCoroutine);
        }
        ThisSpriteRenderer.color = DefaultShade;
    }

    void StartShadeRestoration()
    {
        if (CoroutineRunning)
        {
            StopCoroutine(HurtCoroutine);
        }
        HurtCoroutine = StartCoroutine(RestoreShade());
    }

    bool TheColorIsNotSimilar(Color GoToColor)
    {
        if (!ThisSpriteRenderer)
        {
            ThisSpriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        }
        Color CurrentColor = ThisSpriteRenderer.color;
        bool Result = true;
        if ((Mathf.Abs(CurrentColor.r - GoToColor.r) < 0.075f) && (Mathf.Abs(CurrentColor.g - GoToColor.g) < 0.075f) && (Mathf.Abs(CurrentColor.b - GoToColor.b) < 0.075f))
        {
            Result = false;
        }
        return Result;
    }

    Vector3 GetPopupLocation()
    {
        return Camera.main.WorldToScreenPoint(transform.position) + new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), Camera.main.WorldToScreenPoint(transform.position).z);
    }

    public void ShadowSizeUp()
    {
    }

    public void ShadowSizeDown()
    {
    }

    public void PlayIdleSFX(AudioClip ClipToPlay)
    {
        if (EnemyIdleSound.isPlaying)
        {
            EnemyIdleSound.Stop();
        }
        EnemyIdleSound.clip = ClipToPlay;
        EnemyIdleSound.Play();
    }

    public void PlayAttackSFX(AudioClip ClipToPlay)
    {
        if (EnemyAttackSound.isPlaying)
        {
            EnemyAttackSound.Stop();
        }
        EnemyIdleSound.volume = 0.4f;
        EnemyAttackSound.clip = ClipToPlay;
        EnemyAttackSound.Play();
        Invoke("RestoreIdleVolume", 0.4f);
    }

    void RestoreIdleVolume()
    {
        EnemyIdleSound.volume = 1;
    }

    public void SetAttackVolume(float Vol)
    {
        EnemyAttackSound.volume = Vol;
    }

    public void SetIdleVolume(float Vol)
    {
        EnemyIdleSound.volume = Vol;
    }

    IEnumerator HurtShade(Color GoToColor)
    {
        CoroutineRunning = true;
        while (TheColorIsNotSimilar(GoToColor))
        {
            ThisSpriteRenderer.color = Color.Lerp(ThisSpriteRenderer.color, GoToColor, Speed * Time.deltaTime);
            yield return null;
        }
        CoroutineRunning = false;
        StartShadeRestoration();
        yield return new WaitForSeconds(2);
    }

    IEnumerator RestoreShade()
    {
        CoroutineRunning = true;
        while (TheColorIsNotSimilar(DefaultShade))
        {
            ThisSpriteRenderer.color = Color.Lerp(ThisSpriteRenderer.color, DefaultShade, Speed * Time.deltaTime);
            yield return null;
        }
        CoroutineRunning = false;
        RestoreDefaultColor();
        yield return new WaitForSeconds(2);
    }

    IEnumerator PoisonDamage(int Damage)
    {
        PoisonDamageCoroutineRunning = true;
        int NumberOfTimesAttacked = 0;
        while (NumberOfTimesAttacked < NumberOfTrickles)
        {
            NumberOfTimesAttacked++;
            TakeDamage(Damage);
            yield return new WaitForSeconds(0.4f);
        }
        PoisonDamageCoroutineRunning = false;
        yield return new WaitForSeconds(2);
    }
}
