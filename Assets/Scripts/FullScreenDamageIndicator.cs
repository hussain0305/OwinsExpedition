using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullScreenDamageIndicator : MonoBehaviour
{
    public Color RestingColor;
    public Color DamagedLower;
    public Color DamagedUpper;
    public SpriteRenderer ThisSpriteRenderer;

    private bool FSFlickerCoroutineRunning;

    // Start is called before the first frame update
    void Start()
    {
        if (!ThisSpriteRenderer)
        {
            ThisSpriteRenderer = GetComponent<SpriteRenderer>();
        }

        FSFlickerCoroutineRunning = false;
    }

    public void Damaged()
    {
        if (!FSFlickerCoroutineRunning)
        {
            StartCoroutine(FullScreenDamageFlicker());
        }
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

    IEnumerator FullScreenDamageFlicker()
    {
        FSFlickerCoroutineRunning = true;
        int Flickers = 0;
        bool TransparentDone;
        while (Flickers < 4)
        {
            TransparentDone = false;
            while (!TransparentDone && TheColorIsNotSimilar(DamagedUpper))
            {
                ThisSpriteRenderer.color = Color.Lerp(ThisSpriteRenderer.color, DamagedUpper, 15 * Time.deltaTime);
                yield return null;
            }
            ThisSpriteRenderer.color = DamagedUpper;
            TransparentDone = true;
            while (TheColorIsNotSimilar(DamagedLower))
            {
                ThisSpriteRenderer.color = Color.Lerp(ThisSpriteRenderer.color, DamagedLower, 15 * Time.deltaTime);
                yield return null;
            }
            ThisSpriteRenderer.color = DamagedLower;
            Flickers++;
        }

        while (TheColorIsNotSimilar(RestingColor))
        {
            ThisSpriteRenderer.color = Color.Lerp(ThisSpriteRenderer.color, RestingColor, 10 * Time.deltaTime);
            yield return null;
        }
        ThisSpriteRenderer.color = RestingColor;
        FSFlickerCoroutineRunning = false;
    }
}
