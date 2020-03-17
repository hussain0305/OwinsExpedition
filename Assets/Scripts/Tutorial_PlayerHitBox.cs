using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_PlayerHitBox : MonoBehaviour
{

    public Sprite Armored;
    public Sprite Regular;
    public Sprite Dead;
    public GameObject HealthPoints;
    public GameObject UIHandler;

    private int Health;
    private int HealthRec;
    private float PostDeathInvincibilityDuration = 2.5f;
    private int MaxHealth = 10;
    private Tutorial_PlayerController PlayerControl;

    // Use this for initialization
    void Start()
    {
        InitializeHealthPoints(Regular);
        Health = 3;
        UpdateHealthPoints();
        PlayerControl = GameObject.FindGameObjectWithTag("Player").GetComponent<Tutorial_PlayerController>();
    }

    void InitializeHealthPoints(Sprite ApplyThisSprite)
    {
        foreach (Transform CurrentPoint in HealthPoints.transform)
        {
            CurrentPoint.GetComponent<Image>().sprite = ApplyThisSprite;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (Health < 2)
        {
            return;
        }
        if (collision.gameObject.tag == "Enemy")
        {
            if (collision.gameObject.GetComponent<AIMinion>())
            {
                if (collision.gameObject.GetComponent<AIMinion>().IsAlive())
                {
                        UpdateHealth(-1);
                }
            }

            else if (collision.gameObject.GetComponent<Tutuorial_BaseEnemy>())
            {
                UpdateHealth(-1 * collision.gameObject.GetComponent<Tutuorial_BaseEnemy>().GetBumpDamage());
            }

            else
            {
                UpdateHealth(-2);
            }
        }
        else if (collision.gameObject.tag == "EnemyProjectile")
        {
            UpdateHealth(-1);
        }
        else if (collision.gameObject.tag == "Obstacle" || collision.gameObject.tag == "BigObstacle")
        {
            UpdateHealth(-1);
        }
        else if (collision.gameObject.tag == "Poison" && collision.gameObject.GetComponent<PoisonSludge>().ShouldDamagePlayer())
        {
            UpdateHealth(-3);
        }
    }

    public int GetHealth()
    {
        return Health;
    }

    public void UpdateHealth(int Change)
    {
        if (Change < 0)
        {
            PlayerControl.ProxyDamageFlicker();
            if (PlayerPrefs.GetInt("HapticFeedbackOn", 1) == 1)
            {
                Handheld.Vibrate();
            }
        }
        Health += Change;

        if (Health <= 0)
        {
            Health = 0;
            UpdateHealthPoints();
        }
        else
        {
            if (Health > MaxHealth)
            {
                Health = MaxHealth;
            }
            UpdateHealthPoints();
        }
    }

    void UpdateHealthPoints()
    {
        HealthRec = Health;
        InitializeHealthPoints(Dead);
        Sprite SpriteToApply = Regular;
        while (HealthRec > 0)
        {
            foreach (Transform CurrentPoint in HealthPoints.transform)
            {
                if (HealthRec > 0)
                {
                    CurrentPoint.GetComponent<Image>().sprite = SpriteToApply;
                }
                HealthRec--;
            }
            SpriteToApply = Armored;
        }
    }
    
    public void TryJump(float JumpDuration)
    {
        DisableHitBox();
        Invoke("RestoreHitBox", JumpDuration);
    }

    public void RestoreHitBox()
    {
        this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }

    public void DisableHitBox()
    {
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }

    public void PlayerRevived()
    {
        Invoke("RestoreHitBox", PostDeathInvincibilityDuration);
    }
}
