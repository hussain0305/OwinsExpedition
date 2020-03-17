using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchParticles : MonoBehaviour
{
    public int ParticlesPerTouch;
    public Color Color_1;
    public Color Color_2;
    public GameObject Particle;

    private Touch Gesture;
    private Vector2 TouchPosition;
    private Vector3 CurrentFingerPosition;
    private GameObject HP;

    private void Start()
    {
        if(PlayerPrefs.GetInt("BeautifulGraphics", 1) == 0)
        {
            Destroy(this.gameObject);
        }
        StartCoroutine(ParticlesOnTouch());
    }

    public void SpawnParticleHere()
    {
        for(int loop = 0; loop < ParticlesPerTouch; loop++)
        {
            HP = ObjectPooler_Canvas.CanvasObjectPool.SpawnFromPool(Particle.name, GetParticlePosition(), transform.rotation);
            HP.GetComponent<FireParticle_Touch>().SetParticleColor(Random.Range(1, 10) < 5 ? Color_1 : Color_2);
        }
    }

    public Vector3 GetParticlePosition()
    {
        return new Vector3(CurrentFingerPosition.x + Random.Range(1, 50), CurrentFingerPosition.y + Random.Range(1, 50), CurrentFingerPosition.z + Random.Range(1, 50));
    }

    IEnumerator ParticlesOnTouch()
    {
        while (true)
        {
            if (Input.touches.Length > 0)
            {
                CurrentFingerPosition = Input.GetTouch(0).position;
                Invoke("SpawnParticleHere", 0.1f);
                //SpawnParticleHere();
            }
            yield return new WaitForSeconds(0.05f);
        }
    }
}
