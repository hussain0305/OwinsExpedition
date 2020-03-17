using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_DimishAndWave : MonoBehaviour
{
    public float VanishingSpeed;
    public Vector2 DeathAtScale;

    private Vector2 OriginalScale;
    // Start is called before the first frame update
    void Start()
    {
        OriginalScale = transform.localScale;
        StartCoroutine(Diminish());
    }
    
    IEnumerator Diminish()
    {
        while (transform.localScale.x > DeathAtScale.x)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, DeathAtScale, VanishingSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        Destroy(this.gameObject);
    }
}
