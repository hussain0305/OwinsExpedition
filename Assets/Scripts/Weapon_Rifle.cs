using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Rifle : BaseWeapon
{
    public GameObject RifleBullet;
    public float BulletSpeed;
    public int BulletsPerSecond;
    public float RifleBurstMaxDuration;
    public AudioClip RifleLoop;

    private bool RifleFiring;
    private float TimeSinceLastShot;
    private float RifleBurstDuration;
    private AudioSource RifleSFX;
    private PlayerController PlayerCharacter;

    // Use this for initialization
    new void Awake()
    {
        base.Awake();

        RifleBurstDuration = 0;

        RifleFiring = false;
        TimeSinceLastShot = 0;

        PlayerCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        RifleSFX = gameObject.AddComponent<AudioSource>();
        RifleSFX.clip = RifleLoop;
        RifleSFX.loop = true;
    }

    private void Start()
    {
        Invoke("HackyFix_StopRifleSound", 0.1f);
    }

    public void HackyFix_StopRifleSound()
    {
        RifleSFX.Stop();
    }

    new void OnEnable()
    {
        base.OnEnable();
        RifleBurstDuration = 0;

        RifleFiring = false;
        TimeSinceLastShot = 0;
        Invoke("HackyFix_StopRifleSound", 0.1f);
    }
    // Update is called once per frame
    new void Update()
    {
        base.Update();

        if (RifleFiring)
        {
            RifleBurstDuration += Time.deltaTime;

            if(RifleBurstDuration > RifleBurstMaxDuration)
            {
                FingerRelease(new Vector2());
                //SwipeEnded(new Vector2());
            }

            if(TimeSinceLastShot > (float)1 / BulletsPerSecond)
            {
                TimeSinceLastShot = 0;
                FireProjectile();
            }
            else
            {
                TimeSinceLastShot += Time.deltaTime;
            }
        }
    }

    public override void SwipeInitiated(Vector2 SwipeStart)
    {
        RifleFiring = true;
        RifleSFX.Play();
    }

    public override void SwipeEnded(Vector2 SwipeEnd)
    {
        if (RifleFiring)
        {
            PlayerCharacter.SetCurrentWeaponCooldown(0.5f + RifleBurstDuration);
        }

        RifleFiring = false;
        RifleBurstDuration = 0;
        RifleSFX.Stop();
    }

    void FireProjectile()
    {
        GameObject CurrentBullet = ObjectPooler.CentralObjectPool.SpawnFromPool(RifleBullet.name, transform.position, Arm.transform.rotation);
        CurrentBullet.GetComponent<PlayerAttackProjectile>().SetProjectileDamage((int)WeaponDamage.FetchCurrentElementValue(CurrentElement));
        CurrentBullet.GetComponent<PlayerAttackProjectile>().SetElementIndex(CurrentElement);
        CurrentBullet.GetComponent<Rigidbody2D>().velocity = transform.up * 10.0f;
    }
}
