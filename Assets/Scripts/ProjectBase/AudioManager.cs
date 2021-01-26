using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class AudioManager : MonoBehaviour
{
    static AudioManager current;

    //[Header("环境声音")]
    //public AudioClip ambientClip;
    //public AudioClip musicClip;

    //[Header("FX音效")]
    //public AudioClip BulletClip;
    //public AudioClip EnemyBulletClip;


    //[Header("Player音效")]
    //public AudioClip[] walkStepClips;
    //public AudioClip jumpClip;
    //public AudioClip deathClip;

    AudioSource ambientSource;
    AudioSource musicSource;
    AudioSource FXSource;
    AudioSource playerSource;
    AudioSource EnemyFXSource;

    public AudioMixerGroup ambientGroup,musicGroup,FXGroup, PlayerGroup, EnemyFXGroup;
    private void Awake()
    {
        if (current != null)
        {
            Destroy(gameObject);
            return;
        }
        current = this;
        DontDestroyOnLoad(gameObject);

        ambientSource = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();
        FXSource = gameObject.AddComponent<AudioSource>();
        playerSource = gameObject.AddComponent<AudioSource>();
        EnemyFXSource = gameObject.AddComponent<AudioSource>();

        ambientSource.outputAudioMixerGroup = ambientGroup;
        musicSource.outputAudioMixerGroup = musicGroup;
        FXSource.outputAudioMixerGroup = FXGroup;
        playerSource.outputAudioMixerGroup = PlayerGroup;
        EnemyFXSource.outputAudioMixerGroup = EnemyFXGroup;

        
    }

    private void Start()
    {
        MusicAudio("BackGroundMisuc", 0.5f, true);
        AmbientAudio("brook", 0.5f, true);
    }

    public static void AmbientAudio(string name, float volume = 0.5f, bool isLoop = false)
    {
        ResMgr.GetInstance().LoadAsync<AudioClip>("Sound/" + name, (clip) =>
        {
            
            current.ambientSource.loop = isLoop;
            current.ambientSource.volume = volume;
            current.ambientSource.clip = clip;
            current.ambientSource.Play();
        });
    }
    public static void AmbientAudio(AudioClip clip, float volume = 0.5f, bool isLoop = false)
    {
        current.ambientSource.loop = isLoop;
        current.ambientSource.volume = volume;
        current.ambientSource.clip = clip;
        current.ambientSource.Play();
    }


    public static void SmoothChangeAmbientAudio(AudioClip clip,float volume, float fadeInTime)
    {
        current.StartCoroutine(SmoothChange(current.ambientSource, clip, volume,fadeInTime));
    }

    public static void SmoothChangeAmbientAudio(string name, float volume, float fadeInTime)
    {
        ResMgr.GetInstance().LoadAsync<AudioClip>("Sound/" + name, (clip) =>
        {
            current.StartCoroutine(SmoothChange(current.ambientSource, clip, volume,fadeInTime));
        });
        
    }

   
    public static void SmoothChangeMusicAudio(AudioClip clip, float volume,float fadeInTime)
    {
        current.StartCoroutine(SmoothChange(current.musicSource, clip, volume, fadeInTime));
    }

    public static void SmoothChangeMusicAudio(string name, float volume,float fadeInTime)
    {
        ResMgr.GetInstance().LoadAsync<AudioClip>("Sound/" + name, (clip) =>
        {
            current.StartCoroutine(SmoothChange(current.musicSource, clip, volume,fadeInTime));
        });

    }

    public static void MusicAudio(string name, float volume = 0.5f, bool isLoop = false)
    {
        ResMgr.GetInstance().LoadAsync<AudioClip>("Sound/" + name, (clip) =>
        {
            current.musicSource.loop = isLoop;
            current.musicSource.volume = volume;
            current.musicSource.clip = clip;
            current.musicSource.Play();
        });
    }

    public static void PlayerAudio(string name, float volume = 0.5f, bool isLoop = false)
    {

        ResMgr.GetInstance().LoadAsync<AudioClip>("Sound/" + name, (clip) =>
        {
            current.playerSource.loop = isLoop;
            current.playerSource.volume = volume;
            current.playerSource.clip = clip;
            current.playerSource.Play();

        });
    }

    public static void PlayerFxAudio(string name, float volume = 0.5f, bool isLoop = false)
    {
        ResMgr.GetInstance().LoadAsync<AudioClip>("Sound/" + name, (clip) =>
        {
            current.FXSource.loop = isLoop;
            current.FXSource.volume = volume;
            current.FXSource.clip = clip;
            current.FXSource.Play();            
        });
    }

    public static void PlayerFxAudio(AudioClip clip, float volume = 0.5f, bool isLoop = false)
    {
        current.FXSource.loop = isLoop;
        current.FXSource.volume = volume;
        current.FXSource.clip = clip;
        current.FXSource.Play();
    }
    


    public static void EnemyFXAudio(string name, float volume = 0.5f, bool isLoop = false)
    {
        ResMgr.GetInstance().LoadAsync<AudioClip>("Sound/" + name, (clip) =>
        {
            current.EnemyFXSource.loop = isLoop;
            current.EnemyFXSource.volume = volume;
            current.EnemyFXSource.clip = clip;
            current.EnemyFXSource.Play();            
        });
    }

    public static void EnemyFXAudio(AudioClip clip, float volume = 0.5f, bool isLoop = false)
    {
        current.EnemyFXSource.loop = isLoop;
        current.EnemyFXSource.volume = volume;
        current.EnemyFXSource.clip = clip;
        current.EnemyFXSource.Play();
    }

    private static IEnumerator SmoothChange(AudioSource audioSource, AudioClip clip, float nextVolume,float fadeIn)
    {
        Debug.Log(clip.name);
        float volume = audioSource.volume;
        while (volume > 0f)
        {
            volume -= 0.01f;
            audioSource.volume = volume;
            yield return new WaitForSeconds(0.01f);
        }

        audioSource.loop = true;
        audioSource.volume = volume;
        audioSource.clip = clip;
        audioSource.Play();

        while (volume < nextVolume)
        {
            volume += 0.01f;
            audioSource.volume = volume;
            yield return new WaitForSeconds(fadeIn);
        }
    }

}
