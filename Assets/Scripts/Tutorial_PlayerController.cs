using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_PlayerController : MonoBehaviour
{

    public bool IsAlive;
    public bool WeaponResetNeeded;
    public float PlayerDashSpeed = 8.0f;
    public float PlayerJumpDuration = 1.25f;
    public Color TransparentShade;
    public Animator PlayerAnimationController;
    public GameObject ShadowChild;
    public GameObject HealthComponent;
    public Tutorial_BaseWeapon[] Weapons;
    public BaseSupermove[] Supermoves;

    private int CurrentWeaponIndex;
    private int CurrentPositionIndex;
    private int CurrentSupermoveIndex = -1;
    private int RevivesNeeded;
    private bool PlayerTouched;
    private bool PlayerAttacked = false;
    private bool PlayerCanMove;
    private bool PlayerCanAttack = false;
    private bool PlayerCanSupermove = true;
    private bool FlickerCoroutineRunning = false;
    private bool JumpCoroutineRunning = false;
    private bool WeaponCooldownCoroutineRunning = false;
    private bool SuperCooldownCoroutineRunning = false;
    private float WeaponCooldownProgress = 0;
    private float SuperCooldownProgress = 0;
    private float RetreatSpeed = 10;
    private float CurrentWeaponCooldown;
    private float SupermoveCountdown = 25;
    private float IRNTime;
    private Touch Gesture;
    private Color OriginalShade;
    private Vector2 WeaponPosition;
    private Vector2 PlayerMovementStartPosition;
    private Vector2 AttackStartedAt;
    private Vector3 DefaultPosition;
    private Vector3 PlayerTouchRayCast;
    private Tutorial_BaseWeapon CurrentWeapon;
    private GameObject WeaponArm;
    private GameObject Destination;
    private GameObject LastRecordedPosition;
    private GameObject[] PlayerPositions;
    private Coroutine PlayerRoutine;
    private Coroutine FlickerRoutine;
    private Coroutine JumpRoutine;
    private Quaternion DefaultRotation;
    private RaycastHit2D PlayerRayCastHit;
    private TutorialManager Manager;
    private MusicManager MusicManager;
    private Tutorial_UIInventory Inventory;
    private SpriteRenderer ThisSpriteRenderer;

    void Start()
    {
        PlayerPositions = new GameObject[3];
        PlayerPositions[0] = GameObject.FindGameObjectWithTag("LeftPosition");
        PlayerPositions[1] = GameObject.FindGameObjectWithTag("DefaultPosition");
        PlayerPositions[2] = GameObject.FindGameObjectWithTag("RightPosition");
        Manager = GameObject.FindGameObjectWithTag("WorldManager").GetComponent<TutorialManager>();
        MusicManager = GameObject.FindGameObjectWithTag("WorldManager").GetComponent<MusicManager>();
        Inventory = GameObject.Find("InventoryUI").GetComponent<Tutorial_UIInventory>();
        DefaultPosition = PlayerPositions[1].transform.position;
        DefaultRotation = PlayerPositions[1].transform.rotation;
        transform.position = DefaultPosition;
        transform.rotation = DefaultRotation;

        PlayerTouched = false;
        WeaponResetNeeded = true;

        WeaponArm = Instantiate(new GameObject(), transform.position, transform.rotation);
        WeaponArm.transform.SetParent(transform);
        WeaponArm.gameObject.tag = "WeaponHolder";
        WeaponArm.name = "WeaponArm";


        WeaponPosition = (Vector2)transform.position + new Vector2(0, 0.75f);
        CurrentWeaponIndex = 0;
        CurrentPositionIndex = 1;
        RevivesNeeded = 1;
        IRNTime = 60;
        PlayerCanMove = true;
        IsAlive = true;
        OriginalShade = this.gameObject.GetComponent<SpriteRenderer>().color;
        ThisSpriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();

        Invoke("IncreaseRevivesNeeded", IRNTime);
    }

    void Update()
    {

        if (WeaponCooldownCoroutineRunning)
        {
            WeaponCooldownProgress += Time.deltaTime;
        }
        if (SuperCooldownCoroutineRunning)
        {
            SuperCooldownProgress += Time.deltaTime;
        }

        if (Input.touches.Length > 0)
        {
            Gesture = Input.GetTouch(0);

            if (Gesture.phase == TouchPhase.Began)
            {
                PlayerTouchRayCast = Camera.main.ScreenToWorldPoint(Gesture.position);
                PlayerRayCastHit = Physics2D.Raycast(PlayerTouchRayCast, Vector2.zero);
                if (PlayerRayCastHit.collider && PlayerRayCastHit.collider.name == "PlayerCharacter")
                {
                    PlayerTouched = true;
                }

                if (!PlayerTouched && PlayerCanMove)
                {
                    if (PlayerCanAttack)
                    {
                        PlayerAttacked = true;
                        CurrentWeapon.FingerDown(new Vector2(Gesture.position.x, Gesture.position.y));
                        StartAttackRetreat();
                    }
                }
                else
                {
                    PlayerMovementStartPosition = Gesture.position;
                }
            }

            else if (Gesture.phase == TouchPhase.Ended && !PlayerTouched)
            {
                if (PlayerAttacked)
                {
                    CurrentWeapon.FingerRelease(new Vector2(Gesture.position.x, Gesture.position.y));
                    SetPlayerCanAttack(false);
                    EndAttackRetreat();
                    PlayerAttacked = false;
                }
            }
            else if (Gesture.phase == TouchPhase.Moved && PlayerTouched)
            {
                PlayerTouched = false;
                if (true)
                {
                    if (Gesture.position.x < PlayerMovementStartPosition.x && Mathf.Abs(PlayerMovementStartPosition.x - Gesture.position.x) > Mathf.Abs(PlayerMovementStartPosition.y - Gesture.position.y))
                    {
                        if (CurrentPositionIndex > 0)
                        {
                            PlayerAnimationController.SetTrigger("LungeLeft");
                        }
                    }
                    else if (Gesture.position.x > PlayerMovementStartPosition.x && Mathf.Abs(Gesture.position.x - PlayerMovementStartPosition.x) > Mathf.Abs(Gesture.position.y - PlayerMovementStartPosition.y))
                    {
                        if (CurrentPositionIndex < 2)
                        {
                            PlayerAnimationController.SetTrigger("LungeRight");
                        }
                    }
                    else if (Gesture.position.y > PlayerMovementStartPosition.y)
                    {
                        //GameObject.FindGameObjectWithTag("WorldManager").GetComponent<TutorialManager>().PlayerCompletedDodge = true;
                        PerformJump();
                    }
                    else
                    {
                        PerformSuperMove();
                    }
                }
            }
        }
    }

    void SpawnWeapon(int ElementIndex)
    {
        WeaponArm.transform.SetPositionAndRotation(WeaponArm.transform.position, transform.rotation);
        CurrentWeapon = Instantiate(Weapons[CurrentWeaponIndex], WeaponPosition, transform.rotation);
        CurrentWeapon.transform.SetParent(WeaponArm.transform);
        CurrentWeapon.SetCurrentElemental(ElementIndex);
        CurrentWeaponCooldown = CurrentWeapon.GetComponent<Tutorial_BaseWeapon>().Cooldown;
        PlayerCanAttack = true;
    }

    void PerformJump()
    {
        if (PlayerCanMove)
        {
            RestoreShade();

            HealthComponent.GetComponent<Tutorial_PlayerHitBox>().TryJump(PlayerJumpDuration);
            PlayerCanMove = false;
            ThisSpriteRenderer.color = new Color(ThisSpriteRenderer.color.r, ThisSpriteRenderer.color.g, ThisSpriteRenderer.color.b, 0.5f);
            //JumpRoutine = StartCoroutine(GoTranslucent());
            Invoke("ResetCanAttackAndMove", PlayerJumpDuration);
        }

    }

    void ResetCanAttackAndMove()
    {
        ThisSpriteRenderer.color = new Color(ThisSpriteRenderer.color.r, ThisSpriteRenderer.color.g, ThisSpriteRenderer.color.b, 1);
        GameObject.FindGameObjectWithTag("WorldManager").GetComponent<TutorialManager>().PlayerCompletedDodge = true;

        //JumpRoutine = StartCoroutine(GoOpaque());
        PlayerCanMove = true;
    }

    public void PlayerMoveLeft()
    {
        if (LastRecordedPosition)
            AdjustPlayerPosition(LastRecordedPosition);
        if (CurrentPositionIndex > 0)
        {
            PlayerCanMove = false;
            CurrentPositionIndex--;
            PlayerRoutine = StartCoroutine(PlayerMoveTo(PlayerPositions[CurrentPositionIndex]));
        }
    }
    public void PlayerMoveRight()
    {
        if (LastRecordedPosition)
            AdjustPlayerPosition(LastRecordedPosition);
        if (CurrentPositionIndex < 2)
        {
            PlayerCanMove = false;
            CurrentPositionIndex++;
            PlayerRoutine = StartCoroutine(PlayerMoveTo(PlayerPositions[CurrentPositionIndex]));
        }
    }

    void AdjustPlayerPosition(GameObject Destination)
    {
        StopCoroutine(PlayerRoutine);
        transform.position = Destination.transform.position;
        WeaponPosition = (Vector2)transform.position + new Vector2(0, 0.75f);
        PlayerCanMove = true;
    }

    void AttackPositioningAdjustment(Vector2 FinalAttackPosition, bool ResetNeeded)
    {
        StopCoroutine(PlayerRoutine);
        transform.position = FinalAttackPosition;

        if (ResetNeeded)
        {
            PlayerCanMove = true;
            WeaponPosition = (Vector2)transform.position + new Vector2(0, 0.75f);
            WeaponArm.transform.SetParent(transform);
            HealthComponent.transform.SetParent(transform);
            ShadowChild.transform.SetParent(transform);
        }
    }

    void StartAttackRetreat()
    {
        PlayerCanMove = false;
        AttackStartedAt = transform.position;

        transform.DetachChildren();

        Vector2 RetreatPoint = transform.position - new Vector3(0.0f, 1.0f, 0.0f);

        PlayerRoutine = StartCoroutine(PlayerAttackMovement(RetreatPoint, RetreatSpeed, false));

        PlayerAnimationController.SetTrigger("WeaponAttackLoadedRight");
    }

    void EndAttackRetreat()
    {
        StopCoroutine(PlayerRoutine);
        PlayerRoutine = StartCoroutine(PlayerAttackMovement(AttackStartedAt, RetreatSpeed, true));

        PlayerAnimationController.SetTrigger(GetWhichAttackToPerform());//"WeaponAttacked1"
    }

    string GetWhichAttackToPerform()
    {
        if (CurrentWeaponIndex == 4 || CurrentWeaponIndex == 5)
        {
            return "PlayerThrow";
        }
        return "WeaponAttacked" + Random.Range(1, 6);
    }

    public void DestroyWeapon()
    {
        if (CurrentWeapon)
        {
            Destroy(CurrentWeapon.gameObject);
        }
    }

    public void SetPlayerCanAttack(bool Value)
    {
        PlayerCanAttack = Value;
        if (!PlayerCanAttack)
        {
            Invoke("WeaponCooledDown", CurrentWeaponCooldown);
        }
    }

    public void SetPlayerCantAttack()
    {
        PlayerCanAttack = false;
        WeaponResetNeeded = false;
    }

    public void SetCurrentWeaponCooldown(float NewCooldown)
    {
        CurrentWeaponCooldown = NewCooldown;
    }

    void WeaponCooledDown()
    {
        if(WeaponResetNeeded)
            SetPlayerCanAttack(true);
    }

    public void SetCurrentWeapon(int NewIndex)
    {
        CurrentWeaponIndex = NewIndex;
    }

    public void SetCurrentSupermove(int NewIndex)
    {
        CurrentSupermoveIndex = NewIndex;
    }

    public void NewWeaponSelected(int NewIndex, int ElementIndex)
    {
        DestroyWeapon();
        SetCurrentWeapon(NewIndex);
        ResetToPlayerTransform();
        SpawnWeapon(ElementIndex);
    }

    public void ResetToDefaultTransform()
    {
        WeaponArm.transform.SetPositionAndRotation(DefaultPosition, DefaultRotation);
    }

    public void ResetToPlayerTransform()
    {
        WeaponArm.transform.SetPositionAndRotation(transform.position, transform.rotation);
    }

    public void PerformSuperMove()//int Index
    {
        if (PlayerCanSupermove && CurrentSupermoveIndex >= 0)
        {
            Invoke("ProxyRegisterTutKill", 0.5f);
            Supermoves[CurrentSupermoveIndex].PerformSuperMove();
            PlayerCanSupermove = false;
            Invoke("ResetPerformSuperMove", SupermoveCountdown);
        }
    }

    public void ProxyRegisterTutKill()
    {
        GameObject.FindGameObjectWithTag("WorldManager").GetComponent<TutorialManager>().NextInLine();
    }

    public void PickedUpHealth()
    {
        HealthComponent.GetComponent<Tutorial_PlayerHitBox>().UpdateHealth(1);
    }

    void ResetPerformSuperMove()
    {
        PlayerCanSupermove = true;
    }

    public void RestoreShade()
    {
        if (FlickerCoroutineRunning)
        {
            StopCoroutine(FlickerRoutine);
            FlickerCoroutineRunning = false;
        }

        if (JumpCoroutineRunning)
        {
            StopCoroutine(JumpRoutine);
            JumpCoroutineRunning = false;
        }
        ThisSpriteRenderer.color = OriginalShade;
    }

    void IncreaseRevivesNeeded()
    {
        RevivesNeeded++;
        IRNTime -= 5;
        if (IRNTime < 20)
        {
            IRNTime = 20;
        }
        Invoke("IncreaseRevivesNeeded", IRNTime);
    }

    public int GetRevivesNeeded()
    {
        return RevivesNeeded;
    }
    
    IEnumerator PlayerMoveTo(GameObject Destination)
    {
        LastRecordedPosition = Destination;
        while (Vector2.Distance(transform.position, Destination.transform.position) > 0.05f)
        {
            transform.position = Vector2.Lerp(transform.position, Destination.transform.position, PlayerDashSpeed * Time.deltaTime);
            yield return null;
        }
        GameObject.FindGameObjectWithTag("WorldManager").GetComponent<TutorialManager>().PlayerCompletedLunge = true;
        AdjustPlayerPosition(Destination);
        yield return new WaitForSeconds(2);
    }

    IEnumerator PlayerAttackMovement(Vector2 Destination, float Speed, bool IsResetNeeded)
    {
        while (Vector2.Distance(transform.position, Destination) > 0.05f)
        {
            transform.position = Vector2.Lerp(transform.position, Destination, Speed * Time.deltaTime);
            yield return null;
        }
        AttackPositioningAdjustment(Destination, IsResetNeeded);
        yield return new WaitForSeconds(2);
    }

    IEnumerator GoTranslucent()
    {
        JumpCoroutineRunning = true;
        while (TheColorIsNotSimilar(TransparentShade))
        {
            ThisSpriteRenderer.color = Color.Lerp(ThisSpriteRenderer.color, TransparentShade, 15 * Time.deltaTime);
            yield return null;
        }
        JumpCoroutineRunning = false;
        yield return new WaitForSeconds(2);
    }

    IEnumerator GoOpaque()
    {
        JumpCoroutineRunning = true;
        while (TheColorIsNotSimilar(OriginalShade))
        {
            ThisSpriteRenderer.color = Color.Lerp(ThisSpriteRenderer.color, OriginalShade, 10 * Time.deltaTime);
            yield return null;
        }
        ThisSpriteRenderer.color = OriginalShade;
        JumpCoroutineRunning = false;
        yield return new WaitForSeconds(2);
    }

    public void ProxyDamageFlicker()
    {
        RestoreShade();

        FlickerRoutine = StartCoroutine(PlayerDamageFlicker());
    }

    IEnumerator PlayerDamageFlicker()
    {
        FlickerCoroutineRunning = true;
        int Flickers = 0;
        bool TransparentDone;
        while (Flickers < 4)
        {
            TransparentDone = false;
            while (!TransparentDone && TheColorIsNotSimilar(TransparentShade))
            {
                ThisSpriteRenderer.color = Color.Lerp(ThisSpriteRenderer.color, TransparentShade, 20 * Time.deltaTime);
                yield return null;
            }
            ThisSpriteRenderer.color = TransparentShade;
            TransparentDone = true;
            while (TheColorIsNotSimilar(OriginalShade))
            {
                ThisSpriteRenderer.color = Color.Lerp(ThisSpriteRenderer.color, OriginalShade, 20 * Time.deltaTime);
                yield return null;
            }
            ThisSpriteRenderer.color = OriginalShade;
            Flickers++;
        }
        FlickerCoroutineRunning = false;
        yield return new WaitForSeconds(2);
    }
    
    bool TheColorIsNotSimilar(Color GoToColor)
    {
        if (!ThisSpriteRenderer)
        {
            ThisSpriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        }
        Color CurrentColor = ThisSpriteRenderer.color;
        bool Result = true;
        if ((Mathf.Abs(CurrentColor.a - GoToColor.a) < 0.075f))// && (Mathf.Abs(CurrentColor.g - GoToColor.g) < 0.075f) && (Mathf.Abs(CurrentColor.b - GoToColor.b) < 0.075f))
        {
            Result = false;
        }
        return Result;
    }
    

}
