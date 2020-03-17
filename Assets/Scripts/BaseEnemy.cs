using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    public int EnemyKillTag;
    public int Health;
    public int BumpDamage = 1;
    public bool IsArmored;
    public bool IsSuper = false;
    public Color SkinTone;
    public Color DefaultShade;
    public Color LowDamageShade;
    public Color MidDamageShade;
    public Color HighDamageShade;
    public Material NormalSkin;
    public Material ArmoredSkin;
    public Elements DamageMultipliers;
    public Animator AnimController;
    public GameObject Soul;
    public GameObject KillPopup;
    public GameObject DamagePopup;
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
    private Vector3 OriginalSize;
    private Coroutine HurtCoroutine;
    private Coroutine PoisonDamageCoroutine;
    private Transform CanvasObject;
    private GameObject InstantiatedDamagePopup;
    private GameObject InstantiatedSoul;
    private GameObject Player;
    private GameObject FX;
    private AudioSource EnemyIdleSound;
    private AudioSource EnemyAttackSound;

    // Use this for initialization
    public void Awake()
    {
        MaxHealth = Health;
        EnemyIdleSound = gameObject.AddComponent<AudioSource>();
        EnemyAttackSound = gameObject.AddComponent<AudioSource>();
        EnemyIdleSound.loop = true;
        EnemyAttackSound.loop = false;
        IsArmored = false;
        CoroutineRunning = false;
        DeathProcessed = false;
        CanvasObject = GameObject.FindGameObjectWithTag("Canvas").transform;
        OriginalSize = gameObject.transform.localScale;
        OriginalSortingLayerName = ThisSpriteRenderer.sortingLayerName;
        OriginalSortingLayerOrder = ThisSpriteRenderer.sortingOrder;
        this.gameObject.SetActive(false);
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    public void OnEnable()
    {
        ThisSpriteRenderer.sortingLayerName = OriginalSortingLayerName;
        ThisSpriteRenderer.sortingOrder = OriginalSortingLayerOrder;
        RestoreDefaultColor();
        Health = MaxHealth;
        IsArmored = false;
        CoroutineRunning = false;
        DeathProcessed = false;
        if (PoisonDamageCoroutineRunning)
        {
            StopCoroutine(PoisonDamageCoroutine);
            PoisonDamageCoroutineRunning = false;
        }
    }

    public virtual void DestroyEnemy()
    {

    }
    
    public void MakeInvisible()
    {
        ThisSpriteRenderer.color = Color.clear;
    }

    public void SetNormalSkin(Material NewSkin)
    {
        NormalSkin = NewSkin;
    }

    public void SetIsArmored(bool Value)
    {
        IsArmored = Value;
        if (Value)
        {
            ThisSpriteRenderer.material = ArmoredSkin;
        }
        else
        {
            ThisSpriteRenderer.material = NormalSkin;
        }
    }

    public void SetIsSuper(Material SuperSkin)
    {
        IsSuper = true;
        NormalSkin = SuperSkin;
        ThisSpriteRenderer.material = NormalSkin;

        Health = (int)1.3f * Health;
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
            InstantiatedDamagePopup = ObjectPooler.CentralObjectPool.SpawnFromPool(KillPopup.name, GetPopupLocation(), transform.rotation);
            InstantiatedDamagePopup.transform.SetParent(CanvasObject.transform);
            InstantiatedDamagePopup.GetComponent<DamagePopup>().SetDamageTextAndColour(MaxHealth, Color.green);

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

            InstantiatedDamagePopup = ObjectPooler.CentralObjectPool.SpawnFromPool(DamagePopup.name, GetPopupLocation(), transform.rotation);
            InstantiatedDamagePopup.transform.SetParent(CanvasObject.transform);
            InstantiatedDamagePopup.GetComponent<DamagePopup>().SetDamageTextAndColour(DamageAmount, ColorToSend);

            HurtCoroutine = StartCoroutine(HurtShade(ColorToSend));

        }

        InstantiatedSoul = ObjectPooler.CentralObjectPool.SpawnFromPool(Soul.name, transform.position, transform.rotation);
        InstantiatedSoul.GetComponent<SoulScript>().MoveSoul(GetSoulDirection(), ThisSpriteRenderer.sprite, AnimController.gameObject.transform.localScale);

    }

    public Vector2 GetSoulDirection()
    {
        Vector2 EnemyPosition = transform.position;
        Vector2 OffsetEnemyPosition = transform.position + new Vector3(Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f), 0);
        return OffsetEnemyPosition - (Vector2)Player.transform.position;
    }

    public void TakePoisonDamage(int DamageAmount)
    {
        if (!PoisonDamageCoroutineRunning)
        {
            PoisonDamageCoroutine = StartCoroutine(PoisonDamage(DamageAmount));
        }
    }

    public int GetBumpDamage()
    {
        if (IsArmored)
        {
            return 2 * BumpDamage;
        }
        return BumpDamage;
    }

    public void ThisGuyDied()
    {
        if (DeathProcessed)
            return;

        DeathProcessed = true;
        PlayerScoring PlayerScore = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScoring>();
        PlayerScore.AddToScore(MaxHealth);
        PlayerScore.RegisterKill(EnemyKillTag);
        if (this.gameObject.GetComponent<AIEyeMonster>())
        {
            GameObject.FindGameObjectWithTag("BlindingFilm").GetComponent<BlindingFilmScript>().AdjustAlpha(0);
        }
        FX = ObjectPooler.CentralObjectPool.SpawnFromPool(DeathVFX.name, (transform.position - new Vector3(0, 0.6f)), transform.rotation);
        FX.GetComponent<SpriteRenderer>().color = SkinTone;

        DestroyEnemy();
    }

    public void AutoDestruct()
    {
        if (this.gameObject.GetComponent<AIEyeMonster>())
        {
            GameObject.FindGameObjectWithTag("BlindingFilm").GetComponent<BlindingFilmScript>().AdjustAlpha(0);
        }
        FX = ObjectPooler.CentralObjectPool.SpawnFromPool(DeathVFX.name, transform.position, transform.rotation);
        FX.GetComponent<SpriteRenderer>().color = SkinTone;
        AnimController.Play("Entry", 0);
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
            ThisSpriteRenderer = this.gameObject.GetComponentInChildren<SpriteRenderer>();
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
    }

    IEnumerator DeathSouls()
    {
        int Souls = 0;
        float CurrentScale = 1.1f;
        while(Souls < 4)
        {
            Souls++;
            InstantiatedSoul = ObjectPooler.CentralObjectPool.SpawnFromPool(Soul.name, transform.position, transform.rotation);
            InstantiatedSoul.GetComponent<SoulScript>().MoveSoul(GetSoulDirection(), ThisSpriteRenderer.sprite, CurrentScale * AnimController.gameObject.transform.localScale);
            CurrentScale += 0.025f;
            yield return new WaitForSeconds(0.1f);
        }

        this.gameObject.SetActive(false);
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
    }

    public SpriteRenderer GetSpriteRenderer()
    {
        return ThisSpriteRenderer;
    }
}
