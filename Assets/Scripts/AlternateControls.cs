using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternateControls : MonoBehaviour
{
    //Reference for the UI controls
    public GameObject AlternateControlUI;
    public GameObject AlternateNoTouchZone;

    public PlayerController PCReference;

    bool NoTouchZonePressed;

    private Touch Gesture;
    private Vector3 PlayerTouchRayCast;
    private RaycastHit2D PlayerRayCastHit;

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetString("ControlScheme", "Classic") == "Classic")
        {
            Destroy(AlternateControlUI);
            Destroy(AlternateNoTouchZone);
            Destroy(this);
            return;
        }

        NoTouchZonePressed = false;
        PCReference.gameObject.GetComponent<CircleCollider2D>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touches.Length > 0 && !NoTouchZonePressed)
        {
            Gesture = Input.GetTouch(0);

            if (Gesture.phase == TouchPhase.Began)
            {
                PlayerTouchRayCast = Camera.main.ScreenToWorldPoint(Gesture.position);
                PlayerRayCastHit = Physics2D.Raycast(PlayerTouchRayCast, Vector2.zero);

                if (PlayerRayCastHit.collider && PlayerRayCastHit.collider.gameObject.tag == "NoTouchZone")
                {
                    NoTouchZonePressed = true;
                }

                else
                {
                    if (PCReference.GetPlayerCanMove())
                    {
                        if (PCReference.GetPlayerCanAttack())
                        {
                            PCReference.SetPlayerAttacked(true);
                            PCReference.CurrentWeapon.FingerDown(new Vector2(Gesture.position.x, Gesture.position.y));
                            PCReference.StartAttackRetreat();
                        }
                    }
                }

            }

            else if (Gesture.phase == TouchPhase.Ended)
            {
                if (PCReference.GetPlayerAttacked())
                {
                    PCReference.CurrentWeapon.FingerRelease(new Vector2(Gesture.position.x, Gesture.position.y));
                    PCReference.SetPlayerCanAttack(false);
                    PCReference.EndAttackRetreat();
                    PCReference.SetPlayerAttacked(false);
                }
            }
        }

        else if (NoTouchZonePressed && Input.touches.Length == 0)
        {
            NoTouchZonePressed = false;
        }
    }

    public void PlayerJumpedLeft()
    {
        if(PCReference.GetPlayerCanMove() && PCReference.CurrentPositionIndex > 0)
            PCReference.PlayerAnimationController.SetTrigger("LungeLeft");
    }

    public void PlayerJumpedRight()
    {
        if (PCReference.GetPlayerCanMove() && PCReference.CurrentPositionIndex < 2)
            PCReference.PlayerAnimationController.SetTrigger("LungeRight");
    }

    public void PlayerPerformedJump()
    {
        if (PCReference.GetPlayerCanMove())
            PCReference.PerformJump();
    }

    public void PlayerPerformedSupermove()
    {
        if (PCReference.GetPlayerCanMove())
            PCReference.PerformSuperMove();
    }

}
