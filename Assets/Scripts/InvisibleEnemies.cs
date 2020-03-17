using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleEnemies : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MakeEnemiesInvisible());
        GetComponent<WorldManager>().ChangeLanePositions();
    }

    IEnumerator MakeEnemiesInvisible()
    {
        while (true)
        {
            BaseEnemy[] AllEnemies = GameObject.FindObjectsOfType<BaseEnemy>();
            foreach(BaseEnemy CurrentEnemy in AllEnemies)
            {
                CurrentEnemy.MakeInvisible();
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}
