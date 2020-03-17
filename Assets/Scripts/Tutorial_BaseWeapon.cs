using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_BaseWeapon : MonoBehaviour
{

    public float Cooldown = 0.5f;
    public Sprite WeaponIcon;
    public Animator WeaponAnimationController;
    public Elements WeaponDamage;
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

    // Use this for initialization
    public void Start()
    {
        TimePassed = Cooldown;
        ActionAllowed = false;

        WeaponSoundSource = gameObject.AddComponent<AudioSource>();

        Arm = GameObject.FindGameObjectWithTag("WeaponHolder");
        PlayerControl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    public void Update()
    {
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
            WeaponSoundSource.clip = WeaponLoadSound;
            WeaponSoundSource.loop = false;
            WeaponSoundSource.Play();
            TimePassed = 0;
            ActionAllowed = true;
            SwipeInitiated(SwipeStart);
            WeaponAnimationController.SetTrigger("Loaded");
        }
    }

    public void FingerRelease(Vector2 SwipeEnd)
    {
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
            //PlayerControl.RunWeaponCooldownShader();
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
}
