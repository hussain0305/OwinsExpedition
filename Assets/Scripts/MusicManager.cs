using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip Intro;
    public AudioClip MainLoop;

    private AudioSource MusicPlayer;


    private void Awake()
    {
        Destroy(GameObject.FindGameObjectWithTag("MenuMusic"));
    }

    // Start is called before the first frame update
    void Start()
    {
        MusicPlayer = gameObject.AddComponent<AudioSource>();
        MusicPlayer.volume = 0.5f;

        MusicPlayer.clip = Intro;
        MusicPlayer.Play();

        StartCoroutine(CheckForIntroEnd());

        SetListenerAttributes();
    }

    public void StartMainLoop()
    {
        MusicPlayer.clip = MainLoop;
        MusicPlayer.loop = true;
        MusicPlayer.Play();
    }

    public void SetMusicVolume(float Vol)
    {
        MusicPlayer.volume = Vol;
    }

    void SetListenerAttributes()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("VolumeLevel", 1);
        if(PlayerPrefs.GetInt("IsMuted") == 1){
            AudioListener.volume = 0;
        }
    }

    IEnumerator CheckForIntroEnd()
    {
        while (MusicPlayer.isPlaying)
        {
            yield return null;
        }
        StartMainLoop();
        yield return new WaitForSeconds(1);
    }
}
