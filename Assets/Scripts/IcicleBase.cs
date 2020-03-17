using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcicleBase : MonoBehaviour
{
    public SpriteRenderer BaseSprite;
    public float BaseOpacitySpeed;
    // Start is called before the first frame update
    void Start()
    {
        if (!BaseSprite)
        {
            BaseSprite = GetComponent<SpriteRenderer>();
        }
        BaseSprite.color = new Color(1, 1, 1, 0);
        StartCoroutine(BaseColoration());
    }

    // Update is called once per frame
    void OnEnable()
    {
        BaseSprite.color = new Color(1, 1, 1, 0);
        StartCoroutine(BaseColoration());
    }

    IEnumerator BaseColoration()
    {
        float alpha = BaseSprite.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / BaseOpacitySpeed)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, 0.75f, t));
            BaseSprite.color = newColor;
            yield return null;
        }
        BaseSprite.color = new Color(1, 1, 1, 1);
    }
}
