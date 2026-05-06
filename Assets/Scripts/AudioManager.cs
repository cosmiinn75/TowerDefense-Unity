using Unity.VisualScripting;
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
    }
    public void SetSFXVolume(float sliderValue)
    {
        float decibel = sliderValue > 0 ? Mathf.Log10(sliderValue) * 20 : -80f;
        audioMixer.SetFloat("SFXVolume", decibel);
    }
    public void SetMusicVolume(float sliderValue)
    {
        float decibel = sliderValue > 0 ? Mathf.Log10(sliderValue) * 20 : -80f;
        audioMixer.SetFloat("MusicVolume", decibel);
    }

}
