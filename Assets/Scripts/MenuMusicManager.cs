using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMusicManager : MonoBehaviour
{
    public static MenuMusicManager MMM;
    public AudioClip MenuTrack;

    private AudioSource MenuTrackSource;

    // Start is called before the first frame update
    void Start()
    {
        if (!MMM)
        {
            DontDestroyOnLoad(gameObject);
            MMM = this;
        }
        else if (MMM != this)
        {
            Destroy(gameObject);
        }

        PlayMenuMusic();
    }
    
    void PlayMenuMusic()
    {
        SetListenerAttributes();

        MenuTrackSource = gameObject.AddComponent<AudioSource>();
        MenuTrackSource.clip = MenuTrack;
        MenuTrackSource.loop = true;
        MenuTrackSource.Play();
    }

    void SetListenerAttributes()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("VolumeLevel", 1);
        if (PlayerPrefs.GetInt("IsMuted") == 1)
        {
            AudioListener.volume = 0;
        }
    }
    
    IEnumerator TurnMusicOn(AudioSource CurrentSource)
    {
        while (CurrentSource.volume < 0.96)
        {
            CurrentSource.volume += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator TurnMusicOff(AudioSource CurrentSource)
    {
        while (CurrentSource.volume > 0.04)
        {
            CurrentSource.volume -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
    }

}
