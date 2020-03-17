using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[System.Serializable]
public struct WeaponDetails
{
    public string WeaponName;
    public Sprite WeaponIcon;
    public Sprite PickupIcon;
    public Image PromptImage;

    public string GetWeaponName()
    {
        return WeaponName;
    }

    public Sprite GetWeaponIcon()
    {
        return WeaponIcon;
    }

    public Sprite GetPickupIcon()
    {
        return PickupIcon;
    }

    public Image GetPromptImage()
    {
        return PromptImage;
    }
}

[System.Serializable]
public struct SupermoveDetails
{
    public string SupermoveName;
    public Sprite SupermoveIcon;
    public Sprite PickupIcon;
    public Image PromptImage;

    public string GetSupermoveName()
    {
        return SupermoveName;
    }

    public Sprite GetSupermoveIcon()
    {
        return SupermoveIcon;
    }

    public Sprite GetPickupIcon()
    {
        return PickupIcon;
    }

    public Image GetPromptImage()
    {
        return PromptImage;
    }
}

public class WorldManager : MonoBehaviour
{
    public int EnemySpawnPercentage;
    public bool HasDayNightCycle = true;
    public float SpawnPercentageProbability;
    public float HealthSpawnPercentageProbability = 40;
    public Color DayColor;
    public Color NightBackgroundColor;
    public Color NightForegroundColor;
    public Sprite HealthPickupIcon;
    public Material EnemySuperSkin;
    public GameObject Messenger;
    public GameObject[] Obstacles;
    public GameObject[] Decorations;
    public GameObject[] Enemies;
    public GameObject[] MessengerMarkers;
    public BasePickup WeaponPickup;
    public WeaponDetails[] WeaponsList;
    public SpriteRenderer NightBackground;
    public SpriteRenderer NightForeground;
    public SupermoveDetails[] SupermovesList;

    private int LastSelectedLane = 0;
    private int LastSelectedObstacle = 0;
    private int LastSelectedDecoration = 0;
    private int LastSelectedEnemy = 0;
    private int NumberOfDecorationsSpawned = 1;
    private int CurrentLastEnemyIndex;
    private int CurrentSuperEnemyIndex;
    private int OriginalEnemySpawnPercentage;
    private bool DoubleBumpDamageAtNight = false;
    private bool SpawnWeaponPickup = false;
    private bool SpawnSupermovePickup = false;
    private bool IsDay = true;
    private bool RegularMode = true;
    private bool SpecialMode;
    private bool ProjectilesDestroyEachOther;
    private bool IBallMoves;
    private float GameSpeed = 2;
    private float TimeSinceLastSpawn = 0.0f;
    private float SpawnTime = 1;
    private float DayNightCycleDuration = 60;
    private float SpawnDecorationsTime = 0.6f;
    private float EnemyUnlockTime = 45;
    private float WeaponUnlockTime = 50;
    private float OriginalSpawnPercentageProbability;
    private Vector2 SpawnLocation;
    private Queue<WeaponDetails> WeaponsPendingSpawn;
    private Queue<SupermoveDetails> SupermovesPendingSpawn;

    private Lane[] Lanes;

    void Start()
    {
        SetLanesOnScreen();
        RevertToDay();
        CurrentLastEnemyIndex = 2;
        CurrentSuperEnemyIndex = -1;
        IsDay = true;
        SpecialMode = false;
        ProjectilesDestroyEachOther = false;
        DoubleBumpDamageAtNight = false;
        IBallMoves = false;

        Invoke("UnlockNextEnemy", EnemyUnlockTime);

        if (HasDayNightCycle)
        {
            Invoke("DayNightCycle", DayNightCycleDuration);
        }
        
        WeaponsPendingSpawn = new Queue<WeaponDetails>(WeaponsList);
        SupermovesPendingSpawn = new Queue<SupermoveDetails>(SupermovesList);

        Physics2D.IgnoreLayerCollision(8, 12);
        Physics2D.IgnoreLayerCollision(9, 10); // Enemy with World Elements
        Physics2D.IgnoreLayerCollision(10, 10); //Enemy with Enemy
        Physics2D.IgnoreLayerCollision(9, 12);
        Physics2D.IgnoreLayerCollision(11, 12);

        OriginalSpawnPercentageProbability = SpawnPercentageProbability;
        OriginalEnemySpawnPercentage = EnemySpawnPercentage;
    }

    // Update is called once per frame
    void Update()
    {
        if (RegularMode)
        {
            TimeSinceLastSpawn += Time.deltaTime;
            if (TimeSinceLastSpawn > SpawnTime)
            {
                TimeSinceLastSpawn = 0.0f;
                NumberOfDecorationsSpawned = 1;
                ChooseWhatToSpawn();
            }
            else if (TimeSinceLastSpawn > (NumberOfDecorationsSpawned * SpawnDecorationsTime))
            {
                NumberOfDecorationsSpawned++;
                SpawnDecoration();
            }
        }
        else
        {
            //Special Mode
        }
    }

    void SetLanesOnScreen()
    {
        Lanes = GameObject.FindObjectsOfType<Lane>();
    }

    void SpawnObstacle()
    {
        if (Random.Range(0, 100) < SpawnPercentageProbability)
        {
            int SelectedLane = PickALane();
            int SelectedObstacle = PickAnObstacle();

            SpawnLocation = Lanes[SelectedLane].transform.position;

            if (Obstacles[SelectedObstacle].tag == "BigObstacle")
            {
                SpawnLocation = GetMidLaneLocation(SelectedLane);
            }

            //OPTChange - Instantiate(Obstacles[SelectedObstacle], SpawnLocation, Quaternion.identity);
            ObjectPooler.CentralObjectPool.SpawnFromPool(Obstacles[SelectedObstacle].name, SpawnLocation, Quaternion.identity);
            LastSelectedLane = SelectedLane;
            LastSelectedObstacle = SelectedObstacle;

            AdjustSpawnProbability(true);
        }
        else
        {
            SpawnPickup();
            AdjustEnemySpawnProbability(false);
        }
    }

    Vector2 GetMidLaneLocation(int Lane)
    {
        int SideLane = 0;
        switch (Lane)
        {
            case 0:
                SideLane = 1;
                break;
            case 1:
                if (Random.Range(0, 10) < 5)
                {
                    SideLane = 0;
                }
                else
                {
                    SideLane = 2;
                }
                break;
            case 2:
                SideLane = 1;
                break;
        }
        return (Lanes[Lane].transform.position + Lanes[SideLane].transform.position) / 2;
    }

    void SpawnDecoration()
    {
        if (Random.Range(0, 100) < SpawnPercentageProbability)
        {
            int SelectedDecoration = PickADecoration();

            SpawnLocation = new Vector2((Random.Range(-300, 300) / 100), 6);
            ObjectPooler.CentralObjectPool.SpawnFromPool(Decorations[SelectedDecoration].name, SpawnLocation, Quaternion.identity);
            LastSelectedDecoration = SelectedDecoration;
        }
    }

    void SpawnEnemy()
    {
        bool ShouldSpawn = true;

        if (Random.Range(0, 100) < SpawnPercentageProbability)
        {
            int SelectedEnemy = PickAnEnemy();
            int SelectedLane = PickALane();

            SpawnLocation = Lanes[SelectedLane].transform.position;
            GameObject EnemyToSpawn = Enemies[SelectedEnemy];

            if (EnemyToSpawn.GetComponentInChildren<SpecialClassEnemy>() && (FindObjectOfType<SpecialClassEnemy>()))
            {
                ShouldSpawn = false;
            }
            if (ShouldSpawn)
            {
                //OPTChange - GameObject SpawnedEnemy = Instantiate(EnemyToSpawn, SpawnLocation, Quaternion.identity);
                GameObject SpawnedEnemy = ObjectPooler.CentralObjectPool.SpawnFromPool(EnemyToSpawn.name, SpawnLocation, Quaternion.identity);
                if (DoubleBumpDamageAtNight && !IsDay)
                {
                    SpawnedEnemy.GetComponent<BaseEnemy>().SetIsArmored(true);
                }
                if (SelectedEnemy <= CurrentSuperEnemyIndex)
                {
                    SpawnedEnemy.GetComponent<BaseEnemy>().SetIsSuper(EnemySuperSkin);
                }
                LastSelectedEnemy = SelectedEnemy;
                LastSelectedLane = SelectedLane;
            }
            AdjustSpawnProbability(true);
            AdjustEnemySpawnProbability(true);
        }
        else
        {
            AdjustSpawnProbability(false);
            AdjustEnemySpawnProbability(false);
        }
    }

    void SpawnPickup()
    {
        int SelectedLane = PickALane();
        SpawnLocation = Lanes[SelectedLane].transform.position;

        if (SpawnWeaponPickup && Random.Range(0, 100) < SpawnPercentageProbability)
        {
            //BasePickup Pickup = Instantiate(WeaponPickup, SpawnLocation, Quaternion.identity);
            BasePickup Pickup = (ObjectPooler.CentralObjectPool.SpawnFromPool(WeaponPickup.name, SpawnLocation, Quaternion.identity)).GetComponent<BasePickup>();
            Pickup.SetWeaponName(WeaponsPendingSpawn.Peek().GetWeaponName());
            Pickup.SetWeaponIcon(WeaponsPendingSpawn.Peek().GetPickupIcon());
            Pickup.SetWeaponImage(WeaponsPendingSpawn.Peek().GetWeaponIcon());
            Pickup.SetPromptImage(WeaponsPendingSpawn.Peek().GetPromptImage());
            Pickup.SetPickupType(PickupClass.Weapon);
            //Pickup.SetWeaponDesc(WeaponsPendingSpawn.Peek().GetWeaponDescription());
            //Pickup.SetUsefulAgainst(WeaponsPendingSpawn.Peek().GetUses());
            LastSelectedLane = SelectedLane;
        }

        else if (SpawnSupermovePickup && Random.Range(0, 100) < SpawnPercentageProbability)
        {
            BasePickup Pickup = (ObjectPooler.CentralObjectPool.SpawnFromPool(WeaponPickup.name, SpawnLocation, Quaternion.identity)).GetComponent<BasePickup>();
            Pickup.SetWeaponName(SupermovesPendingSpawn.Peek().GetSupermoveName());
            Pickup.SetWeaponIcon(SupermovesPendingSpawn.Peek().GetPickupIcon());
            Pickup.SetWeaponImage(SupermovesPendingSpawn.Peek().GetSupermoveIcon());
            Pickup.SetPromptImage(SupermovesPendingSpawn.Peek().GetPromptImage());
            Pickup.SetPickupType(PickupClass.Supermove);

            //Pickup.SetWeaponDesc(SupermovesPendingSpawn.Peek().GetSupermoveDescription());
            //Pickup.SetUsefulAgainst(SupermovesPendingSpawn.Peek().GetUses());
            LastSelectedLane = SelectedLane;
        }
        else if (Random.Range(0, 100) < HealthSpawnPercentageProbability)
        {
            BasePickup Pickup = (ObjectPooler.CentralObjectPool.SpawnFromPool(WeaponPickup.name, SpawnLocation, Quaternion.identity)).GetComponent<BasePickup>();
            Pickup.SetWeaponName("Healthy");
            Pickup.SetWeaponIcon(HealthPickupIcon);
            Pickup.SetPickupType(PickupClass.Health);
            LastSelectedLane = SelectedLane;
        }

        else
        {
            AdjustSpawnProbability(false);
        }
    }

    int PickALane()
    {
        bool Picked = false;
        int RandomLane = 0;

        while (!Picked)
        {
            RandomLane = Random.Range(0, (Lanes.Length));
            if (RandomLane != LastSelectedLane)
                Picked = true;
        }
        return RandomLane;
    }

    int PickAnObstacle()
    {
        bool Picked = false;
        int RandomObstacle = 0;

        while (!Picked)
        {
            RandomObstacle = Random.Range(0, (Obstacles.Length));
            if (RandomObstacle != LastSelectedObstacle)
                Picked = true;
        }
        return RandomObstacle;
    }

    int PickADecoration()
    {
        bool Picked = false;
        int RandomDecoration = 0;

        while (!Picked)
        {
            RandomDecoration = Random.Range(0, (Decorations.Length));
            if (RandomDecoration != LastSelectedDecoration)
                Picked = true;
        }
        return RandomDecoration;
    }

    int PickAnEnemy()
    {
        bool Picked = false;
        int RandomEnemy = 0;

        while (!Picked)
        {
            RandomEnemy = Random.Range(0, CurrentLastEnemyIndex);
            if (RandomEnemy != LastSelectedEnemy)
                Picked = true;
        }
        return RandomEnemy;
    }

    void ChooseWhatToSpawn()
    {
        int Rando = Random.Range(0, 100);

        if (Rando < (100 - EnemySpawnPercentage))
            SpawnObstacle();
        else
        {
            if(PlayerPrefs.GetInt("EasyDifficulty",0) == 1 && SpawnedEnough()){
                return;
            }
            SpawnEnemy();
        }
    }

    bool SpawnedEnough()
    {
        if (GameObject.FindObjectsOfType<BaseEnemy>().Length > 2)
        {
            return true;
        }
        return false;
    }

    void UnlockNextEnemy()
    {
        CurrentLastEnemyIndex++;
        if (CurrentLastEnemyIndex < Enemies.Length)
        {
            EnemyUnlockTime += 5;
            if (EnemyUnlockTime > 90)
            {
                EnemyUnlockTime = 90;
            }
            SendMessenger(Enemies[CurrentLastEnemyIndex].name + "\n has joined the fray");
            Invoke("UnlockNextEnemy", EnemyUnlockTime);
        }
    }

    void UnlockWeaponSpawn()
    {
        SpawnWeaponPickup = true;
    }

    void UnlockSupermoveSpawn()
    {
        SpawnSupermovePickup = true;
    }

    void DestroyExistingPickups()
    {
        BasePickup[] PickupsToDestroy = GameObject.FindObjectsOfType<BasePickup>();
        foreach (BasePickup CurrentPickup in PickupsToDestroy)
        {
            if (CurrentPickup.GetPickupType() != PickupClass.Health)
            {
                CurrentPickup.gameObject.SetActive(false);
            }
        }
    }

    public void StartSpawningWeapons()
    {
        Invoke("UnlockWeaponSpawn", (WeaponUnlockTime / 2));
    }

    public void LockWeaponSpawn()
    {
        WeaponsPendingSpawn.Dequeue();
        SpawnWeaponPickup = false;
        DestroyExistingPickups();

        if (WeaponsPendingSpawn.Count > 0)
        {
            Invoke("UnlockWeaponSpawn", WeaponUnlockTime);
        }
        else
        {
            Invoke("UnlockSupermoveSpawn", WeaponUnlockTime);
        }
    }

    public void LockSupermoveSpawn()
    {
        SupermovesPendingSpawn.Dequeue();
        SpawnSupermovePickup = false;
        DestroyExistingPickups();

        if (SupermovesPendingSpawn.Count > 0)
        {
            Invoke("UnlockSupermoveSpawn", WeaponUnlockTime);
        }
        else
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScoring>().SetAllWeaponsUnlocked(true);
        }
    }

    public float GetGameSpeed()
    {
        return GameSpeed;
    }

    void DayNightCycle()
    {
        if (IsDay)
        {
            IsDay = false;
            StartCoroutine(ChangeTimeOfDay(NightBackground, NightBackgroundColor));
            StartCoroutine(ChangeTimeOfDay(NightForeground, NightForegroundColor));
        }
        else
        {
            IsDay = true;
            StartCoroutine(ChangeTimeOfDay(NightBackground, DayColor));
            StartCoroutine(ChangeTimeOfDay(NightForeground, DayColor));
        }

        Invoke("DayNightCycle", DayNightCycleDuration);
    }

    void ModeCycle()
    {
        RegularMode = !RegularMode;
        SpecialMode = !SpecialMode;
    }

    bool TheColorIsNotSimilar(SpriteRenderer Layer, Color GoToColor)
    {
        Color CurrentColor = Layer.color;
        bool Result = true;
        if ((Mathf.Abs(CurrentColor.r - GoToColor.r) < 0.075f) && (Mathf.Abs(CurrentColor.g - GoToColor.g) < 0.075f) && (Mathf.Abs(CurrentColor.b - GoToColor.b) < 0.075f))
        {
            Result = false;
        }
        return Result;
    }

    void RevertToDay()
    {
        NightBackground.color = DayColor;
        NightForeground.color = DayColor;
        NightBackground.gameObject.SetActive(false);
        NightForeground.gameObject.SetActive(false);

        BaseEnemy[] AllEnemies = GameObject.FindObjectsOfType<BaseEnemy>();
        foreach (BaseEnemy CurrentEnemy in AllEnemies)
        {
            CurrentEnemy.SetIsArmored(false);
        }
    }

    void AdjustSpawnProbability(bool SomethingSpawned)
    {
        if (SomethingSpawned)
        {
            SpawnPercentageProbability -= 10;
            if (SpawnPercentageProbability < OriginalSpawnPercentageProbability)
            {
                SpawnPercentageProbability = OriginalSpawnPercentageProbability;
            }
        }
        else
        {
            SpawnPercentageProbability += 10;
            if (SpawnPercentageProbability > 90)
            {
                SpawnPercentageProbability = 90;
            }
        }
    }

    void AdjustEnemySpawnProbability(bool EnemySpawned)
    {
        if (EnemySpawned)
        {
            EnemySpawnPercentage -= 10;
            if (EnemySpawnPercentage < OriginalEnemySpawnPercentage)
            {
                EnemySpawnPercentage = OriginalEnemySpawnPercentage;
            }
        }
        else
        {
            EnemySpawnPercentage += 10;
            if (EnemySpawnPercentage > 90)
            {
                EnemySpawnPercentage = 90;
            }
        }
    }

    public void IncreaseSpawnProbability()
    {
        OriginalSpawnPercentageProbability += 10;
        if (OriginalSpawnPercentageProbability > 85)
        {
            OriginalSpawnPercentageProbability = 85;
        }
        SpawnPercentageProbability = OriginalSpawnPercentageProbability;

        OriginalEnemySpawnPercentage += 5;
        if (OriginalEnemySpawnPercentage > 65)
        {
            OriginalEnemySpawnPercentage = 65;
        }
        EnemySpawnPercentage = OriginalEnemySpawnPercentage;

        HealthSpawnPercentageProbability -= 10;
        if (HealthSpawnPercentageProbability < 10)
        {
            HealthSpawnPercentageProbability = 10;
        }

        StartCoroutine(EurusTimer(3, "Enemy spawn frequency increased"));
    }

    //This is the second step up in difficulty.
    public void SetProjectilesDestroyEachOther(bool NewValue)
    {
        ProjectilesDestroyEachOther = NewValue;
        StartCoroutine(EurusTimer(1.5f, "Enemy Projectiles will now \n destroy yours"));
        Invoke("SetDoubleDamageAtNight", 60);
    }

    //This is the third step up in difficulty.
    public void SetDoubleDamageAtNight()
    {
        DoubleBumpDamageAtNight = true;
        SendMessenger("Bumping into enemies at night \n deals double damage");
        Invoke("MakeNextEnemySuper", 45);
    }

    //This is the fourth step up in difficulty.
    public void MakeNextEnemySuper()
    {
        CurrentSuperEnemyIndex++;

        SendMessenger(Enemies[CurrentSuperEnemyIndex].name + "\n will now be Armored");


        if (CurrentSuperEnemyIndex < (Enemies.Length - 1))
        {
            Invoke("MakeNextEnemySuper", 30);
        }

        else
        {
            Invoke("ChangeLanePositions", 30);
        }
    }

    //This is the fifth step up in difficulty.
    public void ChangeLanePositions()
    {
        SendMessenger("Enemy Attack Timings \n changed");
        Lanes[0].transform.SetPositionAndRotation(new Vector2(Lanes[0].transform.position.x, Lanes[0].transform.position.y + 1), Lanes[0].transform.rotation);
        Lanes[1].transform.SetPositionAndRotation(new Vector2(Lanes[1].transform.position.x, Lanes[1].transform.position.y + 1), Lanes[1].transform.rotation);
        Lanes[2].transform.SetPositionAndRotation(new Vector2(Lanes[2].transform.position.x, Lanes[2].transform.position.y + 1), Lanes[2].transform.rotation);
        Invoke("EnableIBallMovement", 30);
    }

    //This is the sixth step up in difficulty.
    public void EnableIBallMovement()
    {
        SendMessenger("I Ball will now be moving");
        IBallMoves = true;
    }

    public bool GetProjectilesDestroyEachOther()
    {
        return ProjectilesDestroyEachOther;
    }

    public bool GetIBallMoves()
    {
        return IBallMoves;
    }

    public void SendMessenger(string Message)
    {
        //GameObject SpawnedMessage = Instantiate(Messenger, MessengerMarkers[0].transform.position, MessengerMarkers[0].transform.rotation);
        GameObject SpawnedMessage = ObjectPooler.CentralObjectPool.SpawnFromPool(Messenger.name, MessengerMarkers[0].transform.position, MessengerMarkers[0].transform.rotation);
        SpawnedMessage.GetComponentInChildren<Messenger>().SetText(Message);
        SpawnedMessage.GetComponentInChildren<Messenger>().SetPositionMarkers(MessengerMarkers[1].transform.position, MessengerMarkers[2].transform.position);
    }

    public void SetRegularMode(bool Value)
    {
        RegularMode = Value;
    }

    IEnumerator EurusTimer(float Delay, string Message)
    {
        bool Done = false;
        while (!Done)
        {
            Done = true;
            yield return new WaitForSeconds(Delay);
        }
        SendMessenger(Message);
    }

    IEnumerator ChangeTimeOfDay(SpriteRenderer Layer, Color GoToColor)
    {
        Layer.gameObject.SetActive(true);
        while (TheColorIsNotSimilar(Layer, GoToColor))
        {
            Layer.color = Color.Lerp(Layer.color, GoToColor, Time.deltaTime);
            yield return null;
        }
        if (GoToColor == DayColor && Layer == NightBackground)
        {
            RevertToDay();
        }
        yield return new WaitForSeconds(2);
    }

}
