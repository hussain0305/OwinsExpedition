using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public bool IsAlive;
    public float PlayerDashSpeed = 8.0f;
    public float PlayerJumpDuration = 5.25f;
    public Color TransparentShade;
    public Color DamageShade;
    public Image WeaponCooldownShader;
    public Image SuperCooldownShader;
    public Button[] AlternateControlButtons;
    public Animator PlayerAnimationController;
    public GameObject HealthComponent;
    public GameObject WeaponIcon;
    public GameObject SuperIcon;
    public GameObject PlayerShadow;
    public GameObject TelekinesisVisuals;
    public GameObject TelekinesisLines;
    public BaseWeapon[] Weapons;
    public BaseSupermove[] Supermoves;
    public FullScreenDamageIndicator FullScreenIndicator;

    public MonoBehaviour InvincibilityPostProcess;

    //Below - These were ealier private
    public BaseWeapon CurrentWeapon;
    public int CurrentPositionIndex;

    private int CurrentWeaponIndex;
    private int CurrentSupermoveIndex = -1;
    private int TempCurrentSupermoveIndex = -1;
    private int LastWeaponIndex;
    private int LastSupermoveIndex;
    private int RevivesNeeded;
    private int LateWeaponIndex;
    //private bool PlayerTouched;
    //private bool NoTouchZonePressed;
    private bool PlayerAttacked = false;
    private bool PlayerCanMove;
    private bool PlayerCanAttack = false;
    private bool PlayerCanSupermove = true;
    private bool FlickerCoroutineRunning = false;
    private bool MovementCoroutineRunning = false;
    private bool JumpCoroutineRunning = false;
    private bool WeaponCooldownCoroutineRunning = false;
    private bool SuperCooldownCoroutineRunning = false;
    //private float InputThreshold;
    //private float AngularTolerance;
    private float WeaponCooldownProgress = 0;
    private float SuperCooldownProgress = 0;
    private float RetreatSpeed = 10;
    private float CurrentWeaponCooldown;
    private float SupermoveCountdown = 25;
    private float IRNTime;
    //private Touch Gesture;
    private Color OriginalShade;
    private Vector2 WeaponPosition;
    private Vector2 WeaponPositionOffset;
    //private Vector2 PlayerMovementStartPosition;
    //private Vector2 PlayerTappedAtPosition;
    private Vector2 AttackStartedAt;
    private Vector2 WeaponDefaultScale;
    private Vector2 PlayerWeaponShift;
    private Vector3 DefaultPosition;
    //private Vector3 PlayerTouchRayCast;
    private Vector3 PlayerDefaultScale;
    private Vector3 PlayerAttackingScale;
    //private Vector3 WeaponArmOffset;
    private GameObject WeaponArm;
    private GameObject Destination;
    private GameObject LastRecordedPosition;
    private GameObject[] PlayerPositions;
    private Coroutine PlayerRoutine;
    private Coroutine FlickerRoutine;
    private Coroutine JumpRoutine;
    private Quaternion DefaultRotation;
    //private RaycastHit2D PlayerRayCastHit;
    private WorldManager Manager;
    private MusicManager MusicManager;
    private UIInventory Inventory;
    private SpriteRenderer ThisSpriteRenderer;

    void Start()
    {
        PlayerPositions = new GameObject[3];
        PlayerPositions[0] = GameObject.FindGameObjectWithTag("LeftPosition");
        PlayerPositions[1] = GameObject.FindGameObjectWithTag("DefaultPosition");
        PlayerPositions[2] = GameObject.FindGameObjectWithTag("RightPosition");
        Manager = GameObject.FindGameObjectWithTag("WorldManager").GetComponent<WorldManager>();
        MusicManager = GameObject.FindGameObjectWithTag("WorldManager").GetComponent<MusicManager>();
        Inventory = GameObject.Find("InventoryUI").GetComponent<UIInventory>();
        DefaultPosition = PlayerPositions[1].transform.position;
        DefaultRotation = PlayerPositions[1].transform.rotation;

        WeaponPositionOffset = new Vector2(0, -1);
        PlayerWeaponShift = new Vector2(0, -1);

        transform.position = DefaultPosition;
        transform.rotation = DefaultRotation;

        //PlayerTouched = false;
        //NoTouchZonePressed = false;

        WeaponArm = Instantiate(new GameObject(), GetWeaponArmLocation(), transform.rotation);
        WeaponArm.transform.SetParent(transform);
        WeaponArm.gameObject.tag = "WeaponHolder";
        WeaponArm.name = "WeaponArm";
        WeaponDefaultScale = WeaponArm.transform.localScale;
        TelekinesisLines.transform.SetParent(WeaponArm.transform);

        WeaponCooldownShader.gameObject.SetActive(false);
        SuperCooldownShader.gameObject.SetActive(false);

        PlayerDefaultScale = transform.localScale;
        PlayerAttackingScale = new Vector3(0.25f, 0.25f, transform.localScale.z);

        WeaponPosition = (Vector2)transform.position - WeaponPositionOffset;
        CurrentWeaponIndex = 0;
        LastWeaponIndex = -1;
        LastSupermoveIndex = -1;
        CurrentPositionIndex = 1;
        RevivesNeeded = 1;
        IRNTime = 60;
        //InputThreshold = PlayerPrefs.GetInt("InputThreshold", 33) / 33;
        //AngularTolerance = PlayerPrefs.GetInt("AngularTolerance", 60);
        PlayerCanMove = true;
        IsAlive = true;
        OriginalShade = this.gameObject.GetComponent<SpriteRenderer>().color;
        ThisSpriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();

        TelekinesisVisuals.SetActive(false);
        TelekinesisLines.SetActive(false);

        Invoke("IncreaseRevivesNeeded", IRNTime);

        InvincibilityPostProcess.enabled = false;
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
        /*
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
            else if (Gesture.phase == TouchPhase.Moved && PlayerTouched && Vector2.Distance(PlayerTappedAtPosition, Camera.main.ScreenToWorldPoint(Gesture.position)) > InputThreshold)
            {
                PlayerTouched = false;
                if (PlayerCanMove)
                {
                    Vector2 SwipeUpAt = Camera.main.ScreenToWorldPoint(Gesture.position);
                    float SwipeAngle = Mathf.Atan2(SwipeUpAt.y - PlayerTappedAtPosition.y, SwipeUpAt.x - PlayerTappedAtPosition.x) * 180 / Mathf.PI;

                    if ((SwipeAngle > (180 - (AngularTolerance / 2))) || (SwipeAngle < (-180 + (AngularTolerance / 2))))
                    {
                        if (CurrentPositionIndex > 0)
                        {
                            PlayerAnimationController.SetTrigger("LungeLeft");
                        }
                    }
                    else if (SwipeAngle > -1 * (AngularTolerance / 2) && SwipeAngle < (AngularTolerance / 2))
                    {
                        if (CurrentPositionIndex < 2)
                        {
                            PlayerAnimationController.SetTrigger("LungeRight");
                        }
                    }
                    else if (SwipeAngle > 90 - (AngularTolerance / 2) && SwipeAngle < 90 + (AngularTolerance / 2))
                    {
                        PerformJump();
                    }
                    else if (SwipeAngle > -90 - (AngularTolerance / 2) && SwipeAngle < -90 + (AngularTolerance / 2))
                    {
                        PerformSuperMove();
                    }
                }
            }
        }

        else if (NoTouchZonePressed && Input.touches.Length == 0)
        {
            NoTouchZonePressed = false;
        }
        */
    }

    void SpawnWeapon(int ElementIndex)
    {
        WeaponArm.transform.SetPositionAndRotation(GetWeaponArmLocation(), transform.rotation);
        WeaponPosition = (Vector2)transform.position - WeaponPositionOffset;
        CurrentWeapon  = (ObjectPooler.CentralObjectPool.SpawnFromPool(Weapons[CurrentWeaponIndex].name, WeaponPosition, transform.rotation)).GetComponent<BaseWeapon>();
        CurrentWeapon.transform.SetParent(WeaponArm.transform);
        CurrentWeapon.SetCurrentElemental(ElementIndex);
        CurrentWeaponCooldown = CurrentWeapon.GetComponent<BaseWeapon>().Cooldown;
        PlayerCanAttack = true;
        UpdateWeaponIcon(CurrentWeaponIndex, ElementIndex);
        TurnAllVisualsOff();
        if (CurrentWeaponIndex < 4)
        {
            TelekinesisVisuals.transform.GetChild(ElementIndex).gameObject.SetActive(true);
            TelekinesisLines.transform.GetChild(ElementIndex).gameObject.SetActive(true);
        }
    }

    void TurnAllVisualsOff()
    {
        foreach(Transform Current in TelekinesisVisuals.transform)
        {
            Current.gameObject.SetActive(false);
        }
        foreach (Transform Current in TelekinesisLines.transform)
        {
            Current.gameObject.SetActive(false);
        }
    }

    Vector2 GetWeaponArmLocation()
    {
        return transform.position - (Vector3)PlayerWeaponShift;
        //return transform.position - WeaponArmOffset;
    }

    public void PerformJump()
    {
        if (PlayerCanMove)
        {
            RestoreShade();

            InvincibilityPostProcess.enabled = true;

            HealthComponent.GetComponent<PlayerHitBox>().TryJump(PlayerJumpDuration);
            PlayerCanMove = false;

            AlternateButtonsVisibility(false);

            JumpRoutine = StartCoroutine(GoTranslucent());
            Invoke("ResetCanAttackAndMove", PlayerJumpDuration);
        }
    }

    void ResetCanAttackAndMove()
    {
        JumpRoutine = StartCoroutine(GoOpaque());
        PlayerCanMove = true;
        InvincibilityPostProcess.enabled = false;

        AlternateButtonsVisibility(true);
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
        if (MovementCoroutineRunning)
        {
            StopCoroutine(PlayerRoutine);
        }

        if (!((Vector2)Destination.transform.position == (Vector2)PlayerPositions[0].transform.position || (Vector2)Destination.transform.position == (Vector2)PlayerPositions[1].transform.position) || (Vector2)Destination.transform.position == (Vector2)PlayerPositions[2].transform.position)
        {
            transform.position = Destination.transform.position;
        }
        else
        {
            transform.position = LastRecordedPosition.transform.position;
        }

        WeaponPosition = (Vector2)transform.position - WeaponPositionOffset;
        PlayerCanMove = true;
    }

    void AttackPositioningAdjustment(Vector2 FinalAttackPosition, bool ResetNeeded)
    {
        if (MovementCoroutineRunning)
        {
            StopCoroutine(PlayerRoutine);
        }

        transform.position = FinalAttackPosition;

        if (ResetNeeded)
        {
            PlayerCanMove = true;
            transform.localScale = PlayerDefaultScale;
            //WeaponPosition = (Vector2)transform.position + WeaponPositionOffset; DUE TO
            //WeaponPosition = (Vector2)transform.position - PlayerWeaponShift;
            //WeaponArm.transform.SetParent(transform);
            //HealthComponent.transform.SetParent(transform);

            if (!(FinalAttackPosition == (Vector2)PlayerPositions[0].transform.position || FinalAttackPosition == (Vector2)PlayerPositions[1].transform.position) || FinalAttackPosition == (Vector2)PlayerPositions[2].transform.position)
            {
                transform.position = LastRecordedPosition.transform.position;
            }

        }
    }

    public void StartAttackRetreat()
    {
        PlayerCanMove = false;
        AttackStartedAt = transform.position;

        //transform.DetachChildren();
        //PlayerShadow.transform.SetParent(transform);

        // Vector2 RetreatPoint = transform.position - new Vector3(0.0f, 1.0f, 0.0f); DUE TO
        Vector2 RetreatPoint = transform.position + (Vector3)PlayerWeaponShift;

        TelekinesisVisuals.SetActive(true);
        TelekinesisLines.SetActive(true);

        PlayerRoutine = StartCoroutine(PlayerAttackMovement(RetreatPoint, RetreatSpeed, PlayerAttackingScale, false));

        PlayerAnimationController.SetTrigger("WeaponAttackLoadedRight");
    }

    public void EndAttackRetreat()
    {
        Invoke("DelayedEndAttackRetreat", 0.1f);
    }

    void DelayedEndAttackRetreat()
    {
        TelekinesisVisuals.SetActive(false);
        TelekinesisLines.SetActive(false);
        StopCoroutine(PlayerRoutine);
        PlayerRoutine = StartCoroutine(PlayerAttackMovement(AttackStartedAt, RetreatSpeed, PlayerDefaultScale, true));

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
            //Destroy(CurrentWeapon.gameObject);
            CurrentWeapon.gameObject.SetActive(false);
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

    public void SetPlayerActivityPermission(bool Value)
    {
        PlayerCanAttack = Value;
        PlayerCanMove = Value;
        PlayerCanSupermove = Value;
    }

    public void SetCurrentWeaponCooldown(float NewCooldown)
    {
        CurrentWeaponCooldown = NewCooldown;
    }

    void WeaponCooledDown()
    {
        SetPlayerCanAttack(true);
        WeaponCooldownShader.gameObject.SetActive(false);
    }

    public void SetCurrentWeapon(int NewIndex)
    {
        CurrentWeaponIndex = NewIndex;
    }

    public void SetCurrentSupermove(int NewIndex)
    {
        CurrentSupermoveIndex = NewIndex;
        UpdateSuperIcon();
    }

    public void NewWeaponSelected(int NewIndex, int ElementIndex)
    {
        if(PlayerCanMove)
        {
            DestroyWeapon();
            SetCurrentWeapon(NewIndex);
            ResetToPlayerTransform();
            SpawnWeapon(ElementIndex);
        }
        else
        {
            Inventory.ErrorEncountered();
        }
    }

    public void ResetToDefaultTransform()
    {
        WeaponArm.transform.SetPositionAndRotation(DefaultPosition, DefaultRotation);
    }

    public void ResetToPlayerTransform()
    {
        WeaponArm.transform.SetPositionAndRotation(GetWeaponArmLocation(), transform.rotation);
    }

    public void PerformSuperMove()//int Index
    {
        if (PlayerCanSupermove && CurrentSupermoveIndex >= 0)
        {
            Supermoves[CurrentSupermoveIndex].PerformSuperMove();
            RunSuperCooldownShader();
            PlayerCanSupermove = false;
            Invoke("ResetPerformSuperMove", SupermoveCountdown);
        }
    }

    public void PickedUpWeapon(string WeaponName)
    {
        int WeaponIndex = 0;
        foreach (BaseWeapon CurrentWeapon in Weapons)
        {
            if (CurrentWeapon.name == WeaponName)
            {
                if (WeaponIndex > LastWeaponIndex)
                {
                    Inventory.UnlockButton(WeaponName);
                    LateWeaponIndex = WeaponIndex;
                    Manager.LockWeaponSpawn();
                    LastWeaponIndex++;
                }
                break;
            }
            WeaponIndex++;
        }
    }

    public void LateEquipWeapon()
    {
        NewWeaponSelected(LateWeaponIndex, 0);
    }

    public void LateEquipSupermove()
    {
        SetCurrentSupermove(TempCurrentSupermoveIndex);
    }

    public void PickedUpSupermove(string SupermoveName)
    {
        int SupermoveIndex = 0;
        foreach (BaseSupermove CurrentSupermove in Supermoves)
        {
            if (CurrentSupermove.name == SupermoveName)
            {
                if (SupermoveIndex > LastSupermoveIndex)
                {
                    TempCurrentSupermoveIndex = SupermoveIndex;
                    Inventory.UnlockButton(SupermoveName);
                    Manager.LockSupermoveSpawn();
                    LastSupermoveIndex++;
                }
                break;
            }

            SupermoveIndex++;
        }

    }

    public void PickedUpHealth()
    {
        HealthComponent.GetComponent<PlayerHitBox>().UpdateHealth(1);
    }

    void ResetPerformSuperMove()
    {
        PlayerCanSupermove = true;
        SuperCooldownShader.gameObject.SetActive(false);
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

    public void Revived()
    {
        Inventory.InventoryButtonOn();
        Inventory.ProxyCallPromptSwell(false, Inventory.RevivePrompt);
        HealthComponent.GetComponent<PlayerHitBox>().UpdateHealth(5);
        HealthComponent.GetComponent<PlayerHitBox>().PlayerRevived();
        GameController.GameControl.RevivesSpent(RevivesNeeded);
        Time.timeScale = 1;
        MusicManager.SetMusicVolume(1);
        Invoke("CheckRevivePromptIsClosed", 0.5f);

        //Maybe
        GooglePlayAuthenticator.RegisterAchievement("CgkInJX8ptwQEAIQIg", 100.0f);
    }

    void CheckRevivePromptIsClosed()
    {
        Inventory.RevivePrompt.SetActive(false);
    }

    public void RunWeaponCooldownShader()
    {
        WeaponCooldownShader.gameObject.SetActive(true);
        WeaponCooldownProgress = 0;
        StartCoroutine(VisualizeWeaponCooldown());
    }

    public void RunSuperCooldownShader()
    {
        SuperCooldownShader.gameObject.SetActive(true);
        SuperCooldownProgress = 0;
        StartCoroutine(VisualizeSuperCooldown());
    }

    IEnumerator PlayerMoveTo(GameObject Destination)
    {
        MovementCoroutineRunning = true;
        LastRecordedPosition = Destination;
        AlternateButtonsVisibility(false);
        while (Vector2.Distance(transform.position, Destination.transform.position) > 0.05f)
        {
            transform.position = Vector2.Lerp(transform.position, Destination.transform.position, PlayerDashSpeed * Time.deltaTime);
            yield return null;
        }
        MovementCoroutineRunning = false;
        AdjustPlayerPosition(Destination);
        AlternateButtonsVisibility(true);
    }

    IEnumerator PlayerAttackMovement(Vector2 Destination, float Speed, Vector2 Scale, bool IsResetNeeded)
    {
        MovementCoroutineRunning = true;
        while (Vector2.Distance(transform.position, Destination) > 0.05f)
        {
            transform.position = Vector2.Lerp(transform.position, Destination, Speed * Time.deltaTime);
            //transform.localScale = Vector2.Lerp(transform.localScale, Scale, Speed * Time.deltaTime);
            yield return null;
        }
        MovementCoroutineRunning = false;
        AttackPositioningAdjustment(Destination, IsResetNeeded);
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

    public bool GetOngoingDamage()
    {
        return FlickerCoroutineRunning;
    }

    IEnumerator PlayerDamageFlicker()
    {
        FullScreenIndicator.Damaged();
        FlickerCoroutineRunning = true;
        int Flickers = 0;
        bool TransparentDone;
        while (Flickers < 5)
        {
            TransparentDone = false;
            while (!TransparentDone && TheColorIsNotSimilar(DamageShade))
            {
                ThisSpriteRenderer.color = Color.Lerp(ThisSpriteRenderer.color, DamageShade, 12 * Time.deltaTime);
                yield return null;
            }
            ThisSpriteRenderer.color = DamageShade;
            TransparentDone = true;
            while (TheColorIsNotSimilar(OriginalShade))
            {
                ThisSpriteRenderer.color = Color.Lerp(ThisSpriteRenderer.color, OriginalShade, 12 * Time.deltaTime);
                yield return null;
            }
            ThisSpriteRenderer.color = OriginalShade;
            Flickers++;
        }
        FlickerCoroutineRunning = false;
        yield return new WaitForSeconds(2);
    }

    IEnumerator VisualizeWeaponCooldown()
    {
        WeaponCooldownCoroutineRunning = true;
        while (CurrentWeaponCooldown > WeaponCooldownProgress)
        {
            WeaponCooldownShader.GetComponent<Image>().fillAmount = (CurrentWeaponCooldown - WeaponCooldownProgress) / CurrentWeaponCooldown;
            yield return null;
        }
        WeaponCooldownCoroutineRunning = false;

        yield return new WaitForSeconds(1);
    }

    IEnumerator VisualizeSuperCooldown()
    {
        SuperCooldownCoroutineRunning = true;
        while (SupermoveCountdown > SuperCooldownProgress)
        {
            SuperCooldownShader.GetComponent<Image>().fillAmount = (SupermoveCountdown - SuperCooldownProgress) / SupermoveCountdown;
            yield return null;
        }
        SuperCooldownCoroutineRunning = false;

        yield return new WaitForSeconds(1);
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

    void UpdateWeaponIcon(int WeaponInd, int Element)
    {
        WeaponIcon.GetComponent<Image>().sprite = Weapons[WeaponInd].GetIcon();
        Inventory.ColorWeaponIconBG(Element);
    }

    void UpdateSuperIcon()
    {
        SuperIcon.GetComponent<Image>().sprite = Supermoves[CurrentSupermoveIndex].GetIcon();
    }

    public bool GetPlayerCanMove()
    {
        return PlayerCanMove;
    }

    public void SetPlayerCanMove(bool Value)
    {
        PlayerCanMove = Value;
    }

    public bool GetPlayerCanAttack()
    {
        return PlayerCanAttack;
    }

    public void SetPlayerAttacked(bool Value)
    {
        PlayerAttacked = Value;
    }

    public bool GetPlayerAttacked()
    {
        return PlayerAttacked;
    }

    public void AlternateButtonsVisibility(bool MakeAllActive)
    {
        if (GetComponent<AlternateControls>())
        {
            foreach (Button CurrentButton in AlternateControlButtons)
            {
                CurrentButton.interactable = MakeAllActive;
            }
        }
    }

    public void AlternateButtonsActivity(bool MakeAllActive)
    {
        if (GetComponent<AlternateControls>())
        {
            foreach (Button CurrentButton in AlternateControlButtons)
            {
                CurrentButton.gameObject.SetActive(MakeAllActive);
            }
        }
    }

}
