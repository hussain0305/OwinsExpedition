using UnityEngine;
using System.Collections;

public class FireParticle : MonoBehaviour
{
    public bool DestroysSelf = true;
    public float LifeSpan = 2f;
    public Vector2 MinVelocity = new Vector2(-0.05f, 0.1f);
    public Vector2 MaxVelocity = new Vector2(0.05f, 0.2f);
    public Rigidbody2D Body;

    private float ActualLifeSpan;
    private float AlphaValue;
    private Color CurrentColor;
    private Vector3 OriginalScale;

    SpriteRenderer SpriteRendererComp;
    Color OriginalColor;

    Vector2 Velocity;

    void Awake()
    {
        SpriteRendererComp = GetComponent<SpriteRenderer>();
        OriginalColor = SpriteRendererComp.color;
        OriginalScale = transform.localScale;
    }
    void OnEnable()
    {
        Velocity = new Vector2(Random.Range(MinVelocity.x, MaxVelocity.x), Random.Range(MinVelocity.y, MaxVelocity.y));
        ActualLifeSpan = LifeSpan * Random.Range(0.25f, 0.5f); //(0.6f, 0.9f);
        AlphaValue = 1;

        transform.localScale = OriginalScale * Random.Range(0, 1.5f);

        if (Body)
        {
            Body.velocity = Velocity;
        }

        StopAllCoroutines();
        StartCoroutine(ParticleTransparency());

        if (DestroysSelf)
        {
            Invoke("DisableParticle", ActualLifeSpan);
        }
    }

    public void OnDisable()
    {
        StopAllCoroutines();
    }

    private void DisableParticle()
    {
        StopAllCoroutines();
        this.gameObject.SetActive(false);
    }

    public void SetParticleColor(Color PartColor)
    {
        if (Random.Range(1, 10) < 7)
        {
            SpriteRendererComp.color = PartColor;
            OriginalColor = PartColor;
        }
        else
        {
            SpriteRendererComp.color = Color.gray;
            OriginalColor = Color.gray;
        }
    }

    public void SetParticleVelocity(float NewVelocityFactor)
    {
        Velocity = Velocity * NewVelocityFactor;
        if (Body)
        {
            Body.velocity = Velocity;
        }
    }

    public void SetParticleVelocity(Vector3 NewVelocity)
    {
        Velocity = NewVelocity;
        if (Body)
        {
            Body.velocity = Velocity;
        }
    }

    IEnumerator ParticleTransparency()
    {
        while (AlphaValue > 0.1f)
        {
            AlphaValue -= 0.3f;
            transform.localScale = transform.localScale / 1.2f;
            SpriteRendererComp.color = new Color(SpriteRendererComp.color.r, SpriteRendererComp.color.g, SpriteRendererComp.color.b, AlphaValue);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
