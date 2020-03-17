using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassicPlayerControls : MonoBehaviour
{
    public PlayerController PCReference;

    bool NoTouchZonePressed;
    bool PlayerTouched;

    private float InputThreshold;
    private float AngularTolerance;
    private Touch Gesture;
    private Vector2 PlayerTappedAtPosition;
    private Vector2 PlayerMovementStartPosition;
    private Vector3 PlayerTouchRayCast;
    private RaycastHit2D PlayerRayCastHit;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetString("ControlScheme", "Classic") == "Alt")
        {
            Destroy(this);
        }

        NoTouchZonePressed = false;
        PlayerTouched = false;

        InputThreshold = PlayerPrefs.GetInt("InputThreshold", 33) / 33;
        AngularTolerance = PlayerPrefs.GetInt("AngularTolerance", 60);

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
                    if (PlayerRayCastHit.collider && (PlayerRayCastHit.collider.name == "PlayerCharacter" ||
                        PlayerRayCastHit.collider.name == "Health" ||
                        PlayerRayCastHit.collider.name == "PickingComponent"))
                    {
                        PlayerTouched = true;
                        PlayerTappedAtPosition = Camera.main.ScreenToWorldPoint(Gesture.position);
                    }

                    if (!PlayerTouched && PCReference.GetPlayerCanMove())
                    {
                        if (PCReference.GetPlayerCanAttack())
                        {
                            PCReference.SetPlayerAttacked(true);
                            PCReference.CurrentWeapon.FingerDown(new Vector2(Gesture.position.x, Gesture.position.y));
                            PCReference.StartAttackRetreat();
                        }
                    }
                    else
                    {
                        PlayerMovementStartPosition = Gesture.position;
                    }
                }

            }

            else if (Gesture.phase == TouchPhase.Ended && !PlayerTouched)
            {
                if (PCReference.GetPlayerAttacked())
                {
                    PCReference.CurrentWeapon.FingerRelease(new Vector2(Gesture.position.x, Gesture.position.y));
                    PCReference.SetPlayerCanAttack(false);
                    PCReference.EndAttackRetreat();
                    PCReference.SetPlayerAttacked(false);
                }
            }
            else if (Gesture.phase == TouchPhase.Moved && PlayerTouched && Vector2.Distance(PlayerTappedAtPosition, Camera.main.ScreenToWorldPoint(Gesture.position)) > InputThreshold)
            {
                PlayerTouched = false;
                if (PCReference.GetPlayerCanMove())
                {
                    Vector2 SwipeUpAt = Camera.main.ScreenToWorldPoint(Gesture.position);
                    float SwipeAngle = Mathf.Atan2(SwipeUpAt.y - PlayerTappedAtPosition.y, SwipeUpAt.x - PlayerTappedAtPosition.x) * 180 / Mathf.PI;

                    if ((SwipeAngle > (180 - (AngularTolerance / 2))) || (SwipeAngle < (-180 + (AngularTolerance / 2))))
                    {
                        if (PCReference.CurrentPositionIndex > 0)
                        {
                            PCReference.PlayerAnimationController.SetTrigger("LungeLeft");
                        }
                    }
                    else if (SwipeAngle > -1 * (AngularTolerance / 2) && SwipeAngle < (AngularTolerance / 2))
                    {
                        if (PCReference.CurrentPositionIndex < 2)
                        {
                            PCReference.PlayerAnimationController.SetTrigger("LungeRight");
                        }
                    }
                    else if (SwipeAngle > 90 - (AngularTolerance / 2) && SwipeAngle < 90 + (AngularTolerance / 2))
                    {
                        PCReference.PerformJump();
                    }
                    else if (SwipeAngle > -90 - (AngularTolerance / 2) && SwipeAngle < -90 + (AngularTolerance / 2))
                    {
                        PCReference.PerformSuperMove();
                    }
                }
            }
        }

        else if (NoTouchZonePressed && Input.touches.Length == 0)
        {
            NoTouchZonePressed = false;
        }
    }
}
