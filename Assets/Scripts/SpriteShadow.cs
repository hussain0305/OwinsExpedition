using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteShadow : MonoBehaviour {

    private float ShadowSpeed = 0.075f;
    private float LargeShadowFactor = 1.1f;
    private float SmallShadowFactor = 0.95f;

    private Vector2 OriginalScale;
    private Vector2 LittleScale;
    private Vector2 BigScale;
    private Transform ThisTransform;
    private Coroutine ShadowCoroutine;
    private bool Enlarging = false;
    private bool CoroutineRunning = false;
    
    void Start(){
        ThisTransform = this.gameObject.transform;
        OriginalScale = ThisTransform.localScale;
        BigScale = LargeShadowFactor * OriginalScale;
        LittleScale = SmallShadowFactor * OriginalScale;
    }

    public void SetLargeShadowFactor(float NewValue)
    {
        LargeShadowFactor = NewValue;
        BigScale = LargeShadowFactor * OriginalScale;
    }

    public void SetSmallShadowFactor(float NewValue)
    {
        SmallShadowFactor = NewValue;
        LittleScale = SmallShadowFactor * OriginalScale;
    }

    public void StartFlyerShadowPulse()
    {
        Enlarging = true;
        StartCoroutine(ShadowPulse(BigScale));
    }

    public void SpeedUpShadowByFactor(float Factor)
    {
        ShadowSpeed = ShadowSpeed * Factor;
    }

    public void EnlargeShadow()
    {
        if (CoroutineRunning)
        {
            ThisTransform.localScale = OriginalScale;
            StopCoroutine(ShadowCoroutine);
        }
        ShadowCoroutine = StartCoroutine(ShadowGrow());
    }

    public void DiminishShadow()
    {
        if (CoroutineRunning)
        {
            ThisTransform.localScale = OriginalScale;
            StopCoroutine(ShadowCoroutine);
        }
        ShadowCoroutine = StartCoroutine(ShadowDiminish());
    }


    void ShadowFlipFlop()
    {
        if (!Enlarging && !CoroutineRunning)
        {
            StartCoroutine(ShadowPulse(BigScale));
        }
        else if (!CoroutineRunning)
        {
            StartCoroutine(ShadowPulse(LittleScale));
        }
    }

    IEnumerator ShadowPulse(Vector2 DesiredScale)
    {
        CoroutineRunning = true;
        while (Vector2.Distance(ThisTransform.localScale, DesiredScale) > 0.05f)
        {
            ThisTransform.localScale = Vector2.Lerp(ThisTransform.localScale, DesiredScale, ShadowSpeed);
            yield return new WaitForSeconds(0.1f);
        }
        
        Enlarging = !Enlarging;
        ThisTransform.localScale = DesiredScale;
        CoroutineRunning = false;
        ShadowFlipFlop();

        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator ShadowGrow()
    {
        CoroutineRunning = true;
        while (Vector2.Distance(ThisTransform.localScale, BigScale) > 0.05f)
        {
            ThisTransform.localScale = Vector2.Lerp(ThisTransform.localScale, BigScale, ShadowSpeed);
            yield return null;
        }

        ThisTransform.localScale = BigScale;
        CoroutineRunning = false;
    }

    IEnumerator ShadowDiminish()
    {
        CoroutineRunning = true;
        while (Vector2.Distance(ThisTransform.localScale, OriginalScale) > 0.05f)
        {
            ThisTransform.localScale = Vector2.Lerp(ThisTransform.localScale, LittleScale, ShadowSpeed);
            yield return null;
        }

        ThisTransform.localScale = LittleScale;
        CoroutineRunning = false;
    }
}
