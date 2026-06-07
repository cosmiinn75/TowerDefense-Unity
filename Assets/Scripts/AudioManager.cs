using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    [Header("Audio Source")]
    public AudioSource sfxSource;
    public AudioSource musicSource;
    [Header("Audio Clips")]
    public AudioClip cannonClip;
    public AudioClip archerClip;
    public AudioClip magicClip;
    public AudioClip backgroundClip;
    public AudioClip buttonClick;
    public AudioClip lockedClick;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        musicSource.clip = backgroundClip;
        musicSource.Play();
        float masterVol = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
        float musicVol = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        float sfxVol = PlayerPrefs.GetFloat("SFXVolume", 0.75f);
        SetMasterVolume(masterVol);
        SetMusicVolume(musicVol);
        SetSFXVolume(sfxVol);
    }

    public void PlaySFX(AudioClip clip)
    {
        if(clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip); 
        }
    }

    public void SetMasterVolume(float sliderValue)
    {
        float decibel = sliderValue > 0 ? Mathf.Log10(sliderValue) * 20 : -80f;
        audioMixer.SetFloat("MasterVolume", decibel);
        PlayerPrefs.SetFloat("MasterVolume", sliderValue);
    }
    public void SetSFXVolume(float sliderValue)
    {
        float decibel = sliderValue > 0 ? Mathf.Log10(sliderValue) * 20 : -80f;
        audioMixer.SetFloat("SFXVolume", decibel);
        PlayerPrefs.SetFloat("SFXVolume", sliderValue);
    }
    public void SetMusicVolume(float sliderValue)
    {
        float decibel = sliderValue > 0 ? Mathf.Log10(sliderValue) * 20 : -80f;
        audioMixer.SetFloat("MusicVolume", decibel);
        PlayerPrefs.SetFloat("MusicVolume", sliderValue);
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
    public void PlayClick()
    {
        sfxSource.PlayOneShot(buttonClick);
    }
    public void PlayLockedClick()
    {
        sfxSource.PlayOneShot(lockedClick);
    }

    public void PlayMusic()
    {
        if (!musicSource.isPlaying)
        {
            musicSource.clip = backgroundClip;
            musicSource.Play();
        }
    }
}
