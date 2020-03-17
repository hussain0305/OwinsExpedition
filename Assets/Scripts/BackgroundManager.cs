using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour {

    private Rigidbody2D Body;
    private GameObject CurrentBackground;
    private GameObject PlayerCharacter;
    private BaseEnemy[] ListOfEnemies;
    private WorldElementMarker[] Elements;
    private float WorldMovementSpeed = 2.25f;

    public GameObject BackgroundMarker_Top;
    public GameObject BackgroundMarker_Center;
    public GameObject BackgroundMarker_Bottom;
    public GameObject StartingBackground;
    public GameObject BackgroundBP;


    // Use this for initialization
    void Start () {
        PlayerCharacter = GameObject.FindGameObjectWithTag("Player");
        CurrentBackground = StartingBackground;
        Body = CurrentBackground.GetComponent<Rigidbody2D>();
        Body.velocity = new Vector2(0.0f, -WorldMovementSpeed);
        
        StartCoroutine(LayeringManager());
    }
	
	// Update is called once per frame
	void Update () {
        if (CurrentBackground.transform.position.y <= BackgroundMarker_Center.transform.position.y)
        {
            CurrentBackground = ObjectPooler.CentralObjectPool.SpawnFromPool(BackgroundBP.name, BackgroundMarker_Top.transform.position, Quaternion.identity);
            Body = CurrentBackground.GetComponent<Rigidbody2D>();
            Body.velocity = new Vector2(0.0f, -WorldMovementSpeed);
        }
    }

    IEnumerator LayeringManager()
    {
        while (true)
        {
            ListOfEnemies = GameObject.FindObjectsOfType<BaseEnemy>();
            foreach(BaseEnemy CurrentEnemy in ListOfEnemies)
            {
                if(CurrentEnemy.transform.position.y < PlayerCharacter.transform.position.y)
                {
                    CurrentEnemy.GetSpriteRenderer().sortingLayerName = "Player";
                    CurrentEnemy.GetSpriteRenderer().sortingOrder = 2;
                }
            }

            Elements = GameObject.FindObjectsOfType<WorldElementMarker>();

            foreach(WorldElementMarker CurrentElement in Elements)
            {
                if (CurrentElement.transform.position.y < PlayerCharacter.transform.position.y)
                {
                    CurrentElement.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
                    CurrentElement.GetComponent<SpriteRenderer>().sortingOrder = 2;
                }
            }
            yield return new WaitForSeconds(0.15f);
        }

    }
}

/*

*/
