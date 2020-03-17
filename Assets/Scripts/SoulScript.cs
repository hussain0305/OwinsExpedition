using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulScript : MonoBehaviour
{
    public float SoulSpeed = 1;
    public float SoulColorSpeed = 12;
    public float SoulSizeSpeed = 12;

    private bool CoroutineRunning;
    private Color CurrentSoulColor;
    private Vector3 EnlargedScale;
    private Coroutine SpriteCoroutine;
    private Rigidbody2D Body;
    private SpriteRenderer SoulSprite;
    
    // Start is called before the first frame update
    void Awake()
    {
        Body = GetComponent<Rigidbody2D>();
        SoulSprite = GetComponent<SpriteRenderer>();
    }

    public void MoveSoul(Vector2 Direction, Sprite SoulImage, Vector3 CurrentScale)
    {
        SoulSprite.sprite = SoulImage;
        transform.localScale = 1.1f * CurrentScale;
        EnlargedScale = 1.4f * CurrentScale;

        Body.velocity = new Vector2(0, -2.5f);// SoulSpeed * Direction;

        if (CoroutineRunning)
        {
            StopCoroutine(SpriteCoroutine);
        }
        SpriteCoroutine = StartCoroutine(DisappearSoul());
    }

    IEnumerator DisappearSoul()
    {
        SoulSprite.color = new Color(1, 1, 1, 1);
        
        CoroutineRunning = true;

        while (TheColorIsNotSimilar(Color.clear))
        {
            SoulSprite.color = Color.Lerp(SoulSprite.color, Color.clear, SoulColorSpeed * Time.deltaTime);
            transform.localScale = Vector3.Lerp(transform.localScale, EnlargedScale, SoulSizeSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        SoulSprite.color = Color.clear;
        CoroutineRunning = false;
        this.gameObject.SetActive(false);
    }

    bool TheColorIsNotSimilar(Color GoToColor)
    {
        if (!SoulSprite)
        {
            SoulSprite = GetComponent<SpriteRenderer>();
        }
        Color CurrentColor = SoulSprite.color;

        bool Result = true;

        if ((Mathf.Abs(CurrentColor.r - GoToColor.r) < 0.075f) && (Mathf.Abs(CurrentColor.g - GoToColor.g) < 0.075f) && (Mathf.Abs(CurrentColor.b - GoToColor.b) < 0.075f))
        {
            Result = false;
        }
        return Result;
    }
}
