using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FireParticle_Touch : MonoBehaviour
{
    public float LifeSpan = 1;
    public Vector2 MinVelocity = new Vector2(-10f, -10f);
    public Vector2 MaxVelocity = new Vector2(10f, 10f);

    private float ActualLifeSpan;
    private float AlphaValue;
    private Color CurrentColor;
    private Vector3 OriginalScale;

    Image ImageComp;
    Color OriginalColor;

    Vector2 Velocity;

    void Awake()
    {
        ImageComp = GetComponent<Image>();
        OriginalColor = ImageComp.color;
        OriginalScale = transform.localScale;
    }
    void OnEnable()
    {
        Velocity = new Vector2(Random.Range(MinVelocity.x, MaxVelocity.x), Random.Range(MinVelocity.y, MaxVelocity.y));
        ActualLifeSpan = LifeSpan * Random.Range(0.25f, 0.5f); //(0.6f, 0.9f);
        AlphaValue = 1;

        transform.localScale = OriginalScale * Random.Range(0, 1.5f);

        StopAllCoroutines();
        StartCoroutine(ParticleTransparency());

        Invoke("DisableParticle", ActualLifeSpan);
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
        if (Random.Range(1, 10) < 8)
        {
            ImageComp.color = PartColor;
            OriginalColor = PartColor;
        }
        else
        {
            ImageComp.color = Color.gray;
            OriginalColor = Color.gray;
        }
    }

    public void SetParticleVelocity(float NewVelocityFactor)
    {
        Velocity = Velocity * NewVelocityFactor;
    }

    public void SetParticleVelocity(Vector3 NewVelocity)
    {
        Velocity = NewVelocity;
    }

    IEnumerator ParticleTransparency()
    {
        while (AlphaValue > 0.1f)
        {
            AlphaValue -= 0.1f;
            transform.localScale = transform.localScale / 1.2f;
            ImageComp.color = new Color(ImageComp.color.r, ImageComp.color.g, ImageComp.color.b, AlphaValue);
            transform.position = transform.position + new Vector3(Velocity.x, Velocity.y, 0);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
