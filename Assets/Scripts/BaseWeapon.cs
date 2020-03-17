using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : MonoBehaviour {

    public float Cooldown = 0.5f;
    public Color RegularColor;
    public Color FireColor;
    public Color IceColor;
    public Color MagicColor;
    public Sprite WeaponIcon;
    public Animator WeaponAnimationController;
    public Elements WeaponDamage;
    public Material RegularMaterial;
    public Material ElementalMaterial;
    public AudioClip WeaponLoadSound;
    public AudioClip WeaponShotSound;

    private PlayerController PlayerControl;
    private AudioSource WeaponSoundSource;

    protected int CurrentElement = 0;
    protected bool ActionAllowed;
    protected float SwipeX;
    protected float SwipeY;
    protected float TimePassed;
    protected Vector2 SwipeStartAt;
    protected Vector2 SwipeEndAt;
    protected Vector2 CurrentFingerPosition;
    protected GameObject Arm;
    private SpriteRenderer MainWeaponSprite;

    // Use this for initialization
    public void Awake () {
        TimePassed = Cooldown;
        ActionAllowed = false;

        WeaponSoundSource = gameObject.AddComponent<AudioSource>();
        MainWeaponSprite = gameObject.GetComponent<SpriteRenderer>();

        Arm = GameObject.FindGameObjectWithTag("WeaponHolder");
        PlayerControl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        this.gameObject.SetActive(false);
    }

    public void OnEnable()
    {
        TimePassed = Cooldown;
        ActionAllowed = false;
        //Color SpriteColor = gameObject.GetComponent<SpriteRenderer>().color;
        //gameObject.GetComponent<SpriteRenderer>().color = new Color(SpriteColor.r, SpriteColor.g, SpriteColor.b, 1);
    }

    // Update is called once per frame
    public void Update() {
        TimePassed += Time.deltaTime;
        if (ActionAllowed)// && TimePassed > Cooldown
        {
            CurrentFingerPosition = Input.GetTouch(0).position;
            
            Vector3 diff = Camera.main.ScreenToWorldPoint(CurrentFingerPosition) - transform.position;
            diff.Normalize();

            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            Arm.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
        }
    }

    public virtual void SwipeInitiated(Vector2 SwipeStart)
    {

    }

    public virtual void SwipeEnded(Vector2 SwipeEnd)
    {

    }

    public void FingerDown(Vector2 SwipeStart)
    {
        if ((TimePassed > Cooldown))
        {
            if (WeaponSoundSource.isPlaying)
            {
                WeaponSoundSource.Stop();
            }
            if (CurrentElement > 0)
            {
                MainWeaponSprite.material = ElementalMaterial;
            }
            WeaponSoundSource.clip = WeaponLoadSound;
            WeaponSoundSource.loop = false;
            WeaponSoundSource.Play();
            TimePassed = 0;
            ActionAllowed = true;
            SwipeInitiated(SwipeStart);
            WeaponAnimationController.SetTrigger("Loaded");
            SetWeaponColor();
        }
    }

    public void FingerRelease(Vector2 SwipeEnd)
    {
        if(RegularMaterial)
            MainWeaponSprite.material = RegularMaterial;

        if (ActionAllowed)
        {
            if (WeaponSoundSource.isPlaying)
            {
                WeaponSoundSource.Stop();
            }
            WeaponSoundSource.clip = WeaponShotSound;
            WeaponSoundSource.loop = false;
            WeaponSoundSource.Play();
            ActionAllowed = false;
            SwipeEnded(SwipeEnd);
            WeaponAnimationController.SetTrigger("Shot");
            PlayerControl.RunWeaponCooldownShader();
        }
    }

    public void ResetOrientation()
    {
        PlayerControl.ResetToDefaultTransform();
    }

    public void SetCurrentElemental(int Current)
    {
        CurrentElement = Current;
    }

    public int GetCurrentElemental()
    {
        return CurrentElement;
    }

    public Sprite GetIcon()
    {
        return WeaponIcon;
    }

    public void SetWeaponColor()
    {
        switch (CurrentElement)
        {
            case 0:
                if(RegularMaterial)
                    MainWeaponSprite.material.SetColor("_OutlineColor", RegularColor);
                break;
            case 1:
                MainWeaponSprite.material.SetColor("_OutlineColor", IceColor);
                break;
            case 2:
                MainWeaponSprite.material.SetColor("_OutlineColor", FireColor);
                break;
            case 3:
                MainWeaponSprite.material.SetColor("_OutlineColor", MagicColor);
                break;
        }
    }
}
